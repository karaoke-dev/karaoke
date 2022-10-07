// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows
{
    public abstract class Row : CompositeDrawable
    {
        public const int SELECT_AREA_WIDTH = 48;

        [Resolved, AllowNull]
        private LyricEditorColourProvider colourProvider { get; set; }

        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
        private readonly IBindable<ICaretPosition?> bindableHoverCaretPosition = new Bindable<ICaretPosition?>();
        private readonly IBindable<ICaretPosition?> bindableCaretPosition = new Bindable<ICaretPosition?>();

        protected readonly Lyric Lyric;

        private readonly Box background;

        protected Row(Lyric lyric)
        {
            Lyric = lyric;

            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            var columnDimensions = new List<Dimension>
            {
                new(GridSizeMode.AutoSize),
            };
            columnDimensions.AddRange(GetColumnDimensions());

            var rowDimensions = GetRowDimensions();

            var columns = new List<Drawable>
            {
                new SelectArea(lyric),
            };
            columns.AddRange(GetDrawables(lyric));

            InternalChild = new Container
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
                    new GridContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        ColumnDimensions = columnDimensions.ToArray(),
                        RowDimensions = new[] { rowDimensions },
                        Content = new[]
                        {
                            columns.ToArray()
                        }
                    }
                }
            };

            bindableMode.BindValueChanged(_ =>
            {
                updateBackgroundColour();
            });

            bindableHoverCaretPosition.BindValueChanged(e =>
            {
                if (ValueChangedEventUtils.LyricChanged(e))
                    updateBackgroundColour();
            });

            bindableCaretPosition.BindValueChanged(e =>
            {
                if (ValueChangedEventUtils.LyricChanged(e))
                    updateBackgroundColour();
            });
        }

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state, ILyricCaretState lyricCaretState)
        {
            bindableMode.BindTo(state.BindableMode);

            bindableHoverCaretPosition.BindTo(lyricCaretState.BindableHoverCaretPosition);
            bindableCaretPosition.BindTo(lyricCaretState.BindableCaretPosition);

            updateBackgroundColour();
        }

        protected abstract IEnumerable<Dimension> GetColumnDimensions();

        protected abstract Dimension GetRowDimensions();

        protected abstract IEnumerable<Drawable> GetDrawables(Lyric lyric);

        protected abstract bool HighlightBackgroundWhenSelected(ICaretPosition? caretPosition);

        protected abstract Func<LyricEditorMode, Color4> GetBackgroundColour(BackgroundStyle style, LyricEditorColourProvider colourProvider);

        private void updateBackgroundColour()
        {
            var mode = bindableMode.Value;
            var backgroundStyle = getBackgroundStyle();
            var colour = GetBackgroundColour(backgroundStyle, colourProvider).Invoke(mode);

            background.Colour = colour;

            BackgroundStyle getBackgroundStyle()
            {
                if (highlightBackground(bindableCaretPosition.Value))
                    return BackgroundStyle.Focus;

                if (highlightBackground(bindableHoverCaretPosition.Value))
                    return BackgroundStyle.Hover;

                return BackgroundStyle.Idle;
            }

            bool highlightBackground(ICaretPosition? caretPosition)
            {
                if (caretPosition?.Lyric != Lyric)
                    return false;

                return HighlightBackgroundWhenSelected(caretPosition);
            }
        }

        protected enum BackgroundStyle
        {
            Idle,
            Hover,
            Focus
        }

        public class SelectArea : CompositeDrawable
        {
            private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
            private readonly IBindable<bool> selecting = new Bindable<bool>();
            private readonly IBindableDictionary<Lyric, LocalisableString> disableSelectingLyrics = new BindableDictionary<Lyric, LocalisableString>();
            private readonly IBindableList<Lyric> selectedLyrics = new BindableList<Lyric>();

            private readonly Box background;
            private readonly CircleCheckbox selectedCheckbox;

            private readonly Lyric lyric;

            public SelectArea(Lyric lyric)
            {
                this.lyric = lyric;

                Width = SELECT_AREA_WIDTH;
                RelativeSizeAxes = Axes.Y;
                InternalChildren = new Drawable[]
                {
                    background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    selectedCheckbox = new CircleCheckbox
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    }
                };
            }

            protected override bool OnClick(ClickEvent e)
            {
                // trigger checkbox click if click this area.
                selectedCheckbox.TriggerEvent(e);
                return base.OnClick(e);
            }

            [BackgroundDependencyLoader]
            private void load(ILyricEditorState state, ILyricSelectionState lyricSelectionState, LyricEditorColourProvider colourProvider)
            {
                bindableMode.BindTo(state.BindableMode);
                selecting.BindTo(lyricSelectionState.Selecting);
                disableSelectingLyrics.BindTo(lyricSelectionState.DisableSelectingLyric);
                selectedLyrics.BindTo(lyricSelectionState.SelectedLyrics);

                // should update background if mode changed.
                bindableMode.BindValueChanged(e =>
                {
                    background.Colour = colourProvider.Dark2(e.NewValue);
                    selectedCheckbox.AccentColour = colourProvider.Colour2(e.NewValue);
                }, true);

                // show this area only if in selecting.
                selecting.BindValueChanged(e =>
                {
                    if (e.NewValue)
                    {
                        Show();
                    }
                    else
                    {
                        Hide();
                    }
                }, true);

                // get bindable and update bindable if check / uncheck.
                selectedLyrics.BindCollectionChanged((_, _) =>
                {
                    if (selectedCheckbox.Current.Disabled)
                        return;

                    bool selected = selectedLyrics.Contains(lyric);
                    selectedCheckbox.Current.Value = selected;
                }, true);

                // should disable selection if current lyric is disabled.
                disableSelectingLyrics.BindCollectionChanged((_, _) =>
                {
                    bool disabled = disableSelectingLyrics.Keys.Contains(lyric);
                    selectedCheckbox.Current.Disabled = disabled;
                    selectedCheckbox.TooltipText = disabled ? disableSelectingLyrics[lyric] : default;
                });

                selectedCheckbox.Current.BindValueChanged(e =>
                {
                    if (e.NewValue)
                    {
                        lyricSelectionState.Select(lyric);
                    }
                    else
                    {
                        lyricSelectionState.UnSelect(lyric);
                    }
                });
            }
        }
    }
}
