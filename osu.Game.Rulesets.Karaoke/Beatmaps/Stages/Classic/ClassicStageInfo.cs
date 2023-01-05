// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;

public class ClassicStageInfo : StageInfo
{
    #region Category

    /// <summary>
    /// Category to save the <see cref="Lyric"/>'s and <see cref="Note"/>'s style.
    /// </summary>
    public ClassicStyleCategory StyleCategory { get; set; } = new();

    /// <summary>
    /// The definition for the <see cref="Lyric"/>.
    /// Like the line height or font size.
    /// </summary>
    public ClassicLyricLayoutDefinition LyricLayoutDefinition { get; set; } = new();

    /// <summary>
    /// Category to save the <see cref="Lyric"/>'s layout.
    /// </summary>
    public ClassicLyricLayoutCategory LyricLayoutCategory { get; set; } = new();

    #endregion

    #region Stage element

    protected override IEnumerable<IStageElement> GetLyricStageElements(Lyric lyric)
    {
        yield return StyleCategory.GetElementByItem(lyric);
        yield return LyricLayoutCategory.GetElementByItem(lyric);
    }

    protected override IEnumerable<IStageElement> GetNoteStageElements(Note note)
    {
        // todo: should check the real-time mapping result.
        yield return StyleCategory.GetElementByItem(note.ReferenceLyric!);
    }

    protected override IEnumerable<object> ConvertToLyricStageAppliers(IEnumerable<IStageElement> elements)
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerable<object> ConvertToNoteStageAppliers(IEnumerable<IStageElement> elements)
    {
        throw new System.NotImplementedException();
    }

    #endregion
}
