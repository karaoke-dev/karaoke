// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags.Ja
{
    public class JaRomajiTagGeneratorConfig : RomajiTagGeneratorConfig, IHasConfig<JaRomajiTagGeneratorConfig>
    {
        /// <summary>
        /// Generate romaji as uppercase.
        /// </summary>
        public bool Uppercase { get; set; }

        public JaRomajiTagGeneratorConfig CreateDefaultConfig() => new();
    }
}
