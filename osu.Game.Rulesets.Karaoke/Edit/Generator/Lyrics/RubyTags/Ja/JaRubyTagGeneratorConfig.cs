// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RubyTags.Ja
{
    public class JaRubyTagGeneratorConfig : RubyTagGeneratorConfig
    {
        /// <summary>
        /// Generate ruby as Katakana.
        /// </summary>
        [ConfigSource("Ruby as Katakana", "Ruby as Katakana.")]
        public Bindable<bool> RubyAsKatakana { get; } = new BindableBool();

        /// <summary>
        /// Generate ruby even it's same as lyric.
        /// </summary>
        [ConfigSource("Enable duplicated ruby.", "Enable output duplicated ruby even it's match with lyric.")]
        public Bindable<bool> EnableDuplicatedRuby { get; } = new BindableBool();
    }
}
