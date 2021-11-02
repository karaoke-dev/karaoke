// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public enum LyricEditorMode
    {
        /// <summary>
        /// Cannot edit anything except each lyric's left-side part.
        /// </summary>
        View,

        /// <summary>
        /// Can create/delete/mode/split/combine lyric.
        /// </summary>
        Manage,

        /// <summary>
        /// Able to typing lyric.
        /// </summary>
        Typing,

        /// <summary>
        /// Can edit each lyric's language.
        /// </summary>
        Language,

        /// <summary>
        /// Able to create/delete ruby.
        /// </summary>
        EditRuby,

        /// <summary>
        /// Able to create/delete romaji.
        /// </summary>
        EditRomaji,

        /// <summary>
        /// Enable to create/delete and reset time tag.
        /// </summary>
        CreateTimeTag,

        /// <summary>
        /// Click white-space to set current time into time-tag.
        /// </summary>
        RecordTimeTag,

        /// <summary>
        /// Precisely adjust time-tag time.
        /// </summary>
        AdjustTimeTag,

        /// <summary>
        /// Enable to create/delete notes.
        /// </summary>
        EditNote,

        /// <summary>
        /// Can edit each lyric's layout.
        /// </summary>
        Layout,

        /// <summary>
        /// Can edit each lyric's singer.
        /// </summary>
        Singer,
    }
}
