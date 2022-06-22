// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.IO.Stores;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.IO.Stores;
using osu.Game.Rulesets.Karaoke.Skinning.Fonts;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Karaoke.UI
{
    /// <summary>
    /// Having a place to get all user customize font.
    /// todo : need to check will have better place or not.
    /// </summary>
    public class KaraokePlayfieldAdjustmentContainer : PlayfieldAdjustmentContainer
    {
        [Resolved]
        private FontStore fontStore { get; set; }

        private KaraokeLocalFontStore localFontStore;

        [BackgroundDependencyLoader]
        private void load(FontManager fontManager, KaraokeRulesetConfigManager manager)
        {
            // get all font usage which wants to import.
            var targetImportFonts = new[]
            {
                manager.Get<FontUsage>(KaraokeRulesetSetting.MainFont),
                manager.Get<FontUsage>(KaraokeRulesetSetting.RubyFont),
                manager.Get<FontUsage>(KaraokeRulesetSetting.RomajiFont),
                manager.Get<FontUsage>(KaraokeRulesetSetting.TranslateFont),
                manager.Get<FontUsage>(KaraokeRulesetSetting.NoteFont),
            };

            var fontInfos = targetImportFonts
                            .Distinct()
                            .ToArray();

            if (!fontInfos.Any())
                return;

            // create local font store and import those files
            localFontStore = new KaraokeLocalFontStore(fontManager);
            fontStore.AddStore(localFontStore);

            foreach (var fontInfo in fontInfos)
            {
                localFontStore.AddFont(fontInfo);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            fontStore?.RemoveStore(localFontStore);
        }
    }
}
