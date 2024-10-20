// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Stages;
using osu.Game.Rulesets.Karaoke.Stages.Drawables;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Stages.Drawables;

public partial class TestSceneDrawableStageBeatmapCoverInfo : OsuTestScene
{
    private readonly DrawableStageBeatmapCoverInfo beatmapCoverInfo;

    public TestSceneDrawableStageBeatmapCoverInfo()
    {
        Add(beatmapCoverInfo = new DrawableStageBeatmapCoverInfo(new StageBeatmapCoverInfo())
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
