// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricTimeTagsChangeHandler : HitObjectChangeHandler<Lyric>, ILyricTimeTagsChangeHandler
    {
        public void AutoGenerate()
        {
            var selector = new TimeTagGeneratorSelector();

            PerformOnSelection(lyric =>
            {
                var timeTags = selector.GenerateTimeTags(lyric);
                lyric.TimeTags = timeTags;
            });
        }

        public void SetTimeTagTime(TimeTag timeTag, double time)
        {
            PerformOnSelection(lyric =>
            {
                bool containsInLyric = lyric.TimeTags?.Contains(timeTag) ?? false;
                if (containsInLyric == false)
                    throw new InvalidOperationException($"{nameof(timeTag)} is not in the lyric");

                timeTag.Time = time;
                refreshTimeTag(lyric);
            });
        }

        public void ClearTimeTagTime(TimeTag timeTag)
        {
            PerformOnSelection(lyric =>
            {
                bool containsInLyric = lyric.TimeTags?.Contains(timeTag) ?? false;
                if (containsInLyric == false)
                    throw new InvalidOperationException($"{nameof(timeTag)} is not in the lyric");

                timeTag.Time = null;
                refreshTimeTag(lyric);
            });
        }

        private void refreshTimeTag(Lyric lyric)
            => lyric.TimeTags = lyric.TimeTags.ToArray();

        public void Add(TimeTag timeTag)
        {
            PerformOnSelection(lyric =>
            {
                bool containsInLyric = lyric.TimeTags?.Contains(timeTag) ?? false;
                if (containsInLyric == false)
                    throw new InvalidOperationException($"{nameof(timeTag)} is not in the lyric");

                var timeTags = lyric.TimeTags.ToList();
                int targetIndex = timeTags.IndexOf(timeTag);
                if (targetIndex < 0)
                    throw new InvalidOperationException($"{nameof(timeTag)} is not in the lyric");

                lyric.TimeTags = timeTags.ToArray();
                sortingTimeTag(lyric);
            });

            static void sortingTimeTag(Lyric lyric)
                => lyric.TimeTags = TimeTagsUtils.Sort(lyric.TimeTags);
        }

        public void Remove(TimeTag timeTag)
        {
            PerformOnSelection(lyric =>
            {
                // delete time tag from list
                lyric.TimeTags = lyric.TimeTags.Where(x => x != timeTag).ToArray();
            });
        }

        public void AddByPosition(TextIndex index)
        {
            PerformOnSelection(lyric =>
            {
                var timeTags = lyric.TimeTags.ToList();
                var targetTimeTag = timeTags.FirstOrDefault(x => x.Index >= index) ?? timeTags.LastOrDefault();
                if (targetTimeTag == null)
                    throw new InvalidOperationException($"{nameof(targetTimeTag)} is not in the lyric");

                int insertIndex = timeTags.IndexOf(targetTimeTag);
                timeTags.Insert(insertIndex, new TimeTag(index));
                lyric.TimeTags = timeTags.ToArray();
            });
        }

        public void RemoveByPosition(TextIndex index)
        {
            PerformOnSelection(lyric =>
            {
                var timeTags = lyric.TimeTags.ToList();
                var targetTimeTag = timeTags.FirstOrDefault(x => x.Index == index);
                if (targetTimeTag == null)
                    throw new InvalidOperationException($"{nameof(targetTimeTag)} is not in the lyric");

                timeTags.Remove(targetTimeTag);
                lyric.TimeTags = timeTags.ToArray();
            });
        }
    }
}
