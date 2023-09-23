// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Edit.Utils;

public static class ValueChangedEventUtils
{
    public static bool LyricChanged(ValueChangedEvent<ICaretPosition?> e)
    {
        var oldLyric = e.OldValue?.Lyric;
        var newLyric = e.NewValue?.Lyric;

        return oldLyric != newLyric;
    }

    public static bool LyricChanged(ValueChangedEvent<RangeCaretPosition?> e)
    {
        var oldRangeCaret = e.OldValue;
        var newRangeCaret = e.NewValue;

        return oldRangeCaret?.Start.Lyric != newRangeCaret?.Start.Lyric
               || oldRangeCaret?.End.Lyric != newRangeCaret?.End.Lyric;
    }

    public static bool EditModeChanged(ValueChangedEvent<EditorModeWithEditStep> e)
    {
        if (e.OldValue.Default ^ e.NewValue.Default)
            return true;

        return e.OldValue.Mode != e.NewValue.Mode;
    }
}
