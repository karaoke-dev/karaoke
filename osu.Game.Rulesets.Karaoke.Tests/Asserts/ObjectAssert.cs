// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;

namespace osu.Game.Rulesets.Karaoke.Tests.Asserts
{
    public class ObjectAssert : Assert
    {
        public static void ArePropertyEqual<T>(T expect, T actual) where T : class
        {
            AreEqual(JsonConvert.SerializeObject(expect), JsonConvert.SerializeObject(expect));
        }
    }
}
