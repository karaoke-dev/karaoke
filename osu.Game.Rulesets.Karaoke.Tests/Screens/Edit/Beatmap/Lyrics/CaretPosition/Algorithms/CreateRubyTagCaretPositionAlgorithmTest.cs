// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.CaretPosition.Algorithms;

[TestFixture]
public class CreateRubyTagCaretPositionAlgorithmTest : BaseCharIndexCaretPositionAlgorithmTest<CreateRubyTagCaretPositionAlgorithm, CreateRubyTagCaretPosition>
{
    protected override CreateRubyTagCaretPosition CreateCaret(Lyric lyric, int index)
        => new(lyric, index);
}
