// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics.Carets
{
    public interface IHasCaretPosition
    {
        ICaretPosition CaretPosition { get; set; }
    }
}
