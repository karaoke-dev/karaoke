// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricRubyTagsChangeHandler : LyricTextTagsChangeHandler<RubyTag>, ILyricRubyTagsChangeHandler
    {
        private RubyTagGeneratorSelector selector;

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetEditGeneratorConfigManager config)
        {
            selector = new RubyTagGeneratorSelector(config);
        }

        public void AutoGenerate()
        {
            PerformOnSelection(lyric =>
            {
                var rubyTags = selector.Generate(lyric);
                lyric.RubyTags = rubyTags ?? Array.Empty<RubyTag>();
            });
        }

        public bool CanGenerate()
            => HitObjects.Any(lyric => selector.CanGenerate(lyric));

        protected override bool ContainsInLyric(Lyric lyric, RubyTag textTag)
            => lyric.RubyTags.Contains(textTag);

        protected override void AddToLyric(Lyric lyric, RubyTag textTag)
            => lyric.RubyTags.Add(textTag);

        protected override void RemoveFromLyric(Lyric lyric, RubyTag textTag)
            => lyric.RubyTags.Remove(textTag);
    }
}
