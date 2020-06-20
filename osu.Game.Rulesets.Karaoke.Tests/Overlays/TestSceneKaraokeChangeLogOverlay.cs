// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Overlays;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Overlays
{
    [TestFixture]
    public class TestSceneKaraokeChangeLogOverlay : OsuTestScene
    {
        private TestChangelogOverlay changelog;

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
