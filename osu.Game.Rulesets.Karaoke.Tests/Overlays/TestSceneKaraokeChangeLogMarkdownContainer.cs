// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;
using osu.Game.Rulesets.Karaoke.Overlays.Changelog;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Overlays
{
    [TestFixture]
    public class TestSceneKaraokeChangeLogMarkdownContainer : OsuTestScene
    {
        private ChangeLogMarkdownContainer changelog;

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            Child = changelog = new ChangeLogMarkdownContainer(new KaraokeChangelogBuild("andy840119", "osu-karaoke.github.io", "slave/change-url-to-tag")
            {
                Path = "changelog/2020.0620",
                RootUrl = "https://github.com/andy840119/osu-Karaoke.github.io/tree/slave/change-url-to-tag/changelog/2020.0620"
            });
            Child.RelativeSizeAxes = Axes.Both;
        });

        [Test]
        public void ShowWithNoFetch()
        {
            AddStep(@"Show", () => changelog.Show());
        }
    }
}
