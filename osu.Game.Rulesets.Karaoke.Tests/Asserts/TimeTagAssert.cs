// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Asserts
{
    public class TimeTagAssert : Assert
    {
        public static void AreEqual(IReadOnlyList<TimeTag> expect, IReadOnlyList<TimeTag> actually)
        {
            AreEqual(expect?.Count, actually?.Count);
            if (expect == null || actually == null)
                return;

            for (int i = 0; i < expect.Count; i++)
            {
                AreEqual(expect[i].Index, actually[i].Index);
                AreEqual(expect[i].Time, actually[i].Time);
            }
        }
    }
}
