// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
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

        AddInternal(new DetailLyricListBackground
        {
            RelativeSizeAxes = Axes.Both,
            Depth = int.MaxValue,
        });
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

    public partial class DetailLyricListBackground : CompositeDrawable
    {
        private readonly Box infoBackground;
        private readonly Box lyricBackground;

        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
        private readonly IBindable<bool> bindableSelecting = new Bindable<bool>();

        public DetailLyricListBackground()
        {
            InternalChildren = new Drawable[]
            {
                infoBackground = new Box
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    RelativeSizeAxes = Axes.Y,
                },
                lyricBackground = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Depth = int.MaxValue,
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state, ILyricSelectionState lyricSelectionState, LyricEditorColourProvider colourProvider)
        {
            bindableMode.BindTo(state.BindableMode);
            bindableSelecting.BindTo(lyricSelectionState.Selecting);

            bindableMode.BindValueChanged(e =>
            {
                resizeBackground();
                updateColour(colourProvider, e.NewValue);
            }, true);

            bindableSelecting.BindValueChanged(_ =>
            {
                resizeBackground();
            }, true);
        }

        private void updateColour(LyricEditorColourProvider colourProvider, LyricEditorMode mode)
        {
            infoBackground.Colour = colourProvider.Background3(mode);
            lyricBackground.Colour = colourProvider.Background4(mode);
        }

        private void resizeBackground()
        {
            bool showDragHandler = ShowDragHandler(bindableMode.Value, bindableSelecting.Value);
            bool selecting = bindableSelecting.Value;

            float handlerWidth = showDragHandler ? HANDLER_WIDTH : 0;
            float selectingAreaWidth = selecting ? Row.SELECT_AREA_WIDTH : 0;

            infoBackground.Width = LYRIC_LIST_PADDING + handlerWidth + selectingAreaWidth + DetailRow.TIMING_WIDTH;
        }
    }
}
