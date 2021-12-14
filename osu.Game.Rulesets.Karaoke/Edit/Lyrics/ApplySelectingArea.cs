// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
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

        private Bindable<bool> selecting;
        private BindableList<Lyric> selectedLyrics;

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

            selecting = lyricSelectionState.Selecting.GetBoundCopy();
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

            // get bindable and update bindable if select or not select all.
            selectedLyrics = lyricSelectionState.SelectedLyrics.GetBoundCopy();

            selectedLyrics.BindCollectionChanged((_, _) =>
            {
                bool selectAny = selectedLyrics.Any();
                applyButton.Enabled.Value = selectAny;
            }, true);
        }

        public class SelectArea : CompositeDrawable
        {
            private Bindable<LyricEditorMode> mode;
            private BindableDictionary<Lyric, string> disableSelectingLyrics;
            private BindableList<Lyric> selectedLyrics;

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
                mode = state.BindableMode.GetBoundCopy();
                disableSelectingLyrics = lyricSelectionState.DisableSelectingLyric.GetBoundCopy();
                selectedLyrics = lyricSelectionState.SelectedLyrics.GetBoundCopy();

                // should update background if mode changed.
                mode.BindValueChanged(_ =>
                {
                    background.Colour = colourProvider.Dark2(state.Mode);
                    allSelectedCheckbox.AccentColour = colourProvider.Colour2(state.Mode);
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

                    selectedLyrics.Clear();

                    if (e.NewValue)
                    {
                        var disableSelectingLyrics = lyricSelectionState.DisableSelectingLyric.Keys;
                        var lyrics = beatmap.HitObjects.OfType<Lyric>().Where(x => !disableSelectingLyrics.Contains(x));
                        selectedLyrics.AddRange(lyrics);
                    }

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
