// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Asserts;

public class TimeTagAssert : Assert
{
    public static void ArePropertyEqual(IList<TimeTag> expected, IList<TimeTag> actual)
    {
        That(expected.Count, Is.EqualTo(actual.Count));

        for (int i = 0; i < expected.Count; i++)
        {
            ArePropertyEqual(expected[i], actual[i]);
        }
    }

    public static void ArePropertyEqual(TimeTag expect, TimeTag actually)
    {
        That(expect.Index, Is.EqualTo(actually.Index));
        That(expect.Time, Is.EqualTo(actually.Time));
        That(expect.FirstSyllable, Is.EqualTo(actually.FirstSyllable));
        That(expect.RomanisedSyllable, Is.EqualTo(actually.RomanisedSyllable));
    }
}
