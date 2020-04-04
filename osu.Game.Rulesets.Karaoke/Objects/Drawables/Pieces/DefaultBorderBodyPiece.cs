// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.UI.Components;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables.Pieces
{
    public class DefaultBorderBodyPiece : Container
    {
        public const float CORNER_RADIUS = 5;

        public DefaultBorderBodyPiece()
        {
            CornerRadius = CORNER_RADIUS;
            Masking = true;
            Height = DefaultColumnBackground.COLUMN_HEIGHT;
        }
    }
}
