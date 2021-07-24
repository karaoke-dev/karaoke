// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Rulesets.UI;
using osuTK.Graphics.ES30;

namespace osu.Game.Rulesets.Karaoke.UI
{
    /// <summary>
    /// Having a place to get all user customize font.
    /// todo : need to check will have better place or not.
    /// </summary>
    public class KaraokePlayfieldAdjustmentContainer : PlayfieldAdjustmentContainer
    {
        private const string base_path = "fonts\\cached";

        [Resolved]
        private FontStore fontStore { get; set; }

        private FontStore localFontStore;

        [BackgroundDependencyLoader]
        private void load(GameHost host, KaraokeRulesetConfigManager manager)
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

            var glyphStores = targetImportFonts
                              .Select(x => FontUsageUtils.ToFontInfo(x))
                              .Distinct()
                              .Select(host.CreateGlyphStore)
                              .Where(x => x != null)
                              .ToArray();

            if (!glyphStores.Any())
                return;

            // create local font store and import those files
            localFontStore = new FontStore(minFilterMode: All.Linear);
            fontStore.AddStore(localFontStore);

            foreach (var glyphStore in glyphStores)
            {
                localFontStore.AddStore(glyphStore);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            fontStore?.RemoveStore(localFontStore);
        }
    }
}
