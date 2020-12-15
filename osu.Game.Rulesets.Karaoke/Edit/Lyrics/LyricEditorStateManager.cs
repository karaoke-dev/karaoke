// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class LyricEditorStateManager
    {
        public Bindable<Lyric> BindableSplitLyric { get; } = new Bindable<Lyric>();
        public Bindable<int> BindableSplitPosition { get; } = new Bindable<int>();

        public void UpdateSplitCursorPosition(Lyric lyric, int index)
        {
            BindableSplitLyric.Value = lyric;
            BindableSplitPosition.Value = index;
        }

        public void ClearSplitCursorPosition()
        {
            BindableSplitLyric.Value = null;
        }
    }

    public enum Mode
    {
        /// <summary>
        /// Cannot edit anything except each lyric's left-side part.
        /// </summary>
        ViewMode,

        /// <summary>
        /// Can create/delete/mode/split/combine lyric.
        /// </summary>
        EditMode,

        /// <summary>
        /// Click white-space to set current time into time-tag.
        /// </summary>
        RecordMode,

        /// <summary>
        /// Enable to create/delete and reset time tag.
        /// </summary>
        TimeTagEditMode
    }

    public enum LyricFastEditMode
    {
        /// <summary>
        /// User can only see start and end time.
        /// </summary>
        None,

        /// <summary>
        /// Can edit each lyric's layout.
        /// </summary>
        Layout,

        /// <summary>
        /// Can edit each lyric's singer.
        /// </summary>
        Singer,

        /// <summary>
        /// Can edit each lyric's language.
        /// </summary>
        Language,
    }
}
