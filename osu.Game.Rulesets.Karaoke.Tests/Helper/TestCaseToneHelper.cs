// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Helper
{
    public static class TestCaseToneHelper
    {
        public static Tone NumberToTone(double tone)
        {
            bool half = Math.Abs(tone) % 1 == 0.5;
            int scale = tone < 0 ? (int)tone - (half ? 1 : 0) : (int)tone;
            return new Tone(scale, half);
        }
    }
}
