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
    public partial class LyricFontInfoManager : Component
    {
        public readonly BindableList<LyricFontInfo> Configs = new();

        public readonly Bindable<LyricFontInfo> LoadedLyricFontInfo = new();

        public readonly Bindable<LyricFontInfo> EditLyricFontInfo = new();

        [Resolved]
        private ISkinSource source { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            var lookup = new KaraokeSkinLookup(ElementType.LyricFontInfo);
            var lyricFontInfo = source.GetConfig<KaraokeSkinLookup, LyricFontInfo>(lookup)?.Value;
            if (lyricFontInfo != null)
                Configs.Add(lyricFontInfo);

            LoadedLyricFontInfo.Value = Configs.FirstOrDefault();
            EditLyricFontInfo.Value = Configs.FirstOrDefault();
        }

        public void ApplyCurrentLyricFontInfoChange(Action<LyricFontInfo> action)
        {
            action?.Invoke(LoadedLyricFontInfo.Value);
            LoadedLyricFontInfo.TriggerChange();
        }
    }
}
