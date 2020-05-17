// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Overlays;
using osu.Game.Rulesets.Karaoke.Overlays.Changelog;
using osu.Game.Tests.Visual;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Tests.Overlays
{
    [TestFixture]
    public class TestSceneKaraokeChangeLogOverlay : OsuTestScene
    {
        private TestChangelogOverlay changelog;

        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(ChangelogHeader),
            typeof(ChangelogContent),
            typeof(ChangelogListing),
            typeof(ChangelogSingleBuild),
            typeof(ChangelogBuild),
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
            AddAssert(@"listing displayed", () => changelog.Current.Value == null);
        }

        private class TestChangelogOverlay : KaraokeChangelogOverlay
        {
        }
    }
}
