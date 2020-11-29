// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Utils;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Tests.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps
{
    [TestFixture]
    public class KaraokeBeatmapConversionTest : BeatmapConversionTest<ConvertValue>
    {
        protected override string ResourceAssembly => "osu.Game.Rulesets.Karaoke.Tests";

        public KaraokeBeatmapConversionTest()
        {
            // a trick to get osu! to register karaoke beatmaps
            KaraokeLegacyBeatmapDecoder.Register();
        }

        [TestCase("karaoke-file-samples")]
        public void Test(string name) => base.Test(name);

        protected override IEnumerable<ConvertValue> CreateConvertValue(HitObject hitObject)
        {
            switch (hitObject)
            {
                case Lyric line:
                    yield return createConvertValue(line);

                    break;
            }

            static ConvertValue createConvertValue(Lyric obj) => new ConvertValue
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
