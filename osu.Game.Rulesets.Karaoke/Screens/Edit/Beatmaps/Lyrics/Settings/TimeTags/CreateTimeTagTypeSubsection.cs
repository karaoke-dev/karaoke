// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.TimeTags;

public partial class CreateTimeTagTypeSubsection : SwitchSubsection<CreateTimeTagType>
{
    protected override SwitchTabControl CreateTabControl()
        => new CreateTimeTagTypeTabControl();

    protected override DescriptionFormat GetDescription(CreateTimeTagType mode) =>
        mode switch
        {
            CreateTimeTagType.Mouse => "Use mouse to move the caret, and click the button in the UI to create/remove the start/end time tag. It's for the beginner.",
            CreateTimeTagType.HotkeyThenPress => "Press the hotkey to prepare create/remove the start/end time tag. This kind of create mode is still implementing.",
            CreateTimeTagType.Keyboard => new DescriptionFormat
            {
                Text =
                    $"Use [{DescriptionFormat.LINK_KEY_ACTION}](navigate_time_tag) to control caret position, press [{DescriptionFormat.LINK_KEY_ACTION}](create_time_tag) to create new time-tag and press [{DescriptionFormat.LINK_KEY_ACTION}](remove_time_tag) to delete exist time-tag.",
                Actions = new Dictionary<string, IDescriptionAction>
                {
                    {
                        "navigate_time_tag", new InputKeyDescriptionAction
                        {
                            Text = "Keyboard",
                            AdjustableActions = new List<KaraokeEditAction>
                            {
                                KaraokeEditAction.MoveToPreviousLyric,
                                KaraokeEditAction.MoveToNextLyric,
                                KaraokeEditAction.MoveToPreviousIndex,
                                KaraokeEditAction.MoveToNextIndex,
                            },
                        }
                    },
                    {
                        "create_time_tag", new InputKeyDescriptionAction
                        {
                            Text = "Create Time-tag keys",
                            AdjustableActions = new List<KaraokeEditAction>
                            {
                                KaraokeEditAction.CreateStartTimeTag,
                                KaraokeEditAction.CreateEndTimeTag,
                            },
                        }
                    },
                    {
                        "remove_time_tag", new InputKeyDescriptionAction
                        {
                            Text = "Remove Time-tag keys",
                            AdjustableActions = new List<KaraokeEditAction>
                            {
                                KaraokeEditAction.RemoveStartTimeTag,
                                KaraokeEditAction.RemoveEndTimeTag,
                            },
                        }
                    },
                },
            },
            _ => throw new InvalidOperationException(nameof(mode)),
        };

    private partial class CreateTimeTagTypeTabControl : SwitchTabControl
    {
        protected override SwitchTabItem CreateStepButton(OsuColour colours, CreateTimeTagType value)
        {
            return value switch
            {
                CreateTimeTagType.Mouse => new CreateTimeTagTypeTabButton(value)
                {
                    Icon = FontAwesome.Solid.MousePointer,
                    TooltipText = "Mouse",
                    SelectedColour = colours.Green,
                    UnSelectedColour = colours.GreenDarker,
                },
                CreateTimeTagType.HotkeyThenPress => new CreateTimeTagTypeTabButton(value)
                {
                    Icon = FontAwesome.Solid.PencilAlt,
                    TooltipText = "Keyboard + Mouse",
                    SelectedColour = colours.Yellow,
                    UnSelectedColour = colours.YellowDarker,
                },
                CreateTimeTagType.Keyboard => new CreateTimeTagTypeTabButton(value)
                {
                    Icon = FontAwesome.Solid.Keyboard,
                    TooltipText = "Keyboard",
                    SelectedColour = colours.Blue,
                    UnSelectedColour = colours.BlueDarker,
                },
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }

        private partial class CreateTimeTagTypeTabButton : SwitchTabItem, IHasTooltip
        {
            private readonly Box background;
            private readonly SpriteIcon spriteIcon;

            public CreateTimeTagTypeTabButton(CreateTimeTagType value)
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
                        spriteIcon = new SpriteIcon
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(25),
                        },
                    },
                };
            }

            public IconUsage Icon
            {
                get => spriteIcon.Icon;
                set => spriteIcon.Icon = value;
            }

            public LocalisableString TooltipText { get; init; }

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
