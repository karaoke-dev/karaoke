// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags
{
    public class RubyTagGeneratorSelector : GeneratorSelector<RubyTag[], RubyTagGeneratorConfig>
    {
        public RubyTagGeneratorSelector(KaraokeRulesetEditGeneratorConfigManager generatorConfigManager)
            : base(generatorConfigManager)
        {
            RegisterGenerator<JaRubyTagGenerator, JaRubyTagGeneratorConfig>(new CultureInfo(17));
            RegisterGenerator<JaRubyTagGenerator, JaRubyTagGeneratorConfig>(new CultureInfo(1041));
        }

        public override RubyTag[] Generate(Lyric lyric)
        {
            if (lyric.Language == null)
                return Array.Empty<RubyTag>();

            if (string.IsNullOrEmpty(lyric.Text))
                return Array.Empty<RubyTag>();

            if (!Generator.TryGetValue(lyric.Language, out var generator))
                return Array.Empty<RubyTag>();

            return generator.Value.Generate(lyric);
        }

        protected override KaraokeRulesetEditGeneratorSetting GetGeneratorConfigSetting(CultureInfo info)
            => KaraokeRulesetEditGeneratorSetting.JaRubyTagGeneratorConfig;
    }
}
