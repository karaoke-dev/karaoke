// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using osu.Game.IO;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class DefaultKaraokeSkin : KaraokeSkin
    {
        public DefaultKaraokeSkin(SkinInfo skin, IStorageResourceProvider resources, Stream configurationStream = null)
            : base(skin, resources, configurationStream)
        {
        }
    }
}
