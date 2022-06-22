// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Tests.Asserts
{
    public class TextTagAssert : Assert
    {
        public static void ArePropertyEqual<T>(IList<T> expected, IList<T> actual) where T : ITextTag
        {
            AreEqual(expected?.Count, actual?.Count);
            if (expected == null || actual == null)
                return;

            for (int i = 0; i < expected.Count; i++)
            {
                ArePropertyEqual(expected[i], actual[i]);
                ArePropertyEqual(expected[i], actual[i]);
            }
        }

        public static void ArePropertyEqual<T>(T expected, T actual) where T : ITextTag
        {
            AreEqual(expected.Text, actual.Text);
            AreEqual(expected.StartIndex, actual.StartIndex);
            AreEqual(expected.EndIndex, actual.EndIndex);
        }
    }
}
