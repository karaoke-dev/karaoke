// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Skinning.Components;
using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class LegacyPropertyDictionary : KaraokeHitObject
    {
        public IDictionary<string, List<string>> Translates { get; } = new Dictionary<string, List<string>>();

        public IDictionary<int, Singer> Singers { get; set; } = new Dictionary<int, Singer>();
    }
}
