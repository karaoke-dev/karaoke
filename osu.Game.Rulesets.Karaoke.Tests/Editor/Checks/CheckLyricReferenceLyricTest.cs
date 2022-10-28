// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Rulesets.Objects;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckLyricReferenceLyric;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks
{
    public class CheckLyricReferenceLyricTest : HitObjectCheckTest<Lyric, CheckLyricReferenceLyric>
    {
        [Test]
        public void TestCheck()
        {
            var referencedLyric = new Lyric();
            var lyric = new Lyric
            {
                ReferenceLyric = referencedLyric,
                ReferenceLyricConfig = new ReferenceLyricConfig(),
            };

            AssertOk(new HitObject[] { referencedLyric, lyric });
        }

        [Test]
        public void TestCheckSelfReference()
        {
            var lyric = new Lyric
            {
                ReferenceLyricConfig = new ReferenceLyricConfig(),
            };

            lyric.ReferenceLyric = lyric;

            AssertNotOk<IssueTemplateLyricSelfReference>(lyric);
        }

        [Test]
        public void TestCheckInvalidReferenceLyric()
        {
            var referencedLyric = new Lyric();
            var lyric = new Lyric
            {
                ReferenceLyric = referencedLyric,
                ReferenceLyricConfig = new ReferenceLyricConfig(),
            };

            AssertNotOk<IssueTemplateLyricInvalidReferenceLyric>(lyric);
        }

        [Test]
        public void TestCheckNullReferenceLyricConfig()
        {
            var referencedLyric = new Lyric();
            var lyric = new Lyric
            {
                ReferenceLyric = referencedLyric,
            };

            AssertNotOk<IssueTemplateLyricNullReferenceLyricConfig>(new HitObject[] { referencedLyric, lyric });
        }

        [Test]
        public void TestCheckHasReferenceLyricConfigIfNoReferenceLyric()
        {
            var lyric = new Lyric
            {
                ReferenceLyric = null,
                ReferenceLyricConfig = new ReferenceLyricConfig(),
            };

            AssertNotOk<IssueTemplateLyricHasReferenceLyricConfigIfNoReferenceLyric>(lyric);
        }
    }
}
