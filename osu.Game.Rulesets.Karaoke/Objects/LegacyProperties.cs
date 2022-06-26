// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    // todo: this function is used for legacy karaoke beatmap, will be removed eventually.
    public class LegacyProperties : KaraokeHitObject
    {
        public IList<CultureInfo> AvailableTranslates { get; set; } = new List<CultureInfo>();

        public IDictionary<int, Singer> Singers { get; set; } = new Dictionary<int, Singer>();
    }
}
