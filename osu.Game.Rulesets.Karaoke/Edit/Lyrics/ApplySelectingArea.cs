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
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class ApplySelectingArea : CompositeDrawable
    {
        private const float spacing = 10;
        private const float button_width = 100;

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, ILyricEditorState state)
        {
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
                            new ActionButton
                            {
                                Text = "Apply",
                                BackgroundColour = colours.Red,
                                Action = () =>
                                {
                                    state.EndSelecting(LyricEditorSelectingAction.Apply);
                                }
                            },
                            new Box(),
                            new ActionButton
                            {
                                Text = "Cancel",
                                Action = () =>
                                {
                                    state.EndSelecting(LyricEditorSelectingAction.Cancel);
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
        }

        public class SelectArea : CompositeDrawable
        {
            private Bindable<LyricEditorMode> mode;
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
            private void load(ILyricEditorState state, LyricEditorColourProvider colourProvider, EditorBeatmap beatmap)
            {
                mode = state.BindableMode.GetBoundCopy();
                selectedLyrics = state.SelectedLyrics.GetBoundCopy();

                // should update background if mode changed.
                mode.BindValueChanged(e =>
                {
                    background.Colour = colourProvider.Dark2(state.Mode);
                    allSelectedCheckbox.AccentColour = colourProvider.Colour2(state.Mode);
                }, true);

                // get bindable and update bindable if select or not select all.
                selectedLyrics.BindCollectionChanged((a, b) =>
                {
                    if (checkboxClicking)
                        return;

                    selectedLyricsTriggering = true;

                    var lyrics = beatmap.HitObjects.OfType<Lyric>();

                    var selectAll = lyrics.All(x => selectedLyrics.Contains(x));
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
                        var lyrics = beatmap.HitObjects.OfType<Lyric>();
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
