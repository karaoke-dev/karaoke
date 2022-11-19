// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.RubyRomaji
{
    public class RubyTagIssueSection : TextTagIssueSection
    {
        protected override LyricEditorMode EditMode => LyricEditorMode.EditRuby;

        protected override IssueTable CreateIssueTable() => new RubyTagIssueTable();

        private class RubyTagIssueTable : TextTagIssueTable<RubyTag>
        {
            protected override Tuple<Lyric, RubyTag> GetInvalidByIssue(Issue issue)
            {
                if (issue is not LyricRubyTagIssue rubyTagIssue)
                    throw new InvalidCastException();

                return new Tuple<Lyric, RubyTag>(rubyTagIssue.Lyric, rubyTagIssue.RubyTag);
            }
        }
    }
}
