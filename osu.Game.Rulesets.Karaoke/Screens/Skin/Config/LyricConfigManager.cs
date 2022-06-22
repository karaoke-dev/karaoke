// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Config
{
    public class LyricConfigManager : Component
    {
        public readonly BindableList<LyricConfig> Configs = new();

        public readonly Bindable<LyricConfig> LoadedLyricConfig = new();

        public readonly Bindable<LyricConfig> EditLyricConfig = new();

        [Resolved]
        private ISkinSource source { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            var lookup = new KaraokeSkinLookup(ElementType.LyricConfig);
            var lyricConfig = source.GetConfig<KaraokeSkinLookup, LyricConfig>(lookup)?.Value;
            if (lyricConfig != null)
                Configs.Add(lyricConfig);

            LoadedLyricConfig.Value = Configs.FirstOrDefault();
            EditLyricConfig.Value = Configs.FirstOrDefault();
        }

        public void ApplyCurrentLyricConfigChange(Action<LyricConfig> action)
        {
            action?.Invoke(LoadedLyricConfig.Value);
            LoadedLyricConfig.TriggerChange();
        }
    }
}
