// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

/// <summary>
/// Algorithm for navigate to the <see cref="TextIndex"/> position inside the <see cref="TimeTag"/>.
/// </summary>
public class TimeTagIndexCaretPositionAlgorithm : CharIndexCaretPositionAlgorithm<TimeTagIndexCaretPosition>
{
    public TimeTagIndexCaretPositionAlgorithm(Lyric[] lyrics)
        : base(lyrics)
    {
    }

    protected override TimeTagIndexCaretPosition CreateCaretPosition(Lyric lyric, int index) => new(lyric, index);
}
