// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricRubyChangeHandler : HitObjectChangeHandler<Lyric>, ILyricRubyChangeHandler
    {
        public void AutoGenerate()
        {
            var selector = new RubyTagGeneratorSelector();
            PerformOnSelection(lyric =>
            {
                var rubyTags = selector.GenerateRubyTags(lyric);
                lyric.RubyTags = rubyTags;
            });
        }

        public bool CanGenerate()
        {
            var selector = new RubyTagGeneratorSelector();
            return HitObjects.Any(lyric => selector.CanGenerate(lyric));
        }
    }
}
