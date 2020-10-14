// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class LegacyPropertyDictionary : KaraokeHitObject
    {
        public BeatmapSetOnlineLanguage[] AvailableTranslates { get; set; }

        public IDictionary<int, Singer> Singers { get; set; } = new Dictionary<int, Singer>();
    }
}
