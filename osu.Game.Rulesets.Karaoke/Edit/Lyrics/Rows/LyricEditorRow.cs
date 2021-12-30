// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows
{
    public abstract class LyricEditorRow : CompositeDrawable
    {
        public const int SELECT_AREA_WIDTH = 48;

        private const int info_part_spacing = 210;
        private const int min_height = 75;
        private const int max_height = 120;

        private readonly Lyric lyric;

        protected LyricEditorRow(Lyric lyric)
        {
            this.lyric = lyric;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChild = new GridContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                ColumnDimensions = new[]
                {
                    new Dimension(GridSizeMode.AutoSize),
                    new Dimension(GridSizeMode.Absolute, info_part_spacing),
                    new Dimension()
                },
                RowDimensions = new[] { new Dimension(GridSizeMode.AutoSize, minSize: min_height, maxSize: max_height) },
                Content = new[]
                {
                    new[]
                    {
                        new SelectArea(lyric),
                        CreateLyricInfo(lyric),
                        CreateContent(lyric)
                    }
                }
            };
        }

        protected abstract Drawable CreateLyricInfo(Lyric lyric);

        protected abstract Drawable CreateContent(Lyric lyric);

        public class SelectArea : CompositeDrawable
        {
            private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
            private readonly IBindable<bool> selecting = new Bindable<bool>();
            private readonly IBindableDictionary<Lyric, string> disableSelectingLyrics = new BindableDictionary<Lyric, string>();
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
                bindableMode.BindValueChanged(_ =>
                {
                    background.Colour = colourProvider.Dark2(state.Mode);
                    selectedCheckbox.AccentColour = colourProvider.Colour2(state.Mode);
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
                    selectedCheckbox.TooltipText = disabled ? disableSelectingLyrics[lyric] : null;
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
