// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Asserts;

public class RubyTagAssert : Assert
{
    public static void ArePropertyEqual(IList<RubyTag> expected, IList<RubyTag> actual)
    {
        That(expected.Count, Is.EqualTo(actual.Count));

        for (int i = 0; i < expected.Count; i++)
        {
            ArePropertyEqual(expected[i], actual[i]);
            ArePropertyEqual(expected[i], actual[i]);
        }
    }

    public static void ArePropertyEqual(RubyTag expected, RubyTag actual)
    {
        That(expected.Text, Is.EqualTo(actual.Text));
        That(expected.StartIndex, Is.EqualTo(actual.StartIndex));
        That(expected.EndIndex, Is.EqualTo(actual.EndIndex));
    }
}
