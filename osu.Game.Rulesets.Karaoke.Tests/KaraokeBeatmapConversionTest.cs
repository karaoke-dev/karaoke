// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Utils;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Tests.Beatmaps;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    [TestFixture]
    public class KaraokeBeatmapConversionTest : BeatmapConversionTest<ConvertValue>
    {
        protected override string ResourceAssembly => "osu.Game.Rulesets.Karaoke.Tests";

        public KaraokeBeatmapConversionTest()
        {
            // It's a tricky to let osu! to read karaoke testing beatmap
            KaraokeLegacyBeatmapDecoder.Register();
        }

        [TestCase("karaoke-file-samples")]
        public void Test(string name) => base.Test(name);

        protected override IEnumerable<ConvertValue> CreateConvertValue(HitObject hitObject)
        {
            switch (hitObject)
            {
                case LyricLine line:
                    yield return createConvertValue(line);

                    break;
            }

            ConvertValue createConvertValue(LyricLine obj) => new ConvertValue
            {
                StartTime = obj.StartTime,
                EndTime = obj.EndTime,
                Lyric = obj.Text
            };
        }

        protected override Ruleset CreateRuleset() => new KaraokeRuleset();
    }

    public struct ConvertValue : IEquatable<ConvertValue>
    {
        /// <summary>
        /// A sane value to account for osu!stable using <see cref="int"/>s everywhere.
        /// </summary>
        private const double conversion_lenience = 2;

        public double StartTime;
        public double EndTime;
        public string Lyric;

        public bool Equals(ConvertValue other)
            => Precision.AlmostEquals(StartTime, other.StartTime, conversion_lenience)
               && Precision.AlmostEquals(EndTime, other.EndTime, conversion_lenience)
               && Lyric == other.Lyric;
    }
}
