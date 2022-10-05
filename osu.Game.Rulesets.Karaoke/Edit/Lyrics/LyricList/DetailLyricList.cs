// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList
{
    public class DetailLyricList : BaseLyricList
    {
        public DetailLyricList()
        {
            AdjustSkin(skin =>
            {
                skin.FontSize = 15;
            });
        }

        protected override DrawableLyricList CreateDrawableLyricList()
            => new DrawableDetailLyricList();

        protected override Drawable CreateBackground(LyricEditorColourProvider colourProvider, LyricEditorMode mode)
        {
            bool containsHandler = mode == LyricEditorMode.Texting;

            const float timing_base_width = LYRIC_LIST_PADDING + DetailRow.TIMIMG_WIDTH;
            float timingWidth = containsHandler ? DrawableLyricListItem.HANDLER_WIDTH + timing_base_width : timing_base_width;
            return new GridContainer
            {
                ColumnDimensions = new[]
                {
                    new Dimension(GridSizeMode.Absolute, timingWidth),
                    new Dimension()
                },
                Content = new[]
                {
                    new[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = colourProvider.Background3(mode)
                        },
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = colourProvider.Background4(mode)
                        },
                    }
                }
            };
        }
    }
}
