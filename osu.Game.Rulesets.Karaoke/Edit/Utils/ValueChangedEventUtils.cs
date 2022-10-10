// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;

namespace osu.Game.Rulesets.Karaoke.Edit.Utils
{
    public static class ValueChangedEventUtils
    {
        public static bool LyricChanged(ValueChangedEvent<ICaretPosition?> e)
        {
            var oldLyric = e.OldValue?.Lyric;
            var newLyric = e.NewValue?.Lyric;

            return oldLyric != newLyric;
        }

        public static bool EditModeChanged(ValueChangedEvent<ModeWithSubMode> e)
        {
            if (e.OldValue.Default ^ e.NewValue.Default)
                return true;

            return e.OldValue.Mode != e.NewValue.Mode;
        }
    }
}
