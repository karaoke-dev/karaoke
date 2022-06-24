// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks.Components
{
    public class RubyTagIssue : Issue
    {
        public readonly Dictionary<RubyTagInvalid, RubyTag[]> InvalidRubyTags;

        public RubyTagIssue(HitObject lyric, IssueTemplate template, Dictionary<RubyTagInvalid, RubyTag[]> invalidRubyTags, params object[] args)
            : base(lyric, template, args)
        {
            InvalidRubyTags = invalidRubyTags;
        }
    }

    public enum RubyTagInvalid
    {
        OutOfRange,

        Overlapping,

        EmptyText,
    }
}
