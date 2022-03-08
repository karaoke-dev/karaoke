// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Tests.Helper
{
    public static class TestCaseListHelper
    {
        private static readonly Random rng = new();

        /// <summary>
        /// Random the order of the list.
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IList<T> Shuffle<T>(IList<T> list)
        {
            // see: https://stackoverflow.com/a/4262134
            return list.OrderBy(_ => rng.Next()).ToList();
        }
    }
}
