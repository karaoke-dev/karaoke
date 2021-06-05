// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Octokit;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Game.Input.Bindings;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;
using osu.Game.Rulesets.Karaoke.Overlays.Changelog;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Overlays
{
    public class KaraokeChangelogOverlay : OnlineOverlay<ChangelogHeader>
    {
        public override bool IsPresent => base.IsPresent || Scheduler.HasPendingTasks;

        public readonly Bindable<APIChangelogBuild> Current = new Bindable<APIChangelogBuild>();

        private Sample sampleBack;

        private List<APIChangelogBuild> builds;

        private readonly string organizationName;
        private readonly string branchName;

        private string projectName => $"{organizationName}.github.io";

        public KaraokeChangelogOverlay(string organization, string branch = "master")
            : base(OverlayColourScheme.Purple, false)
        {
            organizationName = organization;
            branchName = branch;
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            Header.Build.BindTo(Current);

            sampleBack = audio.Samples.Get(@"UI/generic-select-soft");

            Current.BindValueChanged(e =>
            {
                if (e.NewValue != null)
                    loadContent(new ChangelogSingleBuild(e.NewValue));
                else
                {
                    loadContent(new ChangelogListing(builds));
                }
            });
        }

        protected override ChangelogHeader CreateHeader() => new ChangelogHeader
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
        /// <param name="build">Must contain at least <see cref="APIUpdateStream.Name"/> and
        /// <see cref="APIChangelogBuild.Version"/>. If <see cref="APIUpdateStream.DisplayName"/> and
        /// <see cref="APIChangelogBuild.DisplayVersion"/> are specified, the header will instantly display them.</param>
        public void ShowBuild([NotNull] APIChangelogBuild build)
        {
            if (build == null) throw new ArgumentNullException(nameof(build));

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
                var reposAscending = await client.Repository.Content.GetAllContentsByRef(organizationName, projectName, "changelog", branchName);

                if (reposAscending.Any())
                {
                    builds = reposAscending.Reverse().Where(x => x.Type == ContentType.Dir).Select(x => new APIChangelogBuild(organizationName, projectName, branchName)
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
            Content.FadeTo(0.2f, 300, Easing.OutQuint);

            loadContentCancellation?.Cancel();

            LoadComponentAsync(newContent, c =>
            {
                Content.FadeIn(300, Easing.OutQuint);

                // if content changed view version
                c.BuildSelected = ShowBuild;
                Content.Child = c;
            }, (loadContentCancellation = new CancellationTokenSource()).Token);
        }
    }
}
