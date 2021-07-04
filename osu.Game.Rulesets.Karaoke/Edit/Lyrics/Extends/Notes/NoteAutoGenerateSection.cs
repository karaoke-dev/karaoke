// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Notes;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    /// <summary>
    /// In <see cref="LyricEditorMode.CreateNote"/> mode, able to let user generate notes by <see cref="TimeTag"/>
    /// But need to make sure that lyric should not have any <see cref="TimeTagIssue"/>
    /// If found any issue, will navigate to target lyric.
    /// </summary>
    public class NoteAutoGenerateSection : AutoGenerateSection
    {
        [Resolved]
        private NoteManager noteManager { get; set; }

        [Resolved]
        private LyricCheckerManager lyricCheckerManager { get; set; }

        protected override Dictionary<Lyric, string> GetDisableSelectingLyrics(Lyric[] lyrics)
            => lyricCheckerManager.BindableReports.Where(x => x.Value.OfType<TimeTagIssue>().Any())
                                  .ToDictionary(k => k.Key, i => "Before generate time-tag, need to assign language first.");

        protected override void Apply(Lyric[] lyrics)
            => noteManager.AutoGenerateNotes(lyrics);
    }
}
