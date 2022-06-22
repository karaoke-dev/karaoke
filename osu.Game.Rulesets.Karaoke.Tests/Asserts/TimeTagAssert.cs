// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Asserts
{
    public class TimeTagAssert : Assert
    {
        public static void ArePropertyEqual(IList<TimeTag> expected, IList<TimeTag> actual)
        {
            AreEqual(expected?.Count, actual?.Count);
            if (expected == null || actual == null)
                return;

            for (int i = 0; i < expected.Count; i++)
            {
                ArePropertyEqual(expected[i], actual[i]);
            }
        }

        public static void ArePropertyEqual(TimeTag expect, TimeTag actually)
        {
            AreEqual(expect.Index, actually.Index);
            AreEqual(expect.Time, actually.Time);
        }
    }
}
