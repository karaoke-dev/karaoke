// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RubyTagIssueSection : Section
    {
        protected override string Title => "Invalid ruby-tag";

        private BindableDictionary<Lyric, Issue[]> bindableReports;

        private RubyTagIssueTable table;

        [BackgroundDependencyLoader]
        private void load(LyricCheckerManager lyricCheckerManager)
        {
            Children = new[]
            {
                table = new RubyTagIssueTable(),
            };

            bindableReports = lyricCheckerManager.BindableReports.GetBoundCopy();
            bindableReports.BindCollectionChanged((a, b) =>
            {
                var issues = bindableReports.Values.SelectMany(x => x);
                table.Issues = issues.OfType<RubyTagIssue>();
            }, true);
        }

        public class RubyTagIssueTable : TextTagIssueTable<RubyTagInvalid, RubyTag>
        {
            protected override IEnumerable<Tuple<RubyTag, RubyTagInvalid>> GetInvalidByIssue(Issue issue)
            {
                if (!(issue is RubyTagIssue rubyTagIssue))
                    yield break;

                foreach (var (invalidReason, rubyTags) in rubyTagIssue.InvalidRubyTags)
                {
                    foreach (var rubyTag in rubyTags)
                    {
                        yield return new Tuple<RubyTag, RubyTagInvalid>(rubyTag, invalidReason);
                    }
                }
            }

            protected override string GetInvalidMessage(RubyTagInvalid invalid)
            {
                switch (invalid)
                {
                    case RubyTagInvalid.OutOfRange:
                        return "This ruby is not in the lyric.";

                    case RubyTagInvalid.Overlapping:
                        return "This ruby overlapping to other ruby.";

                    case RubyTagInvalid.EmptyText:
                        return "This ruby is empty.";

                    default:
                        throw new IndexOutOfRangeException(nameof(invalid));
                }
            }
        }
    }
}
