// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose;

public partial class DetailLyricList : LyricList
{
    [Resolved]
    private LyricEditorColourProvider colourProvider { get; set; } = null!;

    private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();

    private Drawable? background;

    public DetailLyricList()
    {
        AdjustSkin(skin =>
        {
            skin.FontSize = 15;
        });

        bindableMode.BindValueChanged(e =>
        {
            redrawBackground();
        });
    }

    [BackgroundDependencyLoader]
    private void load(ILyricEditorState state)
    {
        bindableMode.BindTo(state.BindableMode);

        redrawBackground();
    }

    private void redrawBackground()
    {
        if (background != null)
            RemoveInternal(background, true);

        background = createBackground(colourProvider, bindableMode.Value);
        if (background == null)
            return;

        AddInternal(background.With(x =>
        {
            x.RelativeSizeAxes = Axes.Both;
            x.Depth = int.MaxValue;
        }));
    }

    private static Drawable createBackground(LyricEditorColourProvider colourProvider, LyricEditorMode mode)
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

    protected override DrawableLyricList CreateDrawableLyricList()
        => new DrawableDetailLyricList();

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

        protected override Row CreateEditRow(Lyric lyric)
            => new EditLyricDetailRow(lyric);

        protected override Row GetCreateNewLyricRow()
            => new CreateNewLyricDetailRow();
    }
}
