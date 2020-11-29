// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags.Ja
{
    public class JaRubyTagGeneratorConfig : RubyTagGeneratorConfig
    {
        /// <summary>
        /// Generate ruby as Katakana.
        /// </summary>
        public bool RubyAsKatakana { get; set; }

        /// <summary>
        /// Generate ruby even it's same as lyric.
        /// </summary>
        public bool EnableDuplicatedRuby { get; set; }
    }
}
