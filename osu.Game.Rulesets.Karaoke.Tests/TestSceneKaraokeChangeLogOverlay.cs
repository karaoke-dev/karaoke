// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using Octokit;
using osu.Game.Rulesets.Karaoke.Overlays;
using osu.Game.Rulesets.Karaoke.Overlays.Changelog;
using osu.Game.Tests.Visual;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    [TestFixture]
    public class TestSceneKaraokeChangeLogOverlay : OsuTestScene
    {
        private TestChangelogOverlay changelog;

        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(ChangelogHeader),
            typeof(ChangelogContent),
        };

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            Child = changelog = new TestChangelogOverlay();
        });

        [Test]
        public void ShowWithNoFetch()
        {
            AddStep(@"Show", () => changelog.Show());
            AddStep(@"Hide", () => changelog.Hide());
        }

        [TestCase]
        public async Task GetChangeLogDirectory()
        {
            var client = new GitHubClient(new ProductHeaderValue("osu-karaoke"));
            var reposAscending = await client.Repository.Content.GetAllContents("osu-karaoke", "osu-Karaoke.github.io","changelog");

            //reposAscending[0].DownloadUrl;
        }

        private class TestChangelogOverlay : KaraokeChangelogOverlay
        {
            public new ChangelogHeader Header => base.Header;
        }
    }
}
