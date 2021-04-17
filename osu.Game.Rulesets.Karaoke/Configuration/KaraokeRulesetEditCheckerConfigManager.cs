// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Configs;

namespace osu.Game.Rulesets.Karaoke.Configuration
{
    public class KaraokeRulesetEditCheckerConfigManager : InMemoryConfigManager<KaraokeRulesetEditCheckerSetting>
    {
        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();

            // Lyric
            SetDefault(KaraokeRulesetEditCheckerSetting.Lyric, CreateDefaultConfig<LyricCheckerConfig>());
        }

        protected static T CreateDefaultConfig<T>() where T : IHasConfig<T>, new()
            => new T().CreateDefaultConfig();
    }

    public enum KaraokeRulesetEditCheckerSetting
    {
        Lyric
    }
}
