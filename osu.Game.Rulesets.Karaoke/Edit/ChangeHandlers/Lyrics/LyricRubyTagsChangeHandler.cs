// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;

public partial class LyricRubyTagsChangeHandler : LyricPropertyChangeHandler, ILyricRubyTagsChangeHandler
{
    public void Add(RubyTag item)
    {
        CheckExactlySelectedOneHitObject();

        PerformOnSelection(lyric =>
        {
            bool containsInLyric = this.containsInLyric(lyric, item);
            if (containsInLyric)
                throw new InvalidOperationException($"{nameof(item)} already in the lyric");

            addToLyric(lyric, item);
        });
    }

    public void AddRange(IEnumerable<RubyTag> items)
    {
        CheckExactlySelectedOneHitObject();

        PerformOnSelection(lyric =>
        {
            // should convert to array because enumerable might change while deleting.
            foreach (var rubyTag in items.ToArray())
            {
                bool containsInLyric = this.containsInLyric(lyric, rubyTag);
                if (containsInLyric)
                    throw new InvalidOperationException($"{nameof(rubyTag)} already in the lyric");

                addToLyric(lyric, rubyTag);
            }
        });
    }

    public void Remove(RubyTag item)
    {
        CheckExactlySelectedOneHitObject();

        PerformOnSelection(lyric =>
        {
            bool containsInLyric = this.containsInLyric(lyric, item);
            if (!containsInLyric)
                throw new InvalidOperationException($"{nameof(item)} is not in the lyric");

            removeFromLyric(lyric, item);
        });
    }

    public void RemoveRange(IEnumerable<RubyTag> items)
    {
        CheckExactlySelectedOneHitObject();

        PerformOnSelection(lyric =>
        {
            // should convert to array because enumerable might change while deleting.
            foreach (var rubyTag in items.ToArray())
            {
                bool containsInLyric = this.containsInLyric(lyric, rubyTag);
                if (!containsInLyric)
                    throw new InvalidOperationException($"{nameof(rubyTag)} is not in the lyric");

                removeFromLyric(lyric, rubyTag);
            }
        });
    }

    public void SetIndex(RubyTag rubyTag, int? startIndex, int? endIndex)
    {
        CheckExactlySelectedOneHitObject();

        // note: it's ok not sort the text tag by index.
        PerformOnSelection(lyric =>
        {
            bool containsInLyric = this.containsInLyric(lyric, rubyTag);
            if (!containsInLyric)
                throw new InvalidOperationException($"{nameof(rubyTag)} is not in the lyric");

            if (startIndex != null)
                rubyTag.StartIndex = startIndex.Value;

            if (endIndex != null)
                rubyTag.EndIndex = endIndex.Value;

            // after change the index, should check if index is valid.
            if (TextTagUtils.OutOfRange(rubyTag, lyric.Text))
                throw new InvalidOperationException($"{nameof(startIndex)} or {nameof(endIndex)} is not valid");
        });
    }

    public void ShiftingIndex(IEnumerable<RubyTag> rubyTags, int offset)
    {
        CheckExactlySelectedOneHitObject();

        // note: it's ok not sort the text tag by index.
        PerformOnSelection(lyric =>
        {
            foreach (var rubyTag in rubyTags)
            {
                bool containsInLyric = this.containsInLyric(lyric, rubyTag);
                if (!containsInLyric)
                    throw new InvalidOperationException($"{nameof(rubyTag)} is not in the lyric");

                (int startIndex, int endIndex) = TextTagUtils.GetShiftingIndex(rubyTag, lyric.Text, offset);
                rubyTag.StartIndex = startIndex;
                rubyTag.EndIndex = endIndex;
            }
        });
    }

    public void SetText(RubyTag rubyTag, string text)
    {
        CheckExactlySelectedOneHitObject();

        PerformOnSelection(lyric =>
        {
            bool containsInLyric = this.containsInLyric(lyric, rubyTag);
            if (!containsInLyric)
                throw new InvalidOperationException($"{nameof(rubyTag)} is not in the lyric");

            rubyTag.Text = text;
        });
    }

    private bool containsInLyric(Lyric lyric, RubyTag rubyTag)
        => lyric.RubyTags.Contains(rubyTag);

    private void addToLyric(Lyric lyric, RubyTag rubyTag)
        => lyric.RubyTags.Add(rubyTag);

    private void removeFromLyric(Lyric lyric, RubyTag rubyTag)
        => lyric.RubyTags.Remove(rubyTag);

    protected override bool IsWritePropertyLocked(Lyric lyric)
        => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.RubyTags));
}
