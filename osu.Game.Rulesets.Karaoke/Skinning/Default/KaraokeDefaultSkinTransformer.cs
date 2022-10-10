// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Beatmaps;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning.Default
{
    public class KaraokeDefaultSkinTransformer : SkinTransformer
    {
        private readonly KaraokeSkin karaokeSkin;

        public KaraokeDefaultSkinTransformer(ISkin skin, IBeatmap beatmap)
            : base(skin)
        {
            karaokeSkin = new KaraokeSkin(new SkinInfo(), new InternalSkinStorageResourceProvider("Default"));
        }

        public override IBindable<TValue>? GetConfig<TLookup, TValue>(TLookup lookup)
            => karaokeSkin.GetConfig<TLookup, TValue>(lookup);
    }
}
