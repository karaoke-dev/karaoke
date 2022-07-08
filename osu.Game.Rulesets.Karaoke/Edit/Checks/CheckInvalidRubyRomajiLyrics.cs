// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Configs;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public class CheckInvalidRubyRomajiLyrics : ICheck
    {
        private readonly LyricCheckerConfig config;

        public CheckMetadata Metadata => new(CheckCategory.HitObjects, "Lyrics with invalid ruby/romaji.");

        public IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
        {
            new IssueTemplateInvalidRuby(this),
            new IssueTemplateInvalidRomaji(this),
        };

        public CheckInvalidRubyRomajiLyrics(LyricCheckerConfig config)
        {
            this.config = config;
        }

        public IEnumerable<Issue> Run(BeatmapVerifierContext context)
        {
            foreach (var lyric in context.Beatmap.HitObjects.OfType<Lyric>())
            {
                var invalidRubyTags = checkInvalidRubyTags(lyric);
                if (invalidRubyTags.Any())
                    yield return new IssueTemplateInvalidRuby(this).Create(lyric, invalidRubyTags);

                var invalidRomajiTags = checkInvalidRomajiTags(lyric);
                if (invalidRomajiTags.Any())
                    yield return new IssueTemplateInvalidRomaji(this).Create(lyric, invalidRomajiTags);
            }
        }

        private Dictionary<RubyTagInvalid, RubyTag[]> checkInvalidRubyTags(Lyric lyric)
        {
            var result = new Dictionary<RubyTagInvalid, RubyTag[]>();

            // Checking out of range tags.
            var outOfRangeTags = TextTagsUtils.FindOutOfRange(lyric.RubyTags, lyric.Text);
            if (outOfRangeTags.Length > 0)
                result.Add(RubyTagInvalid.OutOfRange, outOfRangeTags);

            // Checking overlapping.
            var sorting = config.RomajiPositionSorting;
            var overlappingTags = TextTagsUtils.FindOverlapping(lyric.RubyTags, sorting);
            if (overlappingTags.Length > 0)
                result.Add(RubyTagInvalid.Overlapping, overlappingTags);

            // check empty string.
            var emptyTextTags = TextTagsUtils.FindEmptyText(lyric.RubyTags);
            if (emptyTextTags.Length > 0)
                result.Add(RubyTagInvalid.EmptyText, emptyTextTags);

            return result;
        }

        private Dictionary<RomajiTagInvalid, RomajiTag[]> checkInvalidRomajiTags(Lyric lyric)
        {
            var result = new Dictionary<RomajiTagInvalid, RomajiTag[]>();

            // Checking out of range tags.
            var outOfRangeTags = TextTagsUtils.FindOutOfRange(lyric.RomajiTags, lyric.Text);
            if (outOfRangeTags.Length > 0)
                result.Add(RomajiTagInvalid.OutOfRange, outOfRangeTags);

            // Checking overlapping.
            var sorting = config.RomajiPositionSorting;
            var overlappingTags = TextTagsUtils.FindOverlapping(lyric.RomajiTags, sorting);
            if (overlappingTags.Length > 0)
                result.Add(RomajiTagInvalid.Overlapping, overlappingTags);

            // check empty string.
            var emptyTextTags = TextTagsUtils.FindEmptyText(lyric.RomajiTags);
            if (emptyTextTags.Length > 0)
                result.Add(RomajiTagInvalid.EmptyText, emptyTextTags);

            return result;
        }

        public class IssueTemplateInvalidRuby : IssueTemplate
        {
            public IssueTemplateInvalidRuby(ICheck check)
                : base(check, IssueType.Problem, "This lyric contains invalid ruby.")
            {
            }

            public Issue Create(Lyric lyric, Dictionary<RubyTagInvalid, RubyTag[]> invalidRubyTags)
                => new RubyTagIssue(lyric, this, invalidRubyTags);
        }

        public class IssueTemplateInvalidRomaji : IssueTemplate
        {
            public IssueTemplateInvalidRomaji(ICheck check)
                : base(check, IssueType.Problem, "This lyric contains invalid romaji.")
            {
            }

            public Issue Create(Lyric lyric, Dictionary<RomajiTagInvalid, RomajiTag[]> invalidRomajiTags)
                => new RomajiTagIssue(lyric, this, invalidRomajiTags);
        }
    }
}
