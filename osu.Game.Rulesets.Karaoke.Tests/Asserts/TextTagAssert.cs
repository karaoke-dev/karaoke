// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Tests.Asserts
{
    public class TextTagAssert : Assert
    {
        public static void ArePropertyEqual<T>(IReadOnlyList<T> expect, IReadOnlyList<T> actually) where T : ITextTag
        {
            AreEqual(expect?.Count, actually?.Count);
            if (expect == null || actually == null)
                return;

            for (int i = 0; i < expect.Count; i++)
            {
                AreEqual(expect[i], actually[i]);
                AreEqual(expect[i], actually[i]);
            }
        }

        public static void ArePropertyEqual<T>(T expect, T actually) where T : ITextTag
        {
            AreEqual(expect.Text, actually.Text);
            AreEqual(expect.StartIndex, actually.StartIndex);
            AreEqual(expect.EndIndex, actually.EndIndex);
        }
    }
}
