// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.ComponentModel;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public enum LyricEditorMode
    {
        /// <summary>
        /// Cannot edit anything except each lyric's left-side part.
        /// </summary>
        [Description("View")]
        View,

        /// <summary>
        /// Can create/delete/mode/split/combine lyric.
        /// </summary>
        [Description("Manage lyrics")]
        Manage,

        /// <summary>
        /// Able to typing lyric.
        /// </summary>
        [Description("Typing")]
        Typing,

        /// <summary>
        /// Can edit each lyric's language.
        /// </summary>
        [Description("Select language")]
        Language,

        /// <summary>
        /// Able to create/delete ruby.
        /// </summary>
        [Description("Edit ruby")]
        EditRuby,

        /// <summary>
        /// Able to create/delete romaji.
        /// </summary>
        [Description("Edit romaji")]
        EditRomaji,

        /// <summary>
        /// Enable to create/delete and reset time tag.
        /// </summary>
        [Description("Edit time tag")]
        EditTimeTag,

        /// <summary>
        /// Enable to create/delete notes.
        /// </summary>
        [Description("Edit note")]
        EditNote,

        /// <summary>
        /// Can edit each lyric's singer.
        /// </summary>
        [Description("Select singer")]
        Singer,
    }
}
