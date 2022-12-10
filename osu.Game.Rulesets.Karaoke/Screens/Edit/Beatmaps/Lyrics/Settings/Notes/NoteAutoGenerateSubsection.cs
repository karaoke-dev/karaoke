// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Notes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Notes
{
    /// <summary>
    /// In <see cref="NoteEditMode.Generate"/> mode, able to let user generate notes by <see cref="TimeTag"/>
    /// But need to make sure that lyric should not have any <see cref="LyricTimeTagIssue"/>
    /// If found any issue, will navigate to target lyric.
    /// </summary>
    public partial class NoteAutoGenerateSubsection : AutoGenerateSubsection
    {
        private const string create_time_tag_mode = "CREATE_TIME_TAG_MODE";

        public NoteAutoGenerateSubsection()
            : base(LyricAutoGenerateProperty.AutoGenerateNotes)
        {
        }

        protected override DescriptionFormat CreateInvalidLyricDescriptionFormat()
            => new()
            {
                Text = $"Seems some lyric contains invalid time-tag, go to [{DescriptionFormat.LINK_KEY_ACTION}]({create_time_tag_mode}) to fix those issue.",
                Actions = new Dictionary<string, IDescriptionAction>
                {
                    {
                        create_time_tag_mode, new SwitchModeDescriptionAction
                        {
                            Text = "adjust time-tag mode",
                            Mode = LyricEditorMode.EditTimeTag
                        }
                    }
                }
            };

        protected override ConfigButton CreateConfigButton()
            => new NoteAutoGenerateConfigButton();

        protected partial class NoteAutoGenerateConfigButton : ConfigButton
        {
            public override Popover GetPopover()
                => new NoteGeneratorConfigPopover();
        }
    }
}
