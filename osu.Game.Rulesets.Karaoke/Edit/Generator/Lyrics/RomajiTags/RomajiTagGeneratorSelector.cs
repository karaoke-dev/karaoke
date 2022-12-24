// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RomajiTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RomajiTags
{
    public class RomajiTagGeneratorSelector : LyricGeneratorSelector<RomajiTag[], RomajiTagGeneratorConfig>
    {
        public RomajiTagGeneratorSelector(KaraokeRulesetEditGeneratorConfigManager generatorConfigManager)
            : base(generatorConfigManager)
        {
            RegisterGenerator<JaRomajiTagGenerator, JaRomajiTagGeneratorConfig>(new CultureInfo(17));
            RegisterGenerator<JaRomajiTagGenerator, JaRomajiTagGeneratorConfig>(new CultureInfo(1041));
        }

        protected override RomajiTag[] GenerateFromItem(Lyric item)
        {
            if (item.Language == null)
                return Array.Empty<RomajiTag>();

            if (string.IsNullOrEmpty(item.Text))
                return Array.Empty<RomajiTag>();

            if (!Generator.TryGetValue(item.Language, out var generator))
                return Array.Empty<RomajiTag>();

            return generator.Value.Generate(item);
        }

        protected override KaraokeRulesetEditGeneratorSetting GetGeneratorConfigSetting(CultureInfo info)
            => KaraokeRulesetEditGeneratorSetting.JaRomajiTagGeneratorConfig;
    }
}
