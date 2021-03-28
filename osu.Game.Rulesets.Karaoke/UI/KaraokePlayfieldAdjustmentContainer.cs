// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using osu.Framework.Allocation;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Game.Rulesets.Karaoke.IO.Archives;
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

        private FontStore localFontStore;

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            var storage = host.Storage;
            if (!storage.ExistsDirectory($"fonts/cached"))
                return;

            // create font store if wants to import.
            localFontStore = new FontStore(scaleAdjust: 200, minFilterMode: osuTK.Graphics.ES30.All.Linear);
            fontStore.AddStore(localFontStore);

            var files = storage.GetFiles($"fonts/cached");
            foreach (var file in files)
            {
                // should only accept .cached extension.
                if (Path.GetExtension(file) != ".cached")
                    return;

                var fontName = Path.GetFileNameWithoutExtension(file);
                var resources = new CachedFontArchiveReader(storage.GetStream(file), fontName);
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
