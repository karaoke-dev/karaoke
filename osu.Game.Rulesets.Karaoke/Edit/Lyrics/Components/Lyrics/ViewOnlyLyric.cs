// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics
{
    public class ViewOnlyLyric : InteractableLyric
    {
        public ViewOnlyLyric(Lyric lyric)
            : base(lyric)
        {
        }

        protected override IEnumerable<BaseLayer> CreateLayers(Lyric lyric)
        {
            return new BaseLayer[]
            {
                new TimeTagLayer(lyric),
            };
        }
    }
}
