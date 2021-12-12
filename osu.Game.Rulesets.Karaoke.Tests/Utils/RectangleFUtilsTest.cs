// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using osu.Framework.Graphics.Primitives;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    public class RectangleFUtilsTest
    {
        [TestCase(new[] { "[5,5,10,10]" }, "[5,5,10,10]")]
        [TestCase(new[] { "[5,5,10,10]", "[5,5,10,10]" }, "[5,5,10,10]")]
        [TestCase(new[] { "[0,0,0,0]", "[5,5,10,10]" }, "[0,0,15,15]")]
        [TestCase(new[] { "[0,0,0,0]", "[5,0,0,0]", "[0,5,0,0]" }, "[0,0,5,5]")]
        [TestCase(new[] { "" }, "")]
        [TestCase(new string[] { }, "")]
        public void TestUnion(string[] positions, string actual)
        {
            var objects = positions?.Select(convertTestCaseToValue).ToArray();
            var result = RectangleFUtils.Union(objects);
            Assert.AreEqual(result, convertTestCaseToValue(actual));
        }

        private RectangleF convertTestCaseToValue(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new RectangleF();

            var regex = new Regex("(?<x>[-0-9]+),(?<y>[-0-9]+),(?<width>[-0-9]+),(?<height>[-0-9]+)]");
            var result = regex.Match(str);
            if (!result.Success)
                throw new ArgumentException(null, nameof(str));

            float x = result.GetGroupValue<float>("x");
            float y = result.GetGroupValue<float>("y");
            float width = result.GetGroupValue<float>("width");
            float height = result.GetGroupValue<float>("height");
            return new RectangleF(x, y, width, height);
        }
    }
}
