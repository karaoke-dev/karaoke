// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricRomajiTagsChangeHandler : LyricTextTagsChangeHandler<RomajiTag>, ILyricRomajiTagsChangeHandler
    {
        private RomajiTagGeneratorSelector selector;

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetEditGeneratorConfigManager config)
        {
            selector = new RomajiTagGeneratorSelector(config);
        }

        public void AutoGenerate()
        {
            PerformOnSelection(lyric =>
            {
                var romajiTags = selector.Generate(lyric);
                lyric.RomajiTags = romajiTags ?? Array.Empty<RomajiTag>();
            });
        }

        public bool CanGenerate()
            => HitObjects.Any(lyric => selector.CanGenerate(lyric));

        protected override bool ContainsInLyric(Lyric lyric, RomajiTag textTag)
            => lyric.RomajiTags.Contains(textTag);

        protected override void AddToLyric(Lyric lyric, RomajiTag textTag)
            => lyric.RomajiTags.Add(textTag);

        protected override void RemoveFromLyric(Lyric lyric, RomajiTag textTag)
            => lyric.RomajiTags.Remove(textTag);
    }
}
