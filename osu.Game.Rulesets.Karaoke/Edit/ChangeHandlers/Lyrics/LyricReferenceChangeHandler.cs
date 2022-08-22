// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricReferenceChangeHandler : HitObjectChangeHandler<Lyric>, ILyricReferenceChangeHandler
    {
        public void UpdateReferenceLyric(Lyric? referenceLyric)
        {
            if (referenceLyric != null && !HitObjects.Contains(referenceLyric))
                throw new InvalidOperationException($"{nameof(referenceLyric)} should in the beatmap.");

            PerformOnSelection(lyric =>
            {
                if (referenceLyric == lyric)
                    throw new InvalidOperationException($"{nameof(referenceLyric)} should not be the same instance as {nameof(lyric)}");

                if (referenceLyric?.ReferenceLyric != null)
                    throw new InvalidOperationException($"{nameof(referenceLyric)} should not contains another reference lyric.");

                lyric.ReferenceLyric = referenceLyric;
            });
        }
    }
}
