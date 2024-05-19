// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;
using osu.Game.Rulesets.Karaoke.Overlays.Changelog;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Overlays.Changelog;

[TestFixture]
public partial class TestSceneKaraokeChangeLogMarkdownContainer : OsuTestScene
{
    private ChangeLogMarkdownContainer markdownContainer = null!;

    [Cached]
    private readonly OverlayColourProvider overlayColour = new(OverlayColourScheme.Orange);

    [SetUp]
    public void SetUp() => Schedule(() =>
    {
        var build = new APIChangelogBuild
        {
            DocumentUrl = "https://raw.githubusercontent.com/karaoke-dev/karaoke-dev.github.io/master/content/changelog/2020.0620",
            RootUrl = "https://github.com/karaoke-dev/karaoke-dev.github.io/tree/master/content/changelog/2020.0620",
            Content = "---\ntitle: \"2020.0620\"\ndate: 2020-06-20\n---\n\n## Achievement\n\n- Might support convert `UTAU`/`SynthV` project into karaoke beatmap in future. [karaoke](#100#101@andy840119)",
        };

        Children = new Drawable[]
        {
            new Box
            {
                Colour = overlayColour.Background5,
                RelativeSizeAxes = Axes.Both,
            },
            new BasicScrollContainer
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(20),
                Child = markdownContainer = new ChangeLogMarkdownContainer(build)
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                },
            },
        };
    });

    [Test]
    public void TestShowWithNoFetch()
    {
        AddStep("Show", () => markdownContainer.Show());
    }
}
