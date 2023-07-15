// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.UI;

public partial class TestSceneBeatmapCoverInfo : OsuTestScene
{
    private readonly BeatmapCoverInfo beatmapCoverInfo;

    public TestSceneBeatmapCoverInfo()
    {
        Add(beatmapCoverInfo = new BeatmapCoverInfo
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
        });
    }

    [Test]
    public void TestSize()
    {
        AddStep("Small size", () => { beatmapCoverInfo.Size = new Vector2(200); });
        AddStep("Medium size", () => { beatmapCoverInfo.Size = new Vector2(300); });
        AddStep("Large size", () => { beatmapCoverInfo.Size = new Vector2(400); });
    }
}
