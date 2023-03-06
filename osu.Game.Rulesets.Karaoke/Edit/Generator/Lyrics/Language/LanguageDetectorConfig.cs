// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Language
{
    public class LanguageDetectorConfig : GeneratorConfig
    {
        public CultureInfo[] AcceptLanguages { get; set; } = Array.Empty<CultureInfo>();

        public LanguageDetectorConfig CreateDefaultConfig() => new();
    }
}
