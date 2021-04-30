// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class EditorNotePlayfield : ScrollingNotePlayfield
    {
        public EditorNotePlayfield(int columns)
            : base(columns)
        {
            BackgroundLayer.AddRange(new Drawable[]
            {
                new Box
                {
                    Depth = 1,
                    Name = "Background",
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.5f
                },
            });
        }
    }
}
