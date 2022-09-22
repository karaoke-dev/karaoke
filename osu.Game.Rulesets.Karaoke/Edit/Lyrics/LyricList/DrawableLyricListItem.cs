// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList
{
    public abstract class DrawableLyricListItem : OsuRearrangeableListItem<Lyric>
    {
        public const float HANDLER_WIDTH = 22;

        private Box background;

        [Resolved]
        private LyricEditorColourProvider colourProvider { get; set; }

        [Resolved]
        private ILyricCaretState lyricCaretState { get; set; }

        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
        private readonly IBindable<ICaretPosition> bindableHoverCaretPosition = new Bindable<ICaretPosition>();
        private readonly IBindable<ICaretPosition> bindableCaretPosition = new Bindable<ICaretPosition>();

        protected DrawableLyricListItem(Lyric item)
            : base(item)
        {
            bindableMode.BindValueChanged(e =>
            {
                // Only draggable in edit mode.
                ShowDragHandle.Value = e.NewValue == LyricEditorMode.Texting;

                OnModeChanged();
            }, true);

            bindableHoverCaretPosition.BindValueChanged(_ =>
            {
                updateBackgroundColour();
            });

            bindableCaretPosition.BindValueChanged(e =>
            {
                updateBackgroundColour();

                OnCaretPositionChanged(e.NewValue);
            });

            DragActive.BindValueChanged(e =>
            {
                // should mark object as selecting while dragging.
                lyricCaretState.MoveCaretToTargetPosition(Model);

                updateBackgroundColour();
            });
        }

        protected sealed override Drawable CreateContent()
        {
            return new Container
            {
                Masking = true,
                CornerRadius = 5,
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
                Children = new Drawable[]
                {
                    background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.9f
                    },
                    CreateRowContent()
                }
            };
        }

        protected LyricEditorMode EditorMode => bindableMode.Value;

        protected virtual void OnModeChanged()
        {
        }

        protected virtual void OnCaretPositionChanged(ICaretPosition caretPosition)
        {
        }

        protected abstract CompositeDrawable CreateRowContent();

        // todo: might be removed because will not have extend area after.
        public virtual float ExtendHeight => 0;

        protected abstract bool HighlightBackgroundWhenSelected(ICaretPosition caretPosition);

        protected abstract Func<LyricEditorMode, Color4> GetBackgroundColour(BackgroundStyle style, LyricEditorColourProvider colourProvider);

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state, ITimeTagModeState timeTagModeState)
        {
            bindableMode.BindTo(state.BindableMode);
            bindableHoverCaretPosition.BindTo(lyricCaretState.BindableHoverCaretPosition);
            bindableCaretPosition.BindTo(lyricCaretState.BindableCaretPosition);

            updateBackgroundColour();
        }

        private void updateBackgroundColour()
        {
            var mode = bindableMode.Value;
            var backgroundStyle = getBackgroundStyle();
            var colour = GetBackgroundColour(backgroundStyle, colourProvider).Invoke(mode);

            background.Colour = colour;

            BackgroundStyle getBackgroundStyle()
            {
                if (HighlightBackgroundWhenSelected(bindableCaretPosition.Value))
                    return BackgroundStyle.Hover;

                if (HighlightBackgroundWhenSelected(bindableHoverCaretPosition.Value))
                    return BackgroundStyle.Focus;

                return BackgroundStyle.Idle;
            }
        }

        protected enum BackgroundStyle
        {
            Idle,
            Hover,
            Focus
        }
    }
}
