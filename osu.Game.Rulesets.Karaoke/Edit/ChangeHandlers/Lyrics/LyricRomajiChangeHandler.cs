// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricRomajiChangeHandler : HitObjectChangeHandler<Lyric>, ILyricRomajiChangeHandler
    {
        public void AutoGenerate()
        {
            var selector = new RomajiTagGeneratorSelector();
            PerformOnSelection(lyric =>
            {
                var romajiTags = selector.GenerateRomajiTags(lyric);
                lyric.RomajiTags = romajiTags;
            });
        }

        public bool CanGenerate()
        {
            var selector = new RomajiTagGeneratorSelector();
            return HitObjects.Any(lyric => selector.CanGenerate(lyric));
        }
    }
}
