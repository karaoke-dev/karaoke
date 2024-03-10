// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks;

public class CheckLyricReferenceLyric : CheckHitObjectReferenceProperty<Lyric, Lyric>
{
    protected override string Description => "Lyric with invalid reference lyric.";

    public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
    {
        new IssueTemplateSelfReference(this),
        new IssueTemplateInvalidReferenceLyric(this),
        new IssueTemplateNullReferenceLyricConfig(this),
        new IssueTemplateHasReferenceLyricConfigWhenNoReferenceLyric(this),
    };

    protected override IEnumerable<Issue> CheckReferenceProperty(Lyric lyric, IEnumerable<Lyric> allAvailableReferencedHitObjects)
    {
        if (lyric.ReferenceLyric == lyric)
            yield return new IssueTemplateSelfReference(this).Create(lyric);

        if (lyric.ReferenceLyric != null && !allAvailableReferencedHitObjects.Contains(lyric.ReferenceLyric))
            yield return new IssueTemplateInvalidReferenceLyric(this).Create(lyric);

        if (lyric.ReferenceLyric != null && lyric.ReferenceLyricConfig == null)
            yield return new IssueTemplateNullReferenceLyricConfig(this).Create(lyric);

        if (lyric.ReferenceLyric == null && lyric.ReferenceLyricConfig != null)
            yield return new IssueTemplateHasReferenceLyricConfigWhenNoReferenceLyric(this).Create(lyric);
    }

    public class IssueTemplateSelfReference : IssueTemplate
    {
        public IssueTemplateSelfReference(ICheck check)
            : base(check, IssueType.Error, "Lyric should not reference to itself.")
        {
        }

        public Issue Create(Lyric lyric)
            => new LyricIssue(lyric, this);
    }

    public class IssueTemplateInvalidReferenceLyric : IssueTemplate
    {
        public IssueTemplateInvalidReferenceLyric(ICheck check)
            : base(check, IssueType.Error, "Reference lyric does not exist in the beatmap.")
        {
        }

        public Issue Create(Lyric lyric)
            => new LyricIssue(lyric, this);
    }

    public class IssueTemplateNullReferenceLyricConfig : IssueTemplate
    {
        public IssueTemplateNullReferenceLyricConfig(ICheck check)
            : base(check, IssueType.Error, "Must have config if reference to another lyric.")
        {
        }

        public Issue Create(Lyric lyric)
            => new LyricIssue(lyric, this);
    }

    public class IssueTemplateHasReferenceLyricConfigWhenNoReferenceLyric : IssueTemplate
    {
        public IssueTemplateHasReferenceLyricConfigWhenNoReferenceLyric(ICheck check)
            : base(check, IssueType.Error, "Should not have the reference lyric config if reference to another lyric.")
        {
        }

        public Issue Create(Lyric lyric)
            => new LyricIssue(lyric, this);
    }
}
