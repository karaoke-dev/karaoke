// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks.Issues
{
    public class LyricRubyTagIssue : LyricIssue
    {
        public readonly RubyTag RubyTag;

        public LyricRubyTagIssue(Lyric lyric, IssueTemplate template, RubyTag rubyTag, params object[] args)
            : base(lyric, template, args)
        {
            RubyTag = rubyTag;
        }
    }
}
