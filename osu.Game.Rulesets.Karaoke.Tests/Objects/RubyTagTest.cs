// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects;

public class RubyTagTest
{
    [Test]
    public void TestClone()
    {
        var rubyTag = new RubyTag
        {
            Text = "ruby",
            StartIndex = 0,
            EndIndex = 1,
        };

        var clonedRubyTag = rubyTag.DeepClone();

        Assert.That(clonedRubyTag.TextBindable, Is.Not.SameAs(rubyTag.TextBindable));
        Assert.That(rubyTag.Text, Is.EqualTo(clonedRubyTag.Text));

        Assert.That(clonedRubyTag.StartIndexBindable, Is.Not.SameAs(rubyTag.StartIndexBindable));
        Assert.That(rubyTag.StartIndex, Is.EqualTo(clonedRubyTag.StartIndex));

        Assert.That(clonedRubyTag.EndIndexBindable, Is.Not.SameAs(rubyTag.EndIndexBindable));
        Assert.That(rubyTag.EndIndex, Is.EqualTo(clonedRubyTag.EndIndex));
    }
}
