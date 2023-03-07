// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.ReferenceLyric
{
    public class ReferenceLyricDetectorConfig : GeneratorConfig
    {
        [ConfigSource("Ruby as Katakana", "Ruby as Katakana.")]
        public Bindable<bool> IgnorePrefixAndPostfixSymbol { get; } = new BindableBool(true);
    }
}
