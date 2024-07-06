// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content;

public abstract partial class LyricList : CompositeDrawable
{
    public const float LYRIC_LIST_PADDING = 10;
    public const float HANDLER_WIDTH = 22;

    [Resolved]
    private ILyricsChangeHandler? lyricsChangeHandler { get; set; }

    private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
    private readonly IBindable<bool> bindableSelecting = new Bindable<bool>();

    private readonly GridContainer lyricEditorGridContainer;
    private readonly LyricEditorSkin skin;
    private readonly DrawableLyricList container;
    private readonly ApplySelectingArea applySelectingArea;

    protected LyricList()
    {
        InternalChild = lyricEditorGridContainer = new GridContainer
        {
            RelativeSizeAxes = Axes.Both,
            RowDimensions = new[]
            {
                new Dimension(),
                new Dimension(GridSizeMode.AutoSize),
            },
            Content = new[]
            {
                new Drawable[]
                {
                    new SkinProvidingContainer(skin = new LyricEditorSkin(null))
                    {
                        RelativeSizeAxes = Axes.Both,
                        Padding = new MarginPadding(LYRIC_LIST_PADDING),
                        Child = container = CreateDrawableLyricList().With(x =>
                        {
                            x.RelativeSizeAxes = Axes.Both;
                        }),
                    },
                },
                new Drawable[]
                {
                    applySelectingArea = new ApplySelectingArea(),
                },
            },
        };

        container.OnOrderChanged += (x, nowOrder) =>
        {
            lyricsChangeHandler?.ChangeOrder(nowOrder);
        };

        bindableMode.BindValueChanged(e =>
        {
            updateAddLyricState();
        }, true);

        bindableSelecting.BindValueChanged(e =>
        {
            updateAddLyricState();
            updateApplySelectingArea();
        }, true);
    }

    protected void AdjustSkin(Action<LyricEditorSkin> action)
    {
        action(skin);
    }

    protected abstract DrawableLyricList CreateDrawableLyricList();

    private void updateApplySelectingArea()
    {
        if (bindableSelecting.Value)
        {
            applySelectingArea.Show();
        }
        else
        {
            applySelectingArea.Hide();
        }
    }

    private void updateAddLyricState()
    {
        // display add new lyric only with edit mode.
        bool disableBottomDrawable = bindableMode.Value == LyricEditorMode.EditText && !bindableSelecting.Value;
        container.DisplayBottomDrawable = disableBottomDrawable;
    }

    [BackgroundDependencyLoader]
    private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager, ILyricEditorState state,
                      ILyricSelectionState lyricSelectionState, ILyricsProvider lyricsProvider)
    {
        bindableMode.BindTo(state.BindableMode);
        bindableSelecting.BindTo(lyricSelectionState.Selecting);

        container.Items.BindTo(lyricsProvider.BindableLyrics);
    }

    /// <summary>
    /// Visualises a list of <see cref="Lyric"/>s.
    /// </summary>
    public abstract partial class DrawableLyricList : OrderRearrangeableListContainer<Lyric>
    {
        private readonly IBindable<ICaretPosition?> bindableCaretPosition = new Bindable<ICaretPosition?>();

        protected DrawableLyricList()
        {
            // update selected style to child
            bindableCaretPosition.BindValueChanged(e =>
            {
                var newLyric = e.NewValue?.Lyric;
                if (newLyric == null || !ValueChangedEventUtils.LyricChanged(e))
                    return;

                if (!ScrollToPosition(e.NewValue!))
                    return;

                int skippingRows = SkipRows();
                moveItemToTargetPosition(newLyric, skippingRows);
            });
        }

        protected abstract bool ScrollToPosition(ICaretPosition caret);

        protected abstract int SkipRows();

        protected abstract DrawableLyricListItem CreateLyricListItem(Lyric item);

        protected sealed override OsuRearrangeableListItem<Lyric> CreateOsuDrawable(Lyric item)
            => CreateLyricListItem(item);

        protected sealed override Drawable CreateBottomDrawable()
        {
            return new Container
            {
                // todo: should based on the row's height.
                RelativeSizeAxes = Axes.X,
                Height = 75,
                Padding = new MarginPadding { Left = HANDLER_WIDTH },
                Child = GetCreateNewLyricRow(),
            };
        }

        protected abstract Row GetCreateNewLyricRow();

        [BackgroundDependencyLoader]
        private void load(ILyricCaretState lyricCaretState)
        {
            bindableCaretPosition.BindTo(lyricCaretState.BindableCaretPosition);
        }

        private void moveItemToTargetPosition(Lyric targetLyric, int skippingRows)
        {
            var drawable = getListItem(targetLyric);
            if (drawable == null)
                return;

            float topSpacing = drawable.Height * skippingRows;
            float bottomSpacing = DrawHeight - drawable.Height * (skippingRows + 1);
            ScrollContainer.ScrollIntoViewWithSpacing(drawable, new MarginPadding
            {
                Top = topSpacing,
                Bottom = bottomSpacing,
            });
            return;

            DrawableLyricListItem? getListItem(Lyric? lyric)
                => ListContainer.Children.OfType<DrawableLyricListItem>().FirstOrDefault(x => x.Model == lyric);
        }

        /// <summary>
        /// Visualises a <see cref="Lyric"/> inside a <see cref="DrawableLyricList"/>.
        /// </summary>
        public abstract partial class DrawableLyricListItem : OsuRearrangeableListItem<Lyric>
        {
            [Resolved]
            private ILyricCaretState lyricCaretState { get; set; } = null!;

            private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();

            protected DrawableLyricListItem(Lyric item)
                : base(item)
            {
                bindableMode.BindValueChanged(e =>
                {
                    // Only draggable in edit mode.
                    ShowDragHandle.Value = e.NewValue == LyricEditorMode.EditText;
                }, true);

                DragActive.BindValueChanged(e =>
                {
                    // should mark object as selecting while dragging.
                    lyricCaretState.MoveCaretToTargetPosition(Model);
                });
            }

            protected sealed override Drawable CreateContent() => CreateEditRow(Model);

            protected abstract Row CreateEditRow(Lyric lyric);

            [BackgroundDependencyLoader]
            private void load(ILyricEditorState state)
            {
                bindableMode.BindTo(state.BindableMode);
            }
        }
    }
}
