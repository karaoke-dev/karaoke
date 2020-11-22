// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.DragFile.Components
{
    public class DrawableDragFile : Container
    {
        public DrawableDragFile()
        {
            Masking = true;
            BorderThickness = 5;
            BorderColour = Colour4.White;

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Name = "Printing border stupid box",
                    RelativeSizeAxes = Axes.Both,
                    AlwaysPresent = true,
                    Alpha = 0
                },
                new FillFlowContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    AutoSizeAxes = Axes.Both,
                    Spacing = new Vector2(10),
                    Direction = FillDirection.Horizontal,
                    Children = new Drawable[]
                    {
                        new SpriteIcon
                        {
                            Size = new Vector2(32),
                            Icon = FontAwesome.Solid.Upload
                        },
                        new SpriteText
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Text = "Drag file into here"
                        }
                    }
                }
            };
        }
    }
}
