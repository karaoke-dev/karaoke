// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.TimeTags;

public partial class TimeTagCreateConfigSubsection : EditModeSwitchSubsection<CreateTimeTagEditMode>
{
    protected override LocalisableString GetButtonTitle(CreateTimeTagEditMode mode)
        => mode switch
        {
            CreateTimeTagEditMode.Create => "Create",
            CreateTimeTagEditMode.Modify => "Modify",
            _ => throw new InvalidOperationException(nameof(mode)),
        };

    protected override Color4 GetButtonColour(OsuColour colours, CreateTimeTagEditMode mode, bool active)
        => mode switch
        {
            CreateTimeTagEditMode.Create => active ? colours.Green : colours.GreenDarker,
            CreateTimeTagEditMode.Modify => active ? colours.Pink : colours.PinkDarker,
            _ => throw new InvalidOperationException(nameof(mode)),
        };

    protected override DescriptionFormat GetDescription(CreateTimeTagEditMode mode) =>
        mode switch
        {
            CreateTimeTagEditMode.Create => new DescriptionFormat
            {
                Text =
                    $"Use keyboard to control caret position, press [{DescriptionFormat.LINK_KEY_ACTION}](create_time_tag) to create new time-tag and press [{DescriptionFormat.LINK_KEY_ACTION}](remove_time_tag) to delete exist time-tag.",
                Actions = new Dictionary<string, IDescriptionAction>
                {
                    {
                        "create_time_tag", new InputKeyDescriptionAction
                        {
                            Text = "Create Time-tag keys.",
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
                            Text = "Remove Time-tag keys.",
                            AdjustableActions = new List<KaraokeEditAction>
                            {
                                KaraokeEditAction.RemoveStartTimeTag,
                                KaraokeEditAction.RemoveEndTimeTag,
                            },
                        }
                    },
                },
            },
            CreateTimeTagEditMode.Modify => new DescriptionFormat
            {
                Text =
                    $"Press [{DescriptionFormat.LINK_KEY_ACTION}](move_time_tag_position) to move the time-tag position. Press press [{DescriptionFormat.LINK_KEY_ACTION}](create_time_tag) to create new time-tag and [{DescriptionFormat.LINK_KEY_ACTION}](remove_time_tag) to delete exist time-tag.",
                Actions = new Dictionary<string, IDescriptionAction>
                {
                    {
                        "move_time_tag_position", new InputKeyDescriptionAction
                        {
                            Text = "Shifting Time-tag index keys.",
                            AdjustableActions = new List<KaraokeEditAction>
                            {
                                KaraokeEditAction.ShiftTheTimeTagLeft,
                                KaraokeEditAction.ShiftTheTimeTagRight,
                                KaraokeEditAction.ShiftTheTimeTagStateLeft,
                                KaraokeEditAction.ShiftTheTimeTagStateRight,
                            },
                        }
                    },
                    {
                        "create_time_tag",
                        new InputKeyDescriptionAction { AdjustableActions = new List<KaraokeEditAction> { KaraokeEditAction.CreateStartTimeTag, KaraokeEditAction.CreateEndTimeTag } }
                    },
                    {
                        "remove_time_tag",
                        new InputKeyDescriptionAction { AdjustableActions = new List<KaraokeEditAction> { KaraokeEditAction.RemoveStartTimeTag, KaraokeEditAction.RemoveEndTimeTag } }
                    },
                },
            },
            _ => throw new InvalidOperationException(nameof(mode)),
        };
}
