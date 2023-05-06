// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;

public partial class DrawableTextIndex : RightTriangle
{
    private TextIndex.IndexState state;

    public TextIndex.IndexState State
    {
        get => state;
        set
        {
            state = value;

            RightAngleDirection = TextIndexUtils.GetValueByState(state, TriangleRightAngleDirection.BottomLeft, TriangleRightAngleDirection.BottomRight);
        }
    }
}
