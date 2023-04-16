// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Stages;
using osu.Game.Rulesets.Karaoke.Objects.Stages.Classic;
using osu.Game.Rulesets.Karaoke.UI.Stages;
using osu.Game.Rulesets.Karaoke.UI.Stages.Classic;

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

    protected override IPlayfieldStageApplier CreatePlayfieldStageApplier()
    {
        return new PlayfieldClassicStageApplier(StageDefinition);
    }

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

    protected override IStageEffectApplier ConvertToLyricStageAppliers(IEnumerable<StageElement> elements)
    {
        return new LyricClassicStageEffectApplier(elements, StageDefinition);
    }

    protected override IStageEffectApplier ConvertToNoteStageAppliers(IEnumerable<StageElement> elements)
    {
        return new NoteClassicStageEffectApplier(elements, StageDefinition);
    }

    protected override double GetPreemptTime(Lyric lyric)
    {
        // todo: should have the time if having loading effect with duration.
        return 0;
    }

    protected override double GetPreemptTime(Note note)
    {
        // todo: should have the time if having loading effect with duration.
        return 0;
    }

    protected override Tuple<double?, double?> GetStartAndEndTime(Lyric lyric)
    {
        return LyricTimingInfo.GetStartAndEndTime(lyric);
    }

    #endregion
}
