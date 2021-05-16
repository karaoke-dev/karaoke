// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public class CheckInvalidPropertyLyrics : ICheck
    {
        public CheckMetadata Metadata => new CheckMetadata(CheckCategory.HitObjects, "Lyrics with invalid property.");

        public IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
        {
            new IssueTemplateNotFillLanguage(this),
            new IssueTemplateNoText(this),
            new IssueTemplateNoSinger(this),
        };

        public IEnumerable<Issue> Run(BeatmapVerifierContext context)
        {
            foreach (var lyric in context.Beatmap.HitObjects.OfType<Lyric>())
            {
                if (lyric.Language == null)
                    yield return new IssueTemplateNotFillLanguage(this).Create(lyric);

                // todo : check lyric layout.

                if (string.IsNullOrWhiteSpace(lyric.Text))
                    yield return new IssueTemplateNoText(this).Create(lyric);

                if (lyric.Singers == null || lyric.Singers.Length == 0)
                    yield return new IssueTemplateNoSinger(this).Create(lyric);

                // todo : check is singer in singer list.
            }
        }

        public class IssueTemplateNotFillLanguage : IssueTemplate
        {
            public IssueTemplateNotFillLanguage(ICheck check)
                : base(check, IssueType.Problem, "Lyric must have assign language.")
            {
            }

            public Issue Create(Lyric lyric)
                => new Issue(lyric, this);
        }

        public class IssueTemplateNoText : IssueTemplate
        {
            public IssueTemplateNoText(ICheck check)
                : base(check, IssueType.Problem, "Lyric must have text.")
            {
            }

            public Issue Create(Lyric lyric)
                => new Issue(lyric, this);
        }

        public class IssueTemplateNoSinger : IssueTemplate
        {
            public IssueTemplateNoSinger(ICheck check)
                : base(check, IssueType.Problem, "Lyric must have at least one singer.")
            {
            }

            public Issue Create(Lyric lyric)
                => new Issue(lyric, this);
        }
    }
}
