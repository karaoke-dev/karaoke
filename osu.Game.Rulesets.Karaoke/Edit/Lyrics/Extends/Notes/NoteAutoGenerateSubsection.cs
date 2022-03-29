// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    /// <summary>
    /// In <see cref="NoteEditMode.Generate"/> mode, able to let user generate notes by <see cref="TimeTag"/>
    /// But need to make sure that lyric should not have any <see cref="TimeTagIssue"/>
    /// If found any issue, will navigate to target lyric.
    /// </summary>
    public class NoteAutoGenerateSubsection : AutoGenerateSubsection
    {
        [Resolved]
        private INotesChangeHandler notesChangeHandler { get; set; }

        [Resolved]
        private LyricCheckerManager lyricCheckerManager { get; set; }

        protected override Dictionary<Lyric, string> GetDisableSelectingLyrics(IEnumerable<Lyric> lyrics)
            => lyricCheckerManager.BindableReports.Where(x => x.Value.OfType<TimeTagIssue>().Any())
                                  .ToDictionary(k => k.Key, _ => "Before generate time-tag, need to assign language first.");

        protected override void Apply()
            => notesChangeHandler.AutoGenerate();

        protected override InvalidLyricAlertTextContainer CreateInvalidLyricAlertTextContainer()
            => new InvalidLyricTimeTagAlertTextContainer();

        protected override ConfigButton CreateConfigButton()
            => new NoteAutoGenerateConfigButton();

        protected class InvalidLyricTimeTagAlertTextContainer : InvalidLyricAlertTextContainer
        {
            private const string create_time_tag_mode = "CREATE_TIME_TAG_MODE";

            public InvalidLyricTimeTagAlertTextContainer()
            {
                SwitchToEditorMode(create_time_tag_mode, "adjust time-tag mode", LyricEditorMode.CreateTimeTag);
                Text = $"Seems some lyric contains invalid time-tag, go to [{create_time_tag_mode}] to fix those issue.";
            }
        }

        protected class NoteAutoGenerateConfigButton : ConfigButton
        {
            public override Popover GetPopover()
                => new NoteGeneratorConfigPopover();
        }
    }
}
