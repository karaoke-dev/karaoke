// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricTimeTagsChangeHandler : HitObjectChangeHandler<Lyric>, ILyricTimeTagsChangeHandler
    {
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

                insertTimeTag(lyric, timeTag, InsertDirection.End);
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
                insertTimeTag(lyric, new TimeTag(index), InsertDirection.End);
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

        public TimeTag Shifting(TimeTag timeTag, ShiftingDirection direction, ShiftingType type)
        {
            CheckExactlySelectedOneHitObject();

            TimeTag newTimeTag = null;

            PerformOnSelection(lyric =>
            {
                bool containsInLyric = lyric.TimeTags?.Contains(timeTag) ?? false;
                if (!containsInLyric)
                    throw new InvalidOperationException($"{nameof(timeTag)} is not in the lyric");

                // remove the time-tag first.
                lyric.TimeTags.Remove(timeTag);

                // then, create a new one and insert into the list.
                var newIndex = calculateNewIndex(lyric, timeTag.Index, direction, type);
                double? newTime = timeTag.Time;
                newTimeTag = new TimeTag(newIndex, newTime);

                switch (direction)
                {
                    case ShiftingDirection.Left:
                        insertTimeTag(lyric, newTimeTag, InsertDirection.End);
                        break;

                    case ShiftingDirection.Right:
                        insertTimeTag(lyric, newTimeTag, InsertDirection.Start);
                        break;

                    default:
                        throw new InvalidOperationException();
                }
            });

            return newTimeTag;

            static TextIndex calculateNewIndex(Lyric lyric, TextIndex originIndex, ShiftingDirection direction, ShiftingType type)
            {
                var newIndex = getNewIndex(originIndex, direction, type);
                if (TextIndexUtils.OutOfRange(newIndex, lyric.Text))
                    throw new ArgumentOutOfRangeException();

                return newIndex;

                static TextIndex getNewIndex(TextIndex originIndex, ShiftingDirection direction, ShiftingType type) =>
                    type switch
                    {
                        ShiftingType.Index => TextIndexUtils.ShiftingIndex(originIndex, direction == ShiftingDirection.Left ? -1 : 1),
                        ShiftingType.State => direction == ShiftingDirection.Left ? TextIndexUtils.GetPreviousIndex(originIndex) : TextIndexUtils.GetNextIndex(originIndex),
                        _ => throw new InvalidOperationException()
                    };
            }
        }

        private void insertTimeTag(Lyric lyric, TimeTag timeTag, InsertDirection direction)
        {
            var timeTags = lyric.TimeTags;

            // just add if there's no time-tag
            if (lyric.TimeTags.Count == 0)
            {
                timeTags.Add(timeTag);
                return;
            }

            if (timeTags.All(x => x.Index < timeTag.Index))
            {
                timeTags.Add(timeTag);
            }
            else if (timeTags.All(x => x.Index > timeTag.Index))
            {
                timeTags.Insert(0, timeTag);
            }
            else
            {
                switch (direction)
                {
                    case InsertDirection.Start:
                    {
                        var nextTimeTag = timeTags.FirstOrDefault(x => x.Index >= timeTag.Index) ?? timeTags.Last();
                        int index = timeTags.IndexOf(nextTimeTag);
                        timeTags.Insert(index, timeTag);
                        break;
                    }

                    case InsertDirection.End:
                    {
                        var previousTextTag = timeTags.Reverse().FirstOrDefault(x => x.Index <= timeTag.Index) ?? timeTags.First();
                        int index = timeTags.IndexOf(previousTextTag) + 1;
                        timeTags.Insert(index, timeTag);
                        break;
                    }

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Insert direction if contains the time-tag with the same index.
        /// </summary>
        private enum InsertDirection
        {
            Start,

            End
        }
    }
}
