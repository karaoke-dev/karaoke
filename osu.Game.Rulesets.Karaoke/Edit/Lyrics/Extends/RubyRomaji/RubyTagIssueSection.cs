// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Localisation;
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
        protected override LocalisableString Title => "Invalid ruby-tag";

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
            bindableReports.BindCollectionChanged((_, _) =>
            {
                var issues = bindableReports.Values.SelectMany(x => x);
                table.Issues = issues.OfType<RubyTagIssue>();
            }, true);
        }

        private class RubyTagIssueTable : TextTagIssueTable<RubyTagInvalid, RubyTag>
        {
            protected override IEnumerable<Tuple<RubyTag, RubyTagInvalid>> GetInvalidByIssue(Issue issue)
            {
                if (issue is not RubyTagIssue rubyTagIssue)
                    yield break;

                foreach (var (invalidReason, rubyTags) in rubyTagIssue.InvalidRubyTags)
                {
                    foreach (var rubyTag in rubyTags)
                    {
                        yield return new Tuple<RubyTag, RubyTagInvalid>(rubyTag, invalidReason);
                    }
                }
            }

            protected override string GetInvalidMessage(RubyTagInvalid invalid) =>
                invalid switch
                {
                    RubyTagInvalid.OutOfRange => "This ruby is not in the lyric.",
                    RubyTagInvalid.Overlapping => "This ruby overlapping to other ruby.",
                    RubyTagInvalid.EmptyText => "This ruby is empty.",
                    _ => throw new ArgumentOutOfRangeException(nameof(invalid))
                };
        }
    }
}
