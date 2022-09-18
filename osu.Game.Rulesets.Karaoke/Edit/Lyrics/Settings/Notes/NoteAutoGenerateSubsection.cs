// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Notes
{
    /// <summary>
    /// In <see cref="NoteEditMode.Generate"/> mode, able to let user generate notes by <see cref="TimeTag"/>
    /// But need to make sure that lyric should not have any <see cref="TimeTagIssue"/>
    /// If found any issue, will navigate to target lyric.
    /// </summary>
    public class NoteAutoGenerateSubsection : AutoGenerateSubsection
    {
        private const string create_time_tag_mode = "CREATE_TIME_TAG_MODE";

        public NoteAutoGenerateSubsection()
            : base(LyricAutoGenerateProperty.AutoGenerateNotes)
        {
        }

        protected override DescriptionFormat CreateInvalidLyricDescriptionFormat()
            => new()
            {
                Text = $"Seems some lyric contains invalid time-tag, go to [{DescriptionFormat.LINK_KEY_EDIT_MODE}]({create_time_tag_mode}) to fix those issue.",
                EditModes = new Dictionary<string, SwitchMode>
                {
                    {
                        create_time_tag_mode, new SwitchMode
                        {
                            Text = "adjust time-tag mode",
                            Mode = LyricEditorMode.EditTimeTag
                        }
                    }
                }
            };

        protected override ConfigButton CreateConfigButton()
            => new NoteAutoGenerateConfigButton();

        protected class NoteAutoGenerateConfigButton : ConfigButton
        {
            public override Popover GetPopover()
                => new NoteGeneratorConfigPopover();
        }
    }
}
