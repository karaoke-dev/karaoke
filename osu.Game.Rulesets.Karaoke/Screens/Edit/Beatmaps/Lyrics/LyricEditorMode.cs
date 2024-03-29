﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

public enum LyricEditorMode
{
    /// <summary>
    /// Cannot edit anything except each lyric's left-side part.
    /// </summary>
    [Description("View")]
    View,

    /// <summary>
    /// Can create/delete/move/split/combine lyric.
    /// And typing the lyric.
    /// </summary>
    [Description("Edit Text")]
    EditText,

    /// <summary>
    /// Mark the lyric is "similar" to another lyric.
    /// </summary>
    [Description("Edit Reference lyric")]
    EditReferenceLyric,

    /// <summary>
    /// Can edit each lyric's language.
    /// </summary>
    [Description("Select language")]
    EditLanguage,

    /// <summary>
    /// Able to create/delete ruby.
    /// </summary>
    [Description("Edit ruby")]
    EditRuby,

    /// <summary>
    /// Enable to create/delete and reset time tag.
    /// </summary>
    [Description("Edit time tag")]
    EditTimeTag,

    /// <summary>
    /// Able to edit the romanisation from the time-tag.
    /// </summary>
    [Description("Edit Romanisation")]
    EditRomanisation,

    /// <summary>
    /// Enable to create/delete notes.
    /// </summary>
    [Description("Edit note")]
    EditNote,

    /// <summary>
    /// Can edit each lyric's singer.
    /// </summary>
    [Description("Select singer")]
    EditSinger,
}
