// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class LegacySnow : Drawable
    {
        public LegacySnow()
        {
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;
            Colour = Color4.White;

            // TODO : need clean-up but not now.
        }
    }
}
