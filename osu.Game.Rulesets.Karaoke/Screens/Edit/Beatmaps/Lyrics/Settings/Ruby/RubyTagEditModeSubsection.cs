// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Ruby;

public partial class RubyTagEditModeSubsection : SwitchSubsection<RubyTagEditMode>
{
    protected override SwitchTabControl CreateTabControl()
        => new RubyTagTabControl();

    protected override DescriptionFormat GetDescription(RubyTagEditMode mode) =>
        mode switch
        {
            RubyTagEditMode.Create => "Use mouse to select range of the lyric text to create the ruby tag.",
            RubyTagEditMode.Modify => "Select ruby to change the start/end position or delete it.",
            _ => throw new InvalidOperationException(nameof(mode)),
        };

    private partial class RubyTagTabControl : SwitchTabControl
    {
        protected override SwitchTabItem CreateStepButton(OsuColour colours, RubyTagEditMode value)
        {
            return value switch
            {
                RubyTagEditMode.Create => new RubyTagTabButton(value)
                {
                    Text = "Create",
                    SelectedColour = colours.Green,
                    UnSelectedColour = colours.GreenDarker,
                },
                RubyTagEditMode.Modify => new RubyTagTabButton(value)
                {
                    Text = "Modify",
                    SelectedColour = colours.Pink,
                    UnSelectedColour = colours.PinkDarker,
                },
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }

        private partial class RubyTagTabButton : SwitchTabItem
        {
            private readonly Box background;
            private readonly OsuSpriteText text;

            public RubyTagTabButton(RubyTagEditMode value)
                : base(value)
            {
                Child = new Container
                {
                    Masking = true,
                    CornerRadius = 15,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        background = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                        },
                        text = new OsuSpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Font = OsuFont.GetFont(size: 16, weight: FontWeight.Bold),
                        },
                    },
                };
            }

            public LocalisableString Text
            {
                get => text.Text;
                set => text.Text = value;
            }

            public Color4 SelectedColour { get; init; }

            public Color4 UnSelectedColour { get; init; }

            protected override void UpdateState()
            {
                background.Colour = Active.Value ? SelectedColour : UnSelectedColour;
                Child.Alpha = Active.Value ? 0.8f : 0.4f;
            }
        }
    }
}
