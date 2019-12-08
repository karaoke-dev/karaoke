// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Skinning.Components
{
    public class NoteSkin
    {
        public string Name { get; set; }

        public Color4 NoteColor { get; set; }

        public Color4 BlinkColor { get; set; }

        public Color4 TextColor { get; set; }

        public bool BoldText { get; set; }
    }
}
