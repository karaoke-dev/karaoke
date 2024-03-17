// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks;

public class CheckLyricTimeTag : CheckHitObjectProperty<Lyric>
{
    protected override string Description => "Lyric with invalid time-tag.";

    public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
    {
        new IssueTemplateEmpty(this),
        new IssueTemplateMissingStart(this),
        new IssueTemplateMissingEnd(this),
        new IssueTemplateOutOfRange(this),
        new IssueTemplateOverlapping(this),
        new IssueTemplateEmptyTime(this),
        new IssueTemplateInvalidRomanisedSyllable(this),
        new IssueTemplateShouldFillRomanisedSyllable(this),
        new IssueTemplateShouldNotFillRomanisedSyllable(this),
        new IssueTemplateShouldNotMarkFirstSyllable(this),
    };

    protected override IEnumerable<Issue> Check(Lyric lyric)
    {
        var issues = new List<Issue>();
        issues.AddRange(CheckTimeTag(lyric));
        issues.AddRange(CheckTimeTagRomanisedSyllable(lyric));
        return issues;
    }

    protected IEnumerable<Issue> CheckTimeTag(Lyric lyric)
    {
        if (!lyric.TimeTags.Any())
        {
            yield return new IssueTemplateEmpty(this).Create(lyric);

            yield break;
        }

        if (!TimeTagsUtils.HasStartTimeTagInLyric(lyric.TimeTags, lyric.Text))
            yield return new IssueTemplateMissingStart(this).Create(lyric);

        if (!TimeTagsUtils.HasEndTimeTagInLyric(lyric.TimeTags, lyric.Text))
            yield return new IssueTemplateMissingEnd(this).Create(lyric);

        // todo: maybe config?
        const GroupCheck group_check = GroupCheck.Asc;
        const SelfCheck self_check = SelfCheck.BasedOnStart;

        var outOfRangeTags = TimeTagsUtils.FindOutOfRange(lyric.TimeTags, lyric.Text);
        var overlappingTimeTags = TimeTagsUtils.FindOverlapping(lyric.TimeTags, group_check, self_check).ToArray();
        var noTimeTimeTags = TimeTagsUtils.FindNoneTime(lyric.TimeTags);

        foreach (var timeTag in outOfRangeTags)
        {
            yield return new IssueTemplateOutOfRange(this).Create(lyric, timeTag);
        }

        foreach (var timeTag in overlappingTimeTags)
        {
            yield return new IssueTemplateOverlapping(this).Create(lyric, timeTag);
        }

        foreach (var timeTag in noTimeTimeTags)
        {
            yield return new IssueTemplateEmptyTime(this).Create(lyric, timeTag);
        }
    }

    protected IEnumerable<Issue> CheckTimeTagRomanisedSyllable(Lyric lyric)
    {
        if (!lyric.TimeTags.Any())
        {
            yield break;
        }

        foreach (var timeTag in lyric.TimeTags)
        {
            bool firstSyllable = timeTag.FirstSyllable;
            string? romanisedSyllable = timeTag.RomanisedSyllable;

            switch (timeTag.Index.State)
            {
                case TextIndex.IndexState.Start:
                    // if input the romanised syllable, should be valid.
                    if (romanisedSyllable != null && !isRomanisedSyllableValid(romanisedSyllable))
                        yield return new IssueTemplateInvalidRomanisedSyllable(this).Create(lyric, timeTag);

                    // if is first romanised syllable, should not be null.
                    if (firstSyllable && romanisedSyllable == null)
                        yield return new IssueTemplateShouldFillRomanisedSyllable(this).Create(lyric, timeTag);

                    break;

                case TextIndex.IndexState.End:
                    if (romanisedSyllable != null)
                        yield return new IssueTemplateShouldNotFillRomanisedSyllable(this).Create(lyric, timeTag);

                    if (firstSyllable)
                        yield return new IssueTemplateShouldNotMarkFirstSyllable(this).Create(lyric, timeTag);

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        yield break;

        static bool isRomanisedSyllableValid(string text)
        {
            // should not be white-space only.
            if (string.IsNullOrWhiteSpace(text))
                return false;

            // should be all latin text or white-space
            return text.All(c => CharUtils.IsLatin(c) || CharUtils.IsSpacing(c));
        }
    }

    public class IssueTemplateEmpty : IssueTemplate
    {
        public IssueTemplateEmpty(ICheck check)
            : base(check, IssueType.Problem, "This lyric has no time-tag.")
        {
        }

        public Issue Create(Lyric lyric) => new LyricIssue(lyric, this);
    }

    public class IssueTemplateMissingStart : IssueTemplate
    {
        public IssueTemplateMissingStart(ICheck check)
            : base(check, IssueType.Problem, "Missing first time-tag in the lyric.")
        {
        }

        public Issue Create(Lyric lyric) => new LyricIssue(lyric, this);
    }

    public class IssueTemplateMissingEnd : IssueTemplate
    {
        public IssueTemplateMissingEnd(ICheck check)
            : base(check, IssueType.Problem, "Missing last time-tag in the lyric.")
        {
        }

        public Issue Create(Lyric lyric) => new LyricIssue(lyric, this);
    }

    public abstract class IssueTemplateLyricTimeTag : IssueTemplate
    {
        protected IssueTemplateLyricTimeTag(ICheck check, IssueType type, string unformattedMessage)
            : base(check, type, unformattedMessage)
        {
        }

        public Issue Create(Lyric lyric, TimeTag timeTag) => new LyricTimeTagIssue(lyric, this, timeTag, timeTag);
    }

    public class IssueTemplateOutOfRange : IssueTemplateLyricTimeTag
    {
        public IssueTemplateOutOfRange(ICheck check)
            : base(check, IssueType.Problem, "Time-tag index is out of range.")
        {
        }
    }

    public class IssueTemplateOverlapping : IssueTemplateLyricTimeTag
    {
        public IssueTemplateOverlapping(ICheck check)
            : base(check, IssueType.Problem, "Time-tag index is overlapping to another time-tag.")
        {
        }
    }

    public class IssueTemplateEmptyTime : IssueTemplateLyricTimeTag
    {
        public IssueTemplateEmptyTime(ICheck check)
            : base(check, IssueType.Problem, "Time-tag has no time.")
        {
        }
    }

    public class IssueTemplateInvalidRomanisedSyllable : IssueTemplateLyricTimeTag
    {
        public IssueTemplateInvalidRomanisedSyllable(ICheck check)
            : base(check, IssueType.Problem, "Romanised syllable should not be empty or white-space only.")
        {
        }
    }

    public class IssueTemplateShouldFillRomanisedSyllable : IssueTemplateLyricTimeTag
    {
        public IssueTemplateShouldFillRomanisedSyllable(ICheck check)
            : base(check, IssueType.Problem, "Romanised syllable should not be empty or white-space if in the first time-tag.")
        {
        }
    }

    public class IssueTemplateShouldNotFillRomanisedSyllable : IssueTemplateLyricTimeTag
    {
        public IssueTemplateShouldNotFillRomanisedSyllable(ICheck check)
            : base(check, IssueType.Error, "Should not have empty romanised syllable if time-tag is end.")
        {
        }
    }

    public class IssueTemplateShouldNotMarkFirstSyllable : IssueTemplateLyricTimeTag
    {
        public IssueTemplateShouldNotMarkFirstSyllable(ICheck check)
            : base(check, IssueType.Error, "Should not have empty romanised syllable if time-tag is end.")
        {
        }
    }
}
