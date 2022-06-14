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
    public class RomajiTagIssueSection : Section
    {
        protected override LocalisableString Title => "Invalid romaji-tag";

        private BindableDictionary<Lyric, Issue[]> bindableReports;

        private RomajiTagIssueTable table;

        [BackgroundDependencyLoader]
        private void load(LyricCheckerManager lyricCheckerManager)
        {
            Children = new[]
            {
                table = new RomajiTagIssueTable(),
            };

            bindableReports = lyricCheckerManager.BindableReports.GetBoundCopy();
            bindableReports.BindCollectionChanged((_, _) =>
            {
                var issues = bindableReports.Values.SelectMany(x => x);
                table.Issues = issues.OfType<RomajiTagIssue>();
            }, true);
        }

        private class RomajiTagIssueTable : TextTagIssueTable<RomajiTagInvalid, RomajiTag>
        {
            protected override IEnumerable<Tuple<RomajiTag, RomajiTagInvalid>> GetInvalidByIssue(Issue issue)
            {
                if (issue is not RomajiTagIssue romajiTagIssue)
                    yield break;

                foreach (var (invalidReason, romajiTags) in romajiTagIssue.InvalidRomajiTags)
                {
                    foreach (var romajiTag in romajiTags)
                    {
                        yield return new Tuple<RomajiTag, RomajiTagInvalid>(romajiTag, invalidReason);
                    }
                }
            }

            protected override string GetInvalidMessage(RomajiTagInvalid invalid) =>
                invalid switch
                {
                    RomajiTagInvalid.OutOfRange => "This romaji is not in the lyric.",
                    RomajiTagInvalid.Overlapping => "This romaji overlapping to other romaji.",
                    RomajiTagInvalid.EmptyText => "This romaji is empty.",
                    _ => throw new ArgumentOutOfRangeException(nameof(invalid))
                };
        }
    }
}
