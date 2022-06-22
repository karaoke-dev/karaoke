// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class ApplySelectingArea : CompositeDrawable
    {
        private const float spacing = 10;
        private const float button_width = 100;

        private readonly IBindable<bool> selecting = new Bindable<bool>();
        private readonly IBindableList<Lyric> selectedLyrics = new BindableList<Lyric>();

        private ActionButton applyButton;

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, ILyricSelectionState lyricSelectionState)
        {
            RelativeSizeAxes = Axes.X;
            Height = 45;

            Masking = true;
            CornerRadius = 5;

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Colour = colours.Gray2,
                    RelativeSizeAxes = Axes.Both,
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    ColumnDimensions = new[]
                    {
                        new Dimension(GridSizeMode.Absolute, LyricEditorRow.SELECT_AREA_WIDTH),
                        new Dimension(),
                        new Dimension(GridSizeMode.Absolute, spacing),
                        new Dimension(GridSizeMode.Absolute, button_width),
                        new Dimension(GridSizeMode.Absolute, spacing),
                        new Dimension(GridSizeMode.Absolute, button_width),
                        new Dimension(GridSizeMode.Absolute, spacing),
                        new Dimension(GridSizeMode.Absolute, button_width),
                        new Dimension(GridSizeMode.Absolute, spacing),
                    },
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            new SelectArea
                            {
                                RelativeSizeAxes = Axes.Both,
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                            },
                            new Box(),
                            applyButton = new ActionButton
                            {
                                Text = "Apply",
                                BackgroundColour = colours.Red,
                                Action = () =>
                                {
                                    lyricSelectionState.EndSelecting(LyricEditorSelectingAction.Apply);
                                }
                            },
                            new Box(),
                            new ActionButton
                            {
                                Text = "Cancel",
                                Action = () =>
                                {
                                    lyricSelectionState.EndSelecting(LyricEditorSelectingAction.Cancel);
                                }
                            },
                            new Box(),
                            new ActionButton
                            {
                                Text = "Preview",
                                BackgroundColour = colours.Purple,
                                Action = () =>
                                {
                                    // todo : implement
                                }
                            },
                            new Box(),
                        }
                    }
                }
            };

            selecting.BindTo(lyricSelectionState.Selecting);
            selectedLyrics.BindTo(lyricSelectionState.SelectedLyrics);

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

            selectedLyrics.BindCollectionChanged((_, _) =>
            {
                bool selectAny = selectedLyrics.Any();
                applyButton.Enabled.Value = selectAny;
            }, true);
        }

        public class SelectArea : CompositeDrawable
        {
            private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
            private readonly IBindableDictionary<Lyric, LocalisableString> disableSelectingLyrics = new BindableDictionary<Lyric, LocalisableString>();
            private readonly IBindableList<Lyric> selectedLyrics = new BindableList<Lyric>();

            private readonly Box background;
            private readonly CircleCheckbox allSelectedCheckbox;

            public SelectArea()
            {
                InternalChildren = new Drawable[]
                {
                    background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    allSelectedCheckbox = new CircleCheckbox
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    }
                };
            }

            protected override bool OnClick(ClickEvent e)
            {
                // trigger checkbox click if click this area.
                allSelectedCheckbox.TriggerEvent(e);
                return base.OnClick(e);
            }

            private bool selectedLyricsTriggering;
            private bool checkboxClicking;

            [BackgroundDependencyLoader]
            private void load(ILyricEditorState state, ILyricSelectionState lyricSelectionState, LyricEditorColourProvider colourProvider, EditorBeatmap beatmap)
            {
                bindableMode.BindTo(state.BindableMode);
                disableSelectingLyrics.BindTo(lyricSelectionState.DisableSelectingLyric);
                selectedLyrics.BindTo(lyricSelectionState.SelectedLyrics);

                // should update background if mode changed.
                bindableMode.BindValueChanged(e =>
                {
                    background.Colour = colourProvider.Dark2(e.NewValue);
                    allSelectedCheckbox.AccentColour = colourProvider.Colour2(e.NewValue);
                }, true);

                // should disable selection if current lyric is disabled.
                disableSelectingLyrics.BindCollectionChanged((_, _) =>
                {
                    int disabledLyricNumber = lyricSelectionState.DisableSelectingLyric.Count;
                    int totalLyrics = beatmap.HitObjects.OfType<Lyric>().Count();
                    bool disabled = disabledLyricNumber == totalLyrics;

                    allSelectedCheckbox.Current.Disabled = disabled;
                    allSelectedCheckbox.TooltipText = disabled ? "Seems all selection are disabled" : null;
                });

                // get bindable and update bindable if select or not select all.
                selectedLyrics.BindCollectionChanged((_, _) =>
                {
                    if (checkboxClicking)
                        return;

                    selectedLyricsTriggering = true;

                    var lyrics = beatmap.HitObjects.OfType<Lyric>();

                    bool selectAll = lyrics.All(x => selectedLyrics.Contains(x));
                    allSelectedCheckbox.Current.Value = selectAll;

                    selectedLyricsTriggering = false;
                }, true);

                allSelectedCheckbox.Current.BindValueChanged(e =>
                {
                    if (selectedLyricsTriggering)
                        return;

                    checkboxClicking = true;

                    if (e.NewValue)
                        lyricSelectionState.SelectAll();
                    else
                        lyricSelectionState.UnSelectAll();

                    checkboxClicking = false;
                });
            }
        }

        public class ActionButton : OsuButton
        {
            public ActionButton()
            {
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;
                RelativeSizeAxes = Axes.X;
                Height = 45 - spacing * 2;
            }
        }
    }
}
