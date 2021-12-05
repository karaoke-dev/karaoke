// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricLayoutChangeHandler : HitObjectChangeHandler<Lyric>, ILyricLayoutChangeHandler
    {
        public void ChangeLayout(LyricLayout layout)
        {
            PerformOnSelection(lyric =>
            {
                LyricUtils.AssignLayout(lyric, layout);
            });
        }
    }
}
