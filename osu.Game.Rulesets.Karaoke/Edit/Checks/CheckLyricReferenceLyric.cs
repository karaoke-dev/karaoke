// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public class CheckLyricReferenceLyric : CheckHitObjectReferenceProperty<Lyric, Lyric>
    {
        protected override string Description => "Lyric with invalid reference lyric.";

        public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
        {
            new IssueTemplateLyricSelfReference(this),
            new IssueTemplateLyricInvalidReferenceLyric(this),
            new IssueTemplateLyricNullReferenceLyricConfig(this),
            new IssueTemplateLyricHasReferenceLyricConfigIfNoReferenceLyric(this)
        };

        protected override IEnumerable<Issue> CheckReferenceProperty(Lyric lyric, IEnumerable<Lyric> allAvailableReferencedHitObjects)
        {
            if (lyric.ReferenceLyric == lyric)
                yield return new IssueTemplateLyricSelfReference(this).Create(lyric);

            if (lyric.ReferenceLyric != null && !allAvailableReferencedHitObjects.Contains(lyric.ReferenceLyric))
                yield return new IssueTemplateLyricInvalidReferenceLyric(this).Create(lyric);

            if (lyric.ReferenceLyric != null && lyric.ReferenceLyricConfig == null)
                yield return new IssueTemplateLyricNullReferenceLyricConfig(this).Create(lyric);

            if (lyric.ReferenceLyric == null && lyric.ReferenceLyricConfig != null)
                yield return new IssueTemplateLyricHasReferenceLyricConfigIfNoReferenceLyric(this).Create(lyric);
        }

        public class IssueTemplateLyricSelfReference : IssueTemplate
        {
            public IssueTemplateLyricSelfReference(ICheck check)
                : base(check, IssueType.Error, "Lyric should not reference to itself.")
            {
            }

            public Issue Create(Lyric lyric)
                => new(lyric, this);
        }

        public class IssueTemplateLyricInvalidReferenceLyric : IssueTemplate
        {
            public IssueTemplateLyricInvalidReferenceLyric(ICheck check)
                : base(check, IssueType.Error, "Reference lyric does not exist in the beatmap.")
            {
            }

            public Issue Create(Lyric lyric)
                => new(lyric, this);
        }

        public class IssueTemplateLyricNullReferenceLyricConfig : IssueTemplate
        {
            public IssueTemplateLyricNullReferenceLyricConfig(ICheck check)
                : base(check, IssueType.Error, "Must have config if reference to another lyric.")
            {
            }

            public Issue Create(Lyric lyric)
                => new(lyric, this);
        }

        public class IssueTemplateLyricHasReferenceLyricConfigIfNoReferenceLyric : IssueTemplate
        {
            public IssueTemplateLyricHasReferenceLyricConfigIfNoReferenceLyric(ICheck check)
                : base(check, IssueType.Error, "Should not have the reference lyric config if reference to another lyric.")
            {
            }

            public Issue Create(Lyric lyric)
                => new(lyric, this);
        }
    }
}
