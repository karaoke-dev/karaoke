// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    [TestFixture]
    public class TextTagUtilsTest
    {
        #region valid source

        public static RubyTag[] ValidTextTagWithSorted()
            => new[]
            {
                new RubyTag { StartIndex = 0, EndIndex = 1 },
                new RubyTag { StartIndex = 1, EndIndex = 2 },
                new RubyTag { StartIndex = 2, EndIndex = 3 }
            };

        public static RubyTag[] ValidTextTagWithUnsorted()
            => new[]
            {
                new RubyTag { StartIndex = 0, EndIndex = 1 },
                new RubyTag { StartIndex = 2, EndIndex = 3 },
                new RubyTag { StartIndex = 1, EndIndex = 2 }
            };

        #endregion

        #region invalid source

        public static RubyTag[] InvalidTextTagWithWrongIndex()
            => new[]
            {
                new RubyTag { StartIndex = 1, EndIndex = 0 },
            };

        public static RubyTag[] InvalidTextTagWithNegativeIndex()
            => new[]
            {
                new RubyTag { StartIndex = -1, EndIndex = 0 },
            };

        public static RubyTag[] InvalidTextTagWithEndLargerThenNextStart()
            => new[]
            {
                new RubyTag { StartIndex = 0, EndIndex = 2 }, // End is larger than second start.
                new RubyTag { StartIndex = 1, EndIndex = 2 }
            };

        public static RubyTag[] InvalidTextTagWithWrapNextTextTag()
            => new[]
            {
                new RubyTag { StartIndex = 0, EndIndex = 3 }, // Wrap second text tag.
                new RubyTag { StartIndex = 1, EndIndex = 2 }
            };

        public static RubyTag[] InvalidTextTagWithSameStartAndEndIndex()
            => new[]
            {
                new RubyTag { StartIndex = 0, EndIndex = 0 }, // Same number.
            };

        #endregion
    }
}
