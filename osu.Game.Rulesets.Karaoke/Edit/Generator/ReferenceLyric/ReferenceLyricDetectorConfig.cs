// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.ReferenceLyric
{
    public class ReferenceLyricDetectorConfig : IHasConfig<ReferenceLyricDetectorConfig>
    {
        public bool IgnorePrefixAndPostfixSymbol { get; set; }

        public ReferenceLyricDetectorConfig CreateDefaultConfig() => new()
        {
            IgnorePrefixAndPostfixSymbol = true,
        };
    }
}
