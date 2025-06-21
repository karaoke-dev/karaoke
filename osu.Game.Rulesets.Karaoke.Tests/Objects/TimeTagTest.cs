// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects;

public class TimeTagTest
{
    [Test]
    public void TestClone()
    {
        var timeTag = new TimeTag(new TextIndex(1, TextIndex.IndexState.End), 1000)
        {
            FirstSyllable = true,
            RomanisedSyllable = "karaoke",
        };

        var clonedTimeTag = timeTag.DeepClone();

        Assert.That(timeTag.Index, Is.EqualTo(clonedTimeTag.Index));

        Assert.That(clonedTimeTag.TimeBindable, Is.Not.SameAs(timeTag.TimeBindable));
        Assert.That(timeTag.Time, Is.EqualTo(clonedTimeTag.Time));
        Assert.That(clonedTimeTag.FirstSyllable, Is.Not.SameAs(timeTag.FirstSyllable));
        Assert.That(timeTag.RomanisedSyllable, Is.EqualTo(clonedTimeTag.RomanisedSyllable));
    }
}
