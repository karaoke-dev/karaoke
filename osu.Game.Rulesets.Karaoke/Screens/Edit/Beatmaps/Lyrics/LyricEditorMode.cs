// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
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
    [Description("Texting")]
    Texting,

    /// <summary>
    /// Mark the lyric is "similar" to another lyric.
    /// </summary>
    [Description("Reference")]
    Reference,

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
    /// Enable to create/delete and reset time tag.
    /// </summary>
    [Description("Edit time tag")]
    EditTimeTag,

    /// <summary>
    /// Able to edit the romaji from the time-tag.
    /// </summary>
    [Description("Edit romaji")]
    EditRomaji,

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
