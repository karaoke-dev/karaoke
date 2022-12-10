// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.RubyRomaji
{
    public partial class RomajiTagIssueSection : TextTagIssueSection
    {
        protected override LyricEditorMode EditMode => LyricEditorMode.EditRomaji;

        protected override LyricsIssueTable CreateLyricsIssueTable() => new RomajiTagIssueTable();

        private partial class RomajiTagIssueTable : TextTagIssueTable<RomajiTag>
        {
            protected override Tuple<Lyric, RomajiTag> GetInvalidByIssue(Issue issue)
            {
                if (issue is not LyricRomajiTagIssue romajiTagIssue)
                    throw new InvalidCastException();

                return new Tuple<Lyric, RomajiTag>(romajiTagIssue.Lyric, romajiTagIssue.RomajiTag);
            }
        }
    }
}
