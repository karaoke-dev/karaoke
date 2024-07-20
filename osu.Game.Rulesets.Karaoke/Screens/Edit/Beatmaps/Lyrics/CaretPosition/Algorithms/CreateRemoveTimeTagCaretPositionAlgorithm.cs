// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

/// <summary>
/// Algorithm for able to create/remove the time-tag by lyric char index.
/// </summary>
public class CreateRemoveTimeTagCaretPositionAlgorithm : CharIndexCaretPositionAlgorithm<CreateRemoveTimeTagCaretPosition>
{
    public CreateRemoveTimeTagCaretPositionAlgorithm(Lyric[] lyrics)
        : base(lyrics)
    {
    }

    protected override CreateRemoveTimeTagCaretPosition CreateCaretPosition(Lyric lyric, int index) => new(lyric, index);
}
