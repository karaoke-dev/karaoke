// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.IO.Archives;
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
            var storage = host.Storage;
            if (!storage.ExistsDirectory(base_path))
                return;

            // get all font usage which wants to import.
            var targetImportFonts = new[]
            {
                manager.Get<FontUsage>(KaraokeRulesetSetting.MainFont),
                manager.Get<FontUsage>(KaraokeRulesetSetting.RubyFont),
                manager.Get<FontUsage>(KaraokeRulesetSetting.RomajiFont),
                manager.Get<FontUsage>(KaraokeRulesetSetting.TranslateFont),
                manager.Get<FontUsage>(KaraokeRulesetSetting.NoteFont),
            };

            // convert to file path then import
            var targetImportFontPaths = targetImportFonts.Select(x =>
            {
                var path = Path.Combine(base_path, x.FontName);
                var pathWithExtension = Path.ChangeExtension(path, "cached");
                return pathWithExtension;
            }).Where(p => storage.Exists(p)).Distinct().ToArray();
            if (!targetImportFontPaths.Any())
                return;

            // create font store if wants to import.
            localFontStore = new FontStore(minFilterMode: All.Linear);
            fontStore.AddStore(localFontStore);

            foreach (var path in targetImportFontPaths)
            {
                var fontName = Path.GetFileNameWithoutExtension(path);
                var resources = new CachedFontArchiveReader(storage.GetStream(path), fontName);
                var store = new GlyphStore(new ResourceStore<byte[]>(resources), $"{fontName}", host.CreateTextureLoaderStore(resources));
                localFontStore.AddStore(store);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            fontStore?.RemoveStore(localFontStore);
        }
    }
}
