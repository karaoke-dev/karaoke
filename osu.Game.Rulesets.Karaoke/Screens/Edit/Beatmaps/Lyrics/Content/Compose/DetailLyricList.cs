// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose;

public partial class DetailLyricList : LyricList
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
        bool containsHandler = mode == LyricEditorMode.EditText;

        const float timing_base_width = LYRIC_LIST_PADDING + DetailRow.TIMING_WIDTH;
        float timingWidth = containsHandler ? HANDLER_WIDTH + timing_base_width : timing_base_width;
        return new GridContainer
        {
            ColumnDimensions = new[]
            {
                new Dimension(GridSizeMode.Absolute, timingWidth),
                new Dimension(),
            },
            Content = new[]
            {
                new[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = colourProvider.Background3(mode),
                    },
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = colourProvider.Background4(mode),
                    },
                },
            },
        };
    }

    public partial class DrawableDetailLyricList : DrawableLyricList
    {
        protected override Vector2 Spacing => new();

        protected override bool ScrollToPosition(ICaretPosition caret)
        {
            // should scroll to the target position on every case.
            return true;
        }

        protected override int SkipRows()
        {
            // it's a fixed number for now.
            return 3;
        }

        protected override DrawableLyricListItem CreateLyricListItem(Lyric item)
            => new DrawableDetailLyricListItem(item);

        protected override Row GetCreateNewLyricRow()
            => new CreateNewLyricDetailRow();

        public partial class DrawableDetailLyricListItem : DrawableLyricListItem
        {
            public DrawableDetailLyricListItem(Lyric item)
                : base(item)
            {
            }

            protected override Row CreateEditRow(Lyric lyric)
                => new EditLyricDetailRow(lyric);
        }
    }
}
