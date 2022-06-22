// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using System.ComponentModel;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    /// <summary>
    /// todo: it might be better just throw the whole <see cref="KaraokeHitObject"/> to get the config.
    /// because cannot get the result just by id.
    /// </summary>
    public readonly struct KaraokeSkinLookup
    {
        /// <summary>
        /// Parts wants to be searched.
        /// </summary>
        public ElementType Type { get; }

        /// <summary>
        /// Lookup index
        /// </summary>
        public int Lookup { get; }

        /// <summary>
        /// Ctor for <see cref="ElementType.LyricStyle"/> and <see cref="ElementType.NoteStyle"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="singers"></param>
        public KaraokeSkinLookup(ElementType type, IEnumerable<int> singers)
            : this(type, SingerUtils.GetShiftingStyleIndex(singers))
        {
            switch (type)
            {
                case ElementType.LyricStyle:
                case ElementType.LyricConfig:
                case ElementType.NoteStyle:
                    return;

                default:
                    throw new InvalidEnumArgumentException($"Cannot call lookup with {type}");
            }
        }

        public KaraokeSkinLookup(ElementType type, int lookup = -1)
        {
            Type = type;
            Lookup = lookup;
        }
    }
}
