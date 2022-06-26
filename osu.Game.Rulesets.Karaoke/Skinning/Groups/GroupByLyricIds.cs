// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Skinning.Groups
{
    public class GroupByLyricIds : BaseGroup<KaraokeHitObject>
    {
        public IReadOnlyList<int> LyricIds { get; set; } = Array.Empty<int>();

        protected override bool InTheGroup(KaraokeHitObject hitObject)
        {
            return hitObject switch
            {
                Lyric lyric => LyricIds.Contains(lyric.ID),
                Note note => LyricIds.Contains(note.ParentLyric.ID),
                _ => false
            };
        }
    }
}
