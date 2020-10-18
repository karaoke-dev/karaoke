// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.RubyRomaji
{
    public class RubyRomajiEditSection : Container
    {
        public RubyRomajiEditSection(EditorBeatmap editorBeatmap)
        {
            Padding = new MarginPadding(10);

            Child = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                ColumnDimensions = new[]
                {
                    new Dimension(GridSizeMode.Relative, 0.4f)
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        new Box
                        {
                            Width = 200,
                            Height = 1000,
                            Colour = Colour4.Red
                        }
                    }
                }
            };
        }
    }
}
