// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace osu.Game.Rulesets.Karaoke.Graphics.Shapes
{
    public class CornerBackground : Container
    {
        public CornerBackground()
        {
            Masking = true;
            CornerRadius = 5;
            AddInternal(new Box { RelativeSizeAxes = Axes.Both });
        }
    }
}
