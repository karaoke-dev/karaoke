// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Utils;
using System.IO;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public readonly struct KaraokeSkinLookup
    {
        /// <summary>
        /// Ctor for <see cref="KaraokeSkinConfiguration.LyricStyle"/> and <see cref="KaraokeSkinConfiguration.NoteStyle"/>
        /// </summary>
        /// <param name="config"></param>
        /// <param name="singers"></param>
        public KaraokeSkinLookup(KaraokeSkinConfiguration config, int[] singers)
            : this(config, SingerUtils.GetShiftingStyleIndex(singers))
        {
            if (config != KaraokeSkinConfiguration.LyricStyle && config != KaraokeSkinConfiguration.NoteStyle)
                throw new InvalidDataException($"Only {KaraokeSkinConfiguration.LyricStyle} and {KaraokeSkinConfiguration.NoteStyle} can call this ctor.");
        }

        public KaraokeSkinLookup(KaraokeSkinConfiguration config, int lookup)
        {
            Config = config;
            Lookup = lookup;
            Editor = false;
        }

        public KaraokeSkinConfiguration Config { get; }
        public int Lookup { get; }
        public bool Editor { get; }
    }
}
