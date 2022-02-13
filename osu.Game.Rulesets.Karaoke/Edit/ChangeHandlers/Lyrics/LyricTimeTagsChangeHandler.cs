// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricTimeTagsChangeHandler : HitObjectChangeHandler<Lyric>, ILyricTimeTagsChangeHandler
    {
        private TimeTagGeneratorSelector selector;

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetEditGeneratorConfigManager config)
        {
            selector = new TimeTagGeneratorSelector(config);
        }

        public void AutoGenerate()
        {
            PerformOnSelection(lyric =>
            {
                var timeTags = selector.GenerateTimeTags(lyric);
                lyric.TimeTags = timeTags ?? Array.Empty<TimeTag>();
            });
        }

        public void SetTimeTagTime(TimeTag timeTag, double time)
        {
            CheckExactlySelectedOneHitObject();

            PerformOnSelection(lyric =>
            {
                bool containsInLyric = lyric.TimeTags?.Contains(timeTag) ?? false;
                if (!containsInLyric)
                    throw new InvalidOperationException($"{nameof(timeTag)} is not in the lyric");

                timeTag.Time = time;
            });
        }

        public void ClearTimeTagTime(TimeTag timeTag)
        {
            CheckExactlySelectedOneHitObject();

            PerformOnSelection(lyric =>
            {
                bool containsInLyric = lyric.TimeTags?.Contains(timeTag) ?? false;
                if (!containsInLyric)
                    throw new InvalidOperationException($"{nameof(timeTag)} is not in the lyric");

                timeTag.Time = null;
            });
        }

        public void Add(TimeTag timeTag)
        {
            CheckExactlySelectedOneHitObject();

            PerformOnSelection(lyric =>
            {
                bool containsInLyric = lyric.TimeTags.Contains(timeTag);
                if (containsInLyric)
                    throw new InvalidOperationException($"{nameof(timeTag)} already in the lyric");

                lyric.TimeTags.Add(timeTag);
            });
        }

        public void Remove(TimeTag timeTag)
        {
            CheckExactlySelectedOneHitObject();

            PerformOnSelection(lyric =>
            {
                // delete time tag from list
                lyric.TimeTags.Remove(timeTag);
            });
        }

        public void AddByPosition(TextIndex index)
        {
            CheckExactlySelectedOneHitObject();

            PerformOnSelection(lyric =>
            {
                lyric.TimeTags.Add(new TimeTag(index));
            });
        }

        public void RemoveByPosition(TextIndex index)
        {
            CheckExactlySelectedOneHitObject();

            PerformOnSelection(lyric =>
            {
                var matchedTimeTags = lyric.TimeTags.Where(x => x.Index == index).ToList();
                if (!matchedTimeTags.Any())
                    return;

                var removedTimeTag = matchedTimeTags.OrderBy(x => x.Time).FirstOrDefault();
                lyric.TimeTags.Remove(removedTimeTag);
            });
        }
    }
}
