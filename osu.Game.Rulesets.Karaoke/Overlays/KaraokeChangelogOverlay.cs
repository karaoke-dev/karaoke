// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Game.Input.Bindings;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Online.API.Requests;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;
using osu.Game.Rulesets.Karaoke.Overlays.Changelog;
using osu.Game.Rulesets.Karaoke.Overlays.Changelog.Sidebar;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Overlays;

public partial class KaraokeChangelogOverlay : OnlineOverlay<ChangelogHeader>
{
    public override bool IsPresent => base.IsPresent || Scheduler.HasPendingTasks;

    [Cached]
    public readonly Bindable<APIChangelogBuild?> Current = new();

    private readonly Container sidebarContainer;
    private readonly ChangelogSidebar sidebar;
    private readonly Container content;

    private Sample? sampleBack;

    private APIChangelogIndex? index;

    private readonly string organizationName;
    private readonly string branchName;

    private string projectName => $"{organizationName}.github.io";

    public KaraokeChangelogOverlay(string organization, string branch = "master")
        : base(OverlayColourScheme.Purple, false)
    {
        organizationName = organization;
        branchName = branch;

        Child = new GridContainer
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            RowDimensions = new[]
            {
                new Dimension(GridSizeMode.AutoSize),
            },
            ColumnDimensions = new[]
            {
                new Dimension(GridSizeMode.AutoSize),
                new Dimension(),
            },
            Content = new[]
            {
                new Drawable[]
                {
                    sidebarContainer = new Container
                    {
                        AutoSizeAxes = Axes.X,
                        Child = sidebar = new ChangelogSidebar(),
                    },
                    content = new Container
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                    },
                },
            },
        };
    }

    [BackgroundDependencyLoader]
    private void load(AudioManager audio)
    {
        Header.Build.BindTo(Current);

        sampleBack = audio.Samples.Get("UI/generic-select-soft");

        Current.BindValueChanged(e =>
        {
            if (e.NewValue != null)
            {
                loadContent(new ChangelogSingleBuild(e.NewValue));
            }
            else if (index != null)
            {
                loadContent(new ChangelogListing(index.PreviewBuilds));
            }
            else
            {
                // todo: should show oops content.
            }
        });
    }

    protected override void UpdateAfterChildren()
    {
        base.UpdateAfterChildren();
        sidebarContainer.Height = DrawHeight;
        sidebarContainer.Y = (float)Math.Clamp(ScrollFlow.Current - Header.DrawHeight, 0, Math.Max(ScrollFlow.ScrollContent.DrawHeight - DrawHeight - Header.DrawHeight, 0));
    }

    protected override ChangelogHeader CreateHeader() => new()
    {
        ListingSelected = ShowListing,
    };

    protected override Color4 BackgroundColour => ColourProvider.Background4;

    public void ShowListing()
    {
        Current.Value = null;
        Show();
    }

    /// <summary>
    /// Fetches and shows a specific build from a specific update stream.
    /// </summary>
    /// <param name="build"> Singer <see cref="APIChangelogBuild"/>.</param>
    public void ShowBuild(APIChangelogBuild build)
    {
        ArgumentNullException.ThrowIfNull(build);

        Current.Value = build;
        Show();
    }

    public override bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
    {
        switch (e.Action)
        {
            case GlobalAction.Back:
                if (Current.Value == null)
                {
                    Hide();
                }
                else
                {
                    Current.Value = null;
                    sampleBack?.Play();
                }

                return true;

            default:
                return false;
        }
    }

    protected override void PopIn()
    {
        base.PopIn();

        // fetch and refresh to show listing, if no other request was made via Show methods
        if (initialFetchTask == null)
        {
            performAfterFetch(() =>
            {
                Current.TriggerChange();

                if (index == null)
                    return;

                sidebar.Year.Value = index.Years.Max();
                sidebar.Metadata.Value = index;
            });
        }
    }

    private Task? initialFetchTask;

    private void performAfterFetch(Action action) => fetchListing()
        .ContinueWith(_ => Schedule(action), TaskContinuationOptions.OnlyOnRanToCompletion);

    private Task fetchListing()
    {
        if (initialFetchTask != null)
            return initialFetchTask;

        return initialFetchTask = Task.Run(async () =>
        {
            var tcs = new TaskCompletionSource<bool>();

            var req = new GetChangelogRequest();

            req.Success += res => Schedule(() =>
            {
                index = res;
                tcs.SetResult(true);
            });

            req.Failure += e =>
            {
                initialFetchTask = null;
                tcs.SetException(e);
            };

            await req.Perform().ConfigureAwait(false);

            return tcs.Task;
        }).Unwrap();
    }

    private CancellationTokenSource? loadContentCancellation;

    private void loadContent(ChangelogContent newContent)
    {
        content.FadeTo(0.2f, 300, Easing.OutQuint);

        loadContentCancellation?.Cancel();

        LoadComponentAsync(newContent, c =>
        {
            content.FadeIn(300, Easing.OutQuint);

            // if content changed view version
            c.BuildSelected = ShowBuild;
            content.Child = c;
        }, (loadContentCancellation = new CancellationTokenSource()).Token);
    }
}
