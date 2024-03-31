// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Romanisation.Components;

public partial class LabelledRomanisedTextBox : LabelledObjectFieldTextBox<TimeTag>
{
    protected const float FIRST_SYLLABLE_BUTTON_SIZE = 20f;

    [Resolved]
    private ILyricTimeTagsChangeHandler timeTagsChangeHandler { get; set; } = null!;

    [Resolved]
    private IEditRomanisationModeState editRomanisationModeState { get; set; } = null!;

    private readonly IBindable<int> bindableRomanisationVersion = new Bindable<int>();

    public LabelledRomanisedTextBox(Lyric lyric, TimeTag timeTag)
        : base(timeTag)
    {
        Debug.Assert(lyric.TimeTags.Contains(timeTag));

        if (InternalChildren[1] is not FillFlowContainer fillFlowContainer)
            throw new ArgumentNullException(nameof(fillFlowContainer));

        // change padding to place first syllable button.
        fillFlowContainer.Padding = new MarginPadding
        {
            Horizontal = CONTENT_PADDING_HORIZONTAL,
            Vertical = CONTENT_PADDING_VERTICAL,
            Right = CONTENT_PADDING_HORIZONTAL + FIRST_SYLLABLE_BUTTON_SIZE + CONTENT_PADDING_HORIZONTAL,
        };

        // add first syllable button.
        AddInternal(new Container
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Padding = new MarginPadding
            {
                Top = CONTENT_PADDING_VERTICAL + 10,
                Right = CONTENT_PADDING_HORIZONTAL,
            },
            Child = new IconCheckbox(timeTag)
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                Size = new Vector2(FIRST_SYLLABLE_BUTTON_SIZE),
            },
        });

        bindableRomanisationVersion.BindTo(lyric.TimeTagsRomanisationVersion);
        bindableRomanisationVersion.BindValueChanged((_) =>
        {
            // change the label and the description.
            updateLabel(lyric, timeTag);
            updateDescription(lyric, timeTag);
        }, true);
    }

    private void updateLabel(Lyric lyric, TimeTag timeTag)
    {
        Label = !timeTag.FirstSyllable
            ? "  |  "
            : $"#{getRomanisationIndex(lyric, timeTag) + 1}";
        return;

        // get the index that mark as first syllable.
        static int getRomanisationIndex(Lyric lyric, TimeTag timeTag)
            => lyric.TimeTags.TakeWhile(x => x != timeTag).Count(x => x.FirstSyllable);
    }

    private void updateDescription(Lyric lyric, TimeTag timeTag)
    {
        // get the index and the calculated string.
        string displayIndex = TextIndexUtils.PositionFormattedString(timeTag.Index);
        string mainText = LyricUtils.GetTimeTagIndexDisplayText(lyric, timeTag.Index);
        Description = $"{displayIndex}, {mainText}";
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        SelectedItems.BindTo(editRomanisationModeState.SelectedItems);
    }

    protected sealed override string GetFieldValue(TimeTag item)
        => item.RomanisedSyllable ?? string.Empty;

    protected override void TriggerSelect(TimeTag item)
        => editRomanisationModeState.Select(item);

    protected override void ApplyValue(TimeTag item, string value)
        => timeTagsChangeHandler.SetTimeTagRomanisedSyllable(item, value);

    public partial class IconCheckbox : Checkbox, IHasAccentColour, IHasTooltip
    {
        private readonly SpriteIcon selectedIcon;

        [Resolved]
        private ILyricTimeTagsChangeHandler timeTagsChangeHandler { get; set; } = null!;

        private Sample? sampleChecked;
        private Sample? sampleUnchecked;

        public IconCheckbox(TimeTag timeTag)
        {
            Children = new Drawable[]
            {
                selectedIcon = new SpriteIcon
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Icon = FontAwesome.Solid.Tag,
                    Scale = new Vector2(0),
                },
                new HoverSounds(),
            };

            Current.Value = timeTag.FirstSyllable;

            Current.ValueChanged += e =>
            {
                updateSelected(e.NewValue);
                timeTagsChangeHandler.SetTimeTagFirstSyllable(timeTag, e.NewValue);
            };

            updateSelected(Current.Value);
        }

        private void updateSelected(bool selected)
        {
            selectedIcon.ScaleTo(selected ? 1f : 0.8f, 200, Easing.OutElastic);
            selectedIcon.FadeTo(selected ? 1f : 0.2f, 200, Easing.OutElastic);
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            sampleChecked = audio.Samples.Get("UI/check-on");
            sampleUnchecked = audio.Samples.Get("UI/check-off");
        }

        private Color4 accentColour;

        public Color4 AccentColour
        {
            get => accentColour;
            set
            {
                accentColour = value;
                selectedIcon.Colour = AccentColour;
            }
        }

        protected override void OnUserChange(bool value)
        {
            base.OnUserChange(value);

            if (value)
                sampleChecked?.Play();
            else
                sampleUnchecked?.Play();
        }

        public LocalisableString TooltipText => "Mark as the first romanised syllable";
    }
}
