// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using Octokit;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Input.Bindings;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;
using osu.Game.Rulesets.Karaoke.Overlays.Changelog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace osu.Game.Rulesets.Karaoke.Overlays
{
    public class KaraokeChangelogOverlay : FullscreenOverlay
    {
        public readonly Bindable<KaraokeChangelogBuild> Current = new Bindable<KaraokeChangelogBuild>();

        protected ChangelogHeader Header;

        private Container<ChangelogContent> content;

        private SampleChannel sampleBack;

        private List<KaraokeChangelogBuild> builds;

        private readonly string organizationName;

        private string projectName => $"{organizationName}.github.io";

        public KaraokeChangelogOverlay(string organization)
            : base(OverlayColourScheme.Purple)
        {
            organizationName = organization;
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = ColourProvider.Background4,
                },
                new OverlayScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    ScrollbarVisible = false,
                    Child = new ReverseChildIDFillFlowContainer<Drawable>
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Vertical,
                        Children = new Drawable[]
                        {
                            Header = new ChangelogHeader
                            {
                                ListingSelected = ShowListing,
                            },
                            content = new Container<ChangelogContent>
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                            }
                        },
                    },
                },
            };

            sampleBack = audio.Samples.Get(@"UI/generic-select-soft");

            Header.Build.BindTo(Current);

            Current.BindValueChanged(e =>
            {
                if (e.NewValue != null)
                    loadContent(new ChangelogSingleBuild(e.NewValue));
                else
                {
                    // loading empty change log
                    loadContent(new ChangelogListing(builds));
                }
            });
        }

        public void ShowListing()
        {
            Current.Value = null;
            Show();
        }

        /// <summary>
        /// Fetches and shows a specific build from a specific update stream.
        /// </summary>
        /// Must contain at least <see cref="KaraokeChangelogBuild"/> and
        public void ShowBuild([NotNull] KaraokeChangelogBuild build)
        {
            if (build == null)
                throw new ArgumentNullException(nameof(build));

            Current.Value = build;
            Show();
        }

        public override bool OnPressed(GlobalAction action)
        {
            switch (action)
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
            }

            return false;
        }

        protected override void PopIn()
        {
            base.PopIn();

            if (initialFetchTask == null)
                // fetch and refresh to show listing, if no other request was made via Show methods
                performAfterFetch(() => Current.TriggerChange());
        }

        private Task initialFetchTask;

        private void performAfterFetch(Action action) => fetchListing()?.ContinueWith(_ =>
            Schedule(action), TaskContinuationOptions.OnlyOnRanToCompletion);

        private Task fetchListing()
        {
            if (initialFetchTask != null)
                return initialFetchTask;

            return initialFetchTask = Task.Run(async () =>
            {
                var tcs = new TaskCompletionSource<bool>();

                var client = new GitHubClient(new ProductHeaderValue(organizationName));
                var reposAscending = await client.Repository.Content.GetAllContents(organizationName, projectName, "changelog");

                if (reposAscending.Any())
                {
                    builds = reposAscending.Reverse().Where(x=>x.Type == ContentType.Dir).Select(x => new KaraokeChangelogBuild(organizationName, projectName)
                    {
                        RootUrl = x.HtmlUrl,
                        Path = x.Path,
                        DisplayVersion = x.Name
                    }).ToList();

                    foreach (var build in builds)
                    {
                        build.Versions.Previous = builds.GetPrevious(build);
                        build.Versions.Next = builds.GetNext(build);
                    }

                    tcs.SetResult(true);
                }
                else
                {
                    tcs.SetResult(false);
                }

                await tcs.Task;
            });
        }

        private CancellationTokenSource loadContentCancellation;

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
}
