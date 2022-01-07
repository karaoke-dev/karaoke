// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricRubyTagsChangeHandler : LyricTextTagsChangeHandler<RubyTag>, ILyricRubyTagsChangeHandler
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

        protected override bool ContainsInLyric(Lyric lyric, RubyTag textTag)
            => lyric.RubyTags.Contains(textTag);

        protected override void AddToLyric(Lyric lyric, RubyTag textTag)
        {
            var rubyTags = lyric.RubyTags.ToList();
            rubyTags.Add(textTag);

            lyric.RubyTags = TextTagsUtils.Sort(rubyTags);
        }

        protected override void RemoveFromLyric(Lyric lyric, RubyTag textTag)
        {
            var rubyTags = lyric.RubyTags.ToList();
            rubyTags.Remove(textTag);

            lyric.RubyTags = rubyTags.ToArray();
        }
    }
}
