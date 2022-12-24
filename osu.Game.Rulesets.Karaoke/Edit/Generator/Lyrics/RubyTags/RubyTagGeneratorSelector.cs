// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RubyTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RubyTags
{
    public class RubyTagGeneratorSelector : LyricGeneratorSelector<RubyTag[], RubyTagGeneratorConfig>
    {
        public RubyTagGeneratorSelector(KaraokeRulesetEditGeneratorConfigManager generatorConfigManager)
            : base(generatorConfigManager)
        {
            RegisterGenerator<JaRubyTagGenerator, JaRubyTagGeneratorConfig>(new CultureInfo(17));
            RegisterGenerator<JaRubyTagGenerator, JaRubyTagGeneratorConfig>(new CultureInfo(1041));
        }

        protected override RubyTag[] GenerateFromItem(Lyric item)
        {
            if (item.Language == null)
                return Array.Empty<RubyTag>();

            if (string.IsNullOrEmpty(item.Text))
                return Array.Empty<RubyTag>();

            if (!Generator.TryGetValue(item.Language, out var generator))
                return Array.Empty<RubyTag>();

            return generator.Value.Generate(item);
        }

        protected override KaraokeRulesetEditGeneratorSetting GetGeneratorConfigSetting(CultureInfo info)
            => KaraokeRulesetEditGeneratorSetting.JaRubyTagGeneratorConfig;
    }
}
