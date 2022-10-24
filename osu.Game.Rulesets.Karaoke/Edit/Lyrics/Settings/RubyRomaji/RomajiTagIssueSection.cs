// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.RubyRomaji.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckLyricRomajiTag;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.RubyRomaji
{
    public class RomajiTagIssueSection : LyricEditorSection
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

                // todo: use better way to get the invalid message.
                table.Issues = issues.Where(x => x.Template is IssueTemplateLyricRomajiOutOfRange);
            }, true);
        }

        private class RomajiTagIssueTable : TextTagIssueTable<RomajiTag>
        {
        }
    }
}
