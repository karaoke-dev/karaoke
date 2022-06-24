// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Languages
{
    public class LanguageDetectorConfig : IHasConfig<LanguageDetectorConfig>
    {
        public CultureInfo[] AcceptLanguages { get; set; } = Array.Empty<CultureInfo>();

        public LanguageDetectorConfig CreateDefaultConfig() => new();
    }
}
