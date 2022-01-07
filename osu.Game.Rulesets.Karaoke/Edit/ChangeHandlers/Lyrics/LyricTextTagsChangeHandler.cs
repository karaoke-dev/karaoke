// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public abstract class LyricTextTagsChangeHandler<TTextTag> : HitObjectChangeHandler<Lyric>, ILyricTextTagsChangeHandler<TTextTag> where TTextTag : ITextTag
    {
        public void Add(TTextTag textTag)
        {
            PerformOnSelection(lyric =>
            {
                bool containsInLyric = ContainsInLyric(lyric, textTag);
                if (containsInLyric == false)
                    throw new InvalidOperationException($"{nameof(textTag)} is not in the lyric");

                AddToLyric(lyric, textTag);
            });
        }

        public void Remove(TTextTag textTag)
        {
            PerformOnSelection(lyric =>
            {
                bool containsInLyric = ContainsInLyric(lyric, textTag);
                if (containsInLyric == false)
                    throw new InvalidOperationException($"{nameof(textTag)} is not in the lyric");

                RemoveFromLyric(lyric, textTag);
            });
        }

        public void SetPosition(TTextTag textTag, int? startIndex, int? endIndex)
        {
            PerformOnSelection(lyric =>
            {
                bool containsInLyric = ContainsInLyric(lyric, textTag);
                if (containsInLyric == false)
                    throw new InvalidOperationException($"{nameof(textTag)} is not in the lyric");

                if (startIndex != null)
                    textTag.StartIndex = startIndex.Value;

                if (endIndex != null)
                    textTag.EndIndex = endIndex.Value;
            });
        }

        public void SetText(TTextTag textTag, string text)
        {
            PerformOnSelection(lyric =>
            {
                bool containsInLyric = ContainsInLyric(lyric, textTag);
                if (containsInLyric == false)
                    throw new InvalidOperationException($"{nameof(textTag)} is not in the lyric");

                textTag.Text = text;
            });
        }

        protected abstract bool ContainsInLyric(Lyric lyric, TTextTag textTag);

        protected abstract void AddToLyric(Lyric lyric, TTextTag textTag);

        protected abstract void RemoveFromLyric(Lyric lyric, TTextTag textTag);
    }
}
