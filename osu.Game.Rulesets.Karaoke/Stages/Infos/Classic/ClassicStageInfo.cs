// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI.Stages;
using osu.Game.Rulesets.Karaoke.UI.Stages.Classic;

namespace osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;

public class ClassicStageInfo : StageInfo
{
    #region Category

    /// <summary>
    /// Category to save the <see cref="Lyric"/>'s and <see cref="Note"/>'s style.
    /// </summary>
    public ClassicStyleCategory StyleCategory { get; set; } = new();

    /// <summary>
    /// The definition for the <see cref="Lyric"/>.<br/>
    /// Like the line height or font size.
    /// </summary>
    public ClassicStageDefinition StageDefinition { get; set; } = new();

    /// <summary>
    /// Category to save the <see cref="Lyric"/>'s layout.
    /// </summary>
    public ClassicLyricLayoutCategory LyricLayoutCategory { get; set; } = new();

    /// <summary>
    /// Timing info for saving the <see cref="Lyric"/>'s appear and disappear time.
    /// </summary>
    public ClassicLyricTimingInfo LyricTimingInfo { get; set; } = new();

    #endregion

    #region Stage element

    protected override IEnumerable<StageElement> GetLyricStageElements(Lyric lyric)
    {
        yield return StyleCategory.GetElementByItem(lyric);
        yield return LyricLayoutCategory.GetElementByItem(lyric);
    }

    protected override IEnumerable<StageElement> GetNoteStageElements(Note note)
    {
        // todo: should check the real-time mapping result.
        yield return StyleCategory.GetElementByItem(note.ReferenceLyric!);
    }

    #endregion

    #region Provider

    public override IPlayfieldStageApplier GetPlayfieldStageApplier()
        => new PlayfieldClassicStageApplier(StageDefinition);

    public override IPlayfieldCommandProvider CreatePlayfieldCommandProvider(bool displayNotePlayfield)
        => new ClassicPlayfieldCommandProvider(this, displayNotePlayfield);

    public override IHitObjectCommandProvider? CreateHitObjectCommandProvider<TObject>() =>
        typeof(TObject) switch
        {
            Type type when type == typeof(Lyric) => new ClassicLyricCommandProvider(this),
            Type type when type == typeof(Note) => null,
            _ => null
        };

    #endregion
}
