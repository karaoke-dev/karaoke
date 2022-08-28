// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects
{
    public class RubyTagTest
    {
        [TestCase]
        public void TestClone()
        {
            var rubyTag = new RubyTag
            {
                Text = "ruby",
                StartIndex = 1,
                EndIndex = 2
            };

            var clonedRubyTag = rubyTag.DeepClone();

            Assert.AreNotSame(clonedRubyTag.TextBindable, rubyTag.TextBindable);
            Assert.AreEqual(clonedRubyTag.Text, rubyTag.Text);

            Assert.AreNotSame(clonedRubyTag.StartIndexBindable, rubyTag.StartIndexBindable);
            Assert.AreEqual(clonedRubyTag.StartIndex, rubyTag.StartIndex);

            Assert.AreNotSame(clonedRubyTag.EndIndexBindable, rubyTag.EndIndexBindable);
            Assert.AreEqual(clonedRubyTag.EndIndex, rubyTag.EndIndex);
        }
    }
}
