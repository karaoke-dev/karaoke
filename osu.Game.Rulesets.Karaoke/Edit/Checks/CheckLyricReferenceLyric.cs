// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public class CheckLyricReferenceLyric : CheckHitObjectProperty<Lyric>
    {
        protected override string Description => "Lyric with invalid reference lyric.";

        public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
        {
            new IssueTemplateLyricSelfReference(this),
            new IssueTemplateLyricInvalidReferenceLyric(this),
            new IssueTemplateLyricNullReferenceLyricConfig(this),
            new IssueTemplateLyricHasReferenceLyricConfigIfNoReferenceLyric(this)
        };

        protected override IEnumerable<Issue> Check(Lyric lyric)
        {
            yield break;
        }

        protected override IEnumerable<Issue> CheckAllHitObject(IReadOnlyList<HitObject> hitObjects)
        {
            var lyrics = hitObjects.OfType<Lyric>().ToArray();

            foreach (var lyric in lyrics)
            {
                if (lyric.ReferenceLyric == lyric)
                    yield return new IssueTemplateLyricSelfReference(this).Create(lyric);

                if (lyric.ReferenceLyric != null && !lyrics.Contains(lyric.ReferenceLyric))
                    yield return new IssueTemplateLyricInvalidReferenceLyric(this).Create(lyric);

                if (lyric.ReferenceLyric != null && lyric.ReferenceLyricConfig == null)
                    yield return new IssueTemplateLyricNullReferenceLyricConfig(this).Create(lyric);

                if (lyric.ReferenceLyric == null && lyric.ReferenceLyricConfig != null)
                    yield return new IssueTemplateLyricHasReferenceLyricConfigIfNoReferenceLyric(this).Create(lyric);
            }
        }

        public class IssueTemplateLyricSelfReference : IssueTemplate
        {
            public IssueTemplateLyricSelfReference(ICheck check)
                : base(check, IssueType.Error, "Lyric should not reference to self.")
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
                : base(check, IssueType.Error, "Reference lyric should have the config also.")
            {
            }

            public Issue Create(Lyric lyric)
                => new(lyric, this);
        }

        public class IssueTemplateLyricHasReferenceLyricConfigIfNoReferenceLyric : IssueTemplate
        {
            public IssueTemplateLyricHasReferenceLyricConfigIfNoReferenceLyric(ICheck check)
                : base(check, IssueType.Error, "Should not have the reference lyric config if contains no reference lyric.")
            {
            }

            public Issue Create(Lyric lyric)
                => new(lyric, this);
        }
    }
}
