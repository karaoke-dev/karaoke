// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.LyricEditor;
using osu.Game.Rulesets.Karaoke.Objects;
using System;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.LyricEditor
{
    [TestFixture]
    public class TimeTagGeneratorTest
    {
        protected TimeTagGenerator Generator; 

        [SetUp]
        public void SetUp()
        {
            Generator = new TimeTagGenerator();
        }

        [Test]
        public void TestJapaneseLyric(string testCase, int[] index)
        {
            // test japanese with some common case.
            var lyric = getvalueByMethodName(testCase);
            var timneTags = Generator.CreateTimeTagFromJapaneseLyric(lyric);

            // todo : check time tag is metch to result
            Assert.AreEqual(getTimeTagIndex(timneTags), index);
        }

        [Test]
        public void TestEnglishLyric()
        {

        }

        [Test]
        public void TestUnsupooprtedLyric()
        {

        }

        private int[] getTimeTagIndex(Tuple<TimeTagIndex, double?>[] timeTags)
            => timeTags.OrderBy(x => x.Item1).Select((v, i) => i).ToArray();

        private Lyric getvalueByMethodName(string methodName)
        {
            Type thisType = GetType();
            var theMethod = thisType.GetMethod(methodName);
            if (theMethod == null)
                throw new MissingMethodException("Test method is not exist.");

            return theMethod.Invoke(this, null) as Lyric;
        }
    }
}
