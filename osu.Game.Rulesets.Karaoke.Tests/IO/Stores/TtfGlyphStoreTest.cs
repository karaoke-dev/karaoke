// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.IO.Stores;
using osu.Game.Rulesets.Karaoke.IO.Stores;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Stores
{
    [Ignore("This shit is not implemented.")]
    public class TtfGlyphStoreTest : BaseGlyphStoreTest<TtfGlyphStore>
    {
        protected override string FontType => "Ttf";

        protected override string FontName => "OpenSans-Regular";

        protected override TtfGlyphStore CreateFontStore(ResourceStore<byte[]> store, string assetName)
            => new(store, assetName);
    }
}
