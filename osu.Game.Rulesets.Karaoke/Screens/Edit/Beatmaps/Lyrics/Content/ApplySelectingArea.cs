// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

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
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content;

public partial class ApplySelectingArea : CompositeDrawable
{
    private const float spacing = 10;
    private const float button_width = 100;

    private readonly IBindableList<Lyric> selectedLyrics = new BindableList<Lyric>();

    private readonly Box background;
    private readonly GridContainer gridContainer;
    private readonly ActionButton applyButton;
    private readonly ActionButton cancelButton;
    private readonly ActionButton previewButton;

    public ApplySelectingArea()
    {
        RelativeSizeAxes = Axes.X;
        Height = 45;

        InternalChildren = new Drawable[]
        {
            background = new Box
            {
                RelativeSizeAxes = Axes.Both,
            },
            gridContainer = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                ColumnDimensions = new[]
                {
                    new Dimension(GridSizeMode.Absolute, LyricList.LYRIC_LIST_PADDING),
                    new Dimension(GridSizeMode.Absolute, Row.SELECT_AREA_WIDTH),
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
                    new[]
                    {
                        Empty(),
                        new SelectAllArea
                        {
                            RelativeSizeAxes = Axes.Both,
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                        },
                        Empty(),
                        applyButton = new ActionButton
                        {
                            Text = "Apply",
                        },
                        Empty(),
                        cancelButton = new ActionButton
                        {
                            Text = "Cancel",
                        },
                        Empty(),
                        previewButton = new ActionButton
                        {
                            Text = "Preview",
                        },
                        Empty(),
                    },
                },
            },
        };

        selectedLyrics.BindCollectionChanged((_, _) =>
        {
            bool selectAny = selectedLyrics.Any();
            applyButton.Enabled.Value = selectAny;
        }, true);
    }

    [BackgroundDependencyLoader]
    private void load(OsuColour colours, ILyricSelectionState lyricSelectionState)
    {
        background.Colour = colours.Gray2;

        selectedLyrics.BindTo(lyricSelectionState.SelectedLyrics);

        applyButton.BackgroundColour = colours.Red;
        applyButton.Action = () =>
        {
            lyricSelectionState.EndSelecting(LyricEditorSelectingAction.Apply);
        };

        cancelButton.BackgroundColour = colours.Gray2;
        cancelButton.Action = () =>
        {
            lyricSelectionState.EndSelecting(LyricEditorSelectingAction.Cancel);
        };

        previewButton.BackgroundColour = colours.Purple;
        previewButton.Action = () =>
        {
            // todo : implement
        };
    }

    public partial class SelectAllArea : CompositeDrawable
    {
        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
        private readonly IBindableDictionary<Lyric, LocalisableString> disableSelectingLyrics = new BindableDictionary<Lyric, LocalisableString>();
        private readonly IBindableList<Lyric> selectedLyrics = new BindableList<Lyric>();

        private readonly Box background;
        private readonly CircleCheckbox allSelectedCheckbox;

        public SelectAllArea()
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
                },
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
                allSelectedCheckbox.TooltipText = disabled ? "Seems all selection are disabled" : string.Empty;
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

    public partial class ActionButton : OsuButton
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
