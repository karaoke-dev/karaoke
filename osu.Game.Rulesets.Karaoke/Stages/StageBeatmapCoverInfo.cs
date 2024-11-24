// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Stages.Drawables;

namespace osu.Game.Rulesets.Karaoke.Stages;

public class StageBeatmapCoverInfo : StageSprite
{
    public override Drawable CreateDrawable() => new DrawableStageBeatmapCoverInfo(this);
}
