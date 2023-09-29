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

namespace osu.Game.Rulesets.Karaoke.Edit.Checks;

public class CheckLyricTimeTag : CheckHitObjectProperty<Lyric>
{
    protected override string Description => "Lyric with invalid time-tag.";

    public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
    {
        new IssueTemplateLyricEmptyTimeTag(this),
        new IssueTemplateLyricMissingFirstTimeTag(this),
        new IssueTemplateLyricMissingLastTimeTag(this),
        new IssueTemplateLyricTimeTagOutOfRange(this),
        new IssueTemplateLyricTimeTagOverlapping(this),
        new IssueTemplateLyricTimeTagEmptyTime(this),
        new IssueTemplateLyricTimeTagRomajiInvalidText(this),
        new IssueTemplateLyricTimeTagRomajiInvalidTextIfFirst(this),
        new IssueTemplateLyricTimeTagRomajiNotHaveEmptyTextIfEnd(this),
        new IssueTemplateLyricTimeTagRomajiNotFistRomajiTextIfEnd(this),
    };

    protected override IEnumerable<Issue> Check(Lyric lyric)
    {
        var issues = new List<Issue>();
        issues.AddRange(CheckTimeTag(lyric));
        issues.AddRange(CheckTimeTagRomaji(lyric));
        return issues;
    }

    protected IEnumerable<Issue> CheckTimeTag(Lyric lyric)
    {
        if (!lyric.TimeTags.Any())
        {
            yield return new IssueTemplateLyricEmptyTimeTag(this).Create(lyric);

            yield break;
        }

        if (!TimeTagsUtils.HasStartTimeTagInLyric(lyric.TimeTags, lyric.Text))
            yield return new IssueTemplateLyricMissingFirstTimeTag(this).Create(lyric);

        if (!TimeTagsUtils.HasEndTimeTagInLyric(lyric.TimeTags, lyric.Text))
            yield return new IssueTemplateLyricMissingLastTimeTag(this).Create(lyric);

        // todo: maybe config?
        const GroupCheck group_check = GroupCheck.Asc;
        const SelfCheck self_check = SelfCheck.BasedOnStart;

        var outOfRangeTags = TimeTagsUtils.FindOutOfRange(lyric.TimeTags, lyric.Text);
        var overlappingTimeTags = TimeTagsUtils.FindOverlapping(lyric.TimeTags, group_check, self_check).ToArray();
        var noTimeTimeTags = TimeTagsUtils.FindNoneTime(lyric.TimeTags);

        foreach (var textTag in outOfRangeTags)
        {
            yield return new IssueTemplateLyricTimeTagOutOfRange(this).Create(lyric, textTag);
        }

        foreach (var textTag in overlappingTimeTags)
        {
            yield return new IssueTemplateLyricTimeTagOverlapping(this).Create(lyric, textTag);
        }

        foreach (var textTag in noTimeTimeTags)
        {
            yield return new IssueTemplateLyricTimeTagEmptyTime(this).Create(lyric, textTag);
        }
    }

    protected IEnumerable<Issue> CheckTimeTagRomaji(Lyric lyric)
    {
        if (!lyric.TimeTags.Any())
        {
            yield break;
        }

        foreach (var timeTag in lyric.TimeTags)
        {
            switch (timeTag.Index.State)
            {
                case TextIndex.IndexState.Start:
                    if (!timeTag.InitialRomaji)
                    {
                        // romaji text in the time-tag should be null.
                        if (timeTag.RomajiText == null)
                            break;

                        // but should not be empty or white-space only.
                        if (string.IsNullOrWhiteSpace(timeTag.RomajiText))
                            yield return new IssueTemplateLyricTimeTagRomajiInvalidText(this).Create(lyric, timeTag);
                    }
                    else
                    {
                        // if is first romaji text, should not be null.
                        if (string.IsNullOrWhiteSpace(timeTag.RomajiText))
                            yield return new IssueTemplateLyricTimeTagRomajiInvalidTextIfFirst(this).Create(lyric, timeTag);
                    }

                    break;

                case TextIndex.IndexState.End:
                    if (timeTag.RomajiText != null)
                        yield return new IssueTemplateLyricTimeTagRomajiNotHaveEmptyTextIfEnd(this).Create(lyric, timeTag);

                    if (timeTag.InitialRomaji)
                        yield return new IssueTemplateLyricTimeTagRomajiNotFistRomajiTextIfEnd(this).Create(lyric, timeTag);

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class IssueTemplateLyricEmptyTimeTag : IssueTemplate
    {
        public IssueTemplateLyricEmptyTimeTag(ICheck check)
            : base(check, IssueType.Problem, "This lyric has no time-tag.")
        {
        }

        public Issue Create(Lyric lyric) => new LyricIssue(lyric, this);
    }

    public class IssueTemplateLyricMissingFirstTimeTag : IssueTemplate
    {
        public IssueTemplateLyricMissingFirstTimeTag(ICheck check)
            : base(check, IssueType.Problem, "Missing first time-tag in the lyric.")
        {
        }

        public Issue Create(Lyric lyric) => new LyricIssue(lyric, this);
    }

    public class IssueTemplateLyricMissingLastTimeTag : IssueTemplate
    {
        public IssueTemplateLyricMissingLastTimeTag(ICheck check)
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

    public class IssueTemplateLyricTimeTagOutOfRange : IssueTemplateLyricTimeTag
    {
        public IssueTemplateLyricTimeTagOutOfRange(ICheck check)
            : base(check, IssueType.Problem, "Time-tag index is out of range.")
        {
        }
    }

    public class IssueTemplateLyricTimeTagOverlapping : IssueTemplateLyricTimeTag
    {
        public IssueTemplateLyricTimeTagOverlapping(ICheck check)
            : base(check, IssueType.Problem, "Time-tag index is overlapping to another time-tag.")
        {
        }
    }

    public class IssueTemplateLyricTimeTagEmptyTime : IssueTemplateLyricTimeTag
    {
        public IssueTemplateLyricTimeTagEmptyTime(ICheck check)
            : base(check, IssueType.Problem, "Time-tag has no time.")
        {
        }
    }

    public class IssueTemplateLyricTimeTagRomajiInvalidText : IssueTemplateLyricTimeTag
    {
        public IssueTemplateLyricTimeTagRomajiInvalidText(ICheck check)
            : base(check, IssueType.Problem, "Time-tag romaji text should not be empty or white-space only.")
        {
        }
    }

    public class IssueTemplateLyricTimeTagRomajiInvalidTextIfFirst : IssueTemplateLyricTimeTag
    {
        public IssueTemplateLyricTimeTagRomajiInvalidTextIfFirst(ICheck check)
            : base(check, IssueType.Problem, "Time-tag romaji text should not be empty or white-space if is the first romaji text.")
        {
        }
    }

    public class IssueTemplateLyricTimeTagRomajiNotHaveEmptyTextIfEnd : IssueTemplateLyricTimeTag
    {
        public IssueTemplateLyricTimeTagRomajiNotHaveEmptyTextIfEnd(ICheck check)
            : base(check, IssueType.Error, "Should not have empty romaji text if time-tag is end.")
        {
        }
    }

    public class IssueTemplateLyricTimeTagRomajiNotFistRomajiTextIfEnd : IssueTemplateLyricTimeTag
    {
        public IssueTemplateLyricTimeTagRomajiNotFistRomajiTextIfEnd(ICheck check)
            : base(check, IssueType.Error, "Should not have empty romaji text if time-tag is end.")
        {
        }
    }
}
