// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

public readonly struct CreateRemoveTimeTagCaretPosition : ICharIndexCaretPosition, IComparable<CreateRemoveTimeTagCaretPosition>
{
    public CreateRemoveTimeTagCaretPosition(Lyric lyric, int charIndex)
    {
        Lyric = lyric;
        CharIndex = charIndex;
    }

    public Lyric Lyric { get; }

    public int CharIndex { get; }

    public TimeTag[] GetTimeTagsWithState(TextIndex.IndexState state)
    {
        int charIndex = CharIndex;
        return Lyric.TimeTags.Where(x => TextIndexUtils.ToCharIndex(x.Index) == charIndex && x.Index.State == state).ToArray();
    }

    public int CompareTo(CreateRemoveTimeTagCaretPosition other)
    {
        if (Lyric != other.Lyric)
            throw new InvalidOperationException();

        return CharIndex.CompareTo(other.CharIndex);
    }

    public int CompareTo(IIndexCaretPosition? other)
    {
        if (other is not CreateRemoveTimeTagCaretPosition timeTagIndexCaretPosition)
            throw new InvalidOperationException();

        return CompareTo(timeTagIndexCaretPosition);
    }
}
