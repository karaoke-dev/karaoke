// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings.Previews.Gameplay;

public partial class ScoringSettingsPreview : SettingsSubsectionPreview
{
    private readonly BindableBool overridePitch = new();
    private readonly BindableInt pitch = new();
    private readonly BindableBool overrideVocalPitch = new();
    private readonly BindableInt vocalPitch = new();
    private readonly BindableBool overrideScoringPitch = new();
    private readonly BindableInt scoringPitch = new();

    public ScoringSettingsPreview()
    {
        PitchPreviewRow songRow;
        PitchPreviewRow vocalRow;
        PitchPreviewRow scoringRow;

        Size = new Vector2(0.85f, 0.58f);

        Child = new FillFlowContainer
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Padding = new MarginPadding(16),
            Spacing = new Vector2(0, 8),
            Direction = FillDirection.Vertical,
            Children = new Drawable[]
            {
                new OsuSpriteText
                {
                    Text = "Gameplay pitch offsets",
                    Font = OsuFont.Default.With(size: 24, weight: FontWeight.Bold),
                },
                new OsuSpriteText
                {
                    Text = "Each step represents one semitone.",
                    Font = OsuFont.Default.With(size: 14),
                },
                songRow = new PitchPreviewRow("Song"),
                vocalRow = new PitchPreviewRow("Vocal"),
                scoringRow = new PitchPreviewRow("Scoring"),
            },
        };

        bindRow(songRow, overridePitch, pitch);
        bindRow(vocalRow, overrideVocalPitch, vocalPitch);
        bindRow(scoringRow, overrideScoringPitch, scoringPitch);
    }

    [BackgroundDependencyLoader]
    private void load(KaraokeRulesetConfigManager config)
    {
        config.BindWith(KaraokeRulesetSetting.OverridePitchAtGameplay, overridePitch);
        config.BindWith(KaraokeRulesetSetting.Pitch, pitch);
        config.BindWith(KaraokeRulesetSetting.OverrideVocalPitchAtGameplay, overrideVocalPitch);
        config.BindWith(KaraokeRulesetSetting.VocalPitch, vocalPitch);
        config.BindWith(KaraokeRulesetSetting.OverrideScoringPitchAtGameplay, overrideScoringPitch);
        config.BindWith(KaraokeRulesetSetting.ScoringPitch, scoringPitch);
    }

    private static void bindRow(PitchPreviewRow row, BindableBool enabled, BindableInt value)
    {
        enabled.BindValueChanged(_ => update(), true);
        value.BindValueChanged(_ => update(), true);

        void update()
        {
            row.Value = value.Value;
            row.Enabled = enabled.Value;
        }
    }

    private partial class PitchPreviewRow : CompositeDrawable
    {
        private readonly Box background;
        private readonly PitchOffsetBar offsetBar;
        private readonly OsuSpriteText valueText;

        private int value;

        public int Value
        {
            get => value;
            set
            {
                this.value = value;
                offsetBar.Value = value;
                updateValueText();
            }
        }

        private bool enabled;

        public bool Enabled
        {
            get => enabled;
            set
            {
                enabled = value;
                this.FadeTo(value ? 1 : 0.45f, 150, Easing.OutQuint);
                updateValueText();
            }
        }

        public PitchPreviewRow(string label)
        {
            RelativeSizeAxes = Axes.X;
            Height = 44;
            Masking = true;
            CornerRadius = 6;

            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding(8),
                    ColumnDimensions = new[]
                    {
                        new Dimension(GridSizeMode.Absolute, 62),
                        new Dimension(GridSizeMode.Relative, 1),
                        new Dimension(GridSizeMode.Absolute, 38),
                    },
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            new OsuSpriteText
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Text = label,
                                Font = OsuFont.Default.With(size: 16, weight: FontWeight.SemiBold),
                            },
                            offsetBar = new PitchOffsetBar(),
                            valueText = new OsuSpriteText
                            {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                Font = OsuFont.Default.With(size: 15, weight: FontWeight.Bold),
                            },
                        },
                    },
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.Gray3;
            valueText.Colour = colours.Yellow;
        }

        private void updateValueText()
            => valueText.Text = Enabled ? Value.ToString("+0;-0;0") : "Off";
    }

    private partial class PitchOffsetBar : CompositeDrawable
    {
        private readonly Box track;
        private readonly Box centreMarker;
        private readonly Circle valueMarker;

        public int Value
        {
            set => valueMarker.MoveToX(0.5f + Math.Clamp(value, -10, 10) / 20f, 180, Easing.OutQuint);
        }

        public PitchOffsetBar()
        {
            RelativeSizeAxes = Axes.Both;
            Padding = new MarginPadding { Horizontal = 8 };

            InternalChildren = new Drawable[]
            {
                track = new Box
                {
                    RelativeSizeAxes = Axes.X,
                    Height = 3,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                centreMarker = new Box
                {
                    Width = 2,
                    Height = 13,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                valueMarker = new Circle
                {
                    RelativePositionAxes = Axes.X,
                    X = 0.5f,
                    Size = new Vector2(12),
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.Centre,
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            track.Colour = colours.Gray6;
            centreMarker.Colour = colours.GrayF;
            valueMarker.Colour = colours.Yellow;
        }
    }
}
