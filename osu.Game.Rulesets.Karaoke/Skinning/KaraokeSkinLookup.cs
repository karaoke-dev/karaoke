// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public readonly struct KaraokeSkinLookup
    {
        /// <summary>
        /// Parts wants to be searched.
        /// </summary>
        public KaraokeSkinConfiguration Config { get; }

        /// <summary>
        /// Lookup index
        /// </summary>
        public int Lookup { get; }

        /// <summary>
        /// Ctor for <see cref="KaraokeSkinConfiguration.LyricStyle"/> and <see cref="KaraokeSkinConfiguration.NoteStyle"/>
        /// </summary>
        /// <param name="config"></param>
        /// <param name="singers"></param>
        public KaraokeSkinLookup(KaraokeSkinConfiguration config, int[] singers)
            : this(config, SingerUtils.GetShiftingStyleIndex(singers))
        {
            if (config != KaraokeSkinConfiguration.LyricStyle && config != KaraokeSkinConfiguration.NoteStyle)
                throw new InvalidEnumArgumentException($"Only {KaraokeSkinConfiguration.LyricStyle} and {KaraokeSkinConfiguration.NoteStyle} can call this ctor.");
        }

        public KaraokeSkinLookup(KaraokeSkinConfiguration config, int lookup)
        {
            Config = config;
            Lookup = lookup;
        }
    }
}
