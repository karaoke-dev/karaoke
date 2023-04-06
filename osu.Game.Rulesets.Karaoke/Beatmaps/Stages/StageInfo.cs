// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages;

public abstract class StageInfo
{
    #region Init

    /// <summary>
    /// Should call this method on the <see cref="KaraokeBeatmapProcessor"/> before the <see cref="KaraokeBeatmapProcessor.PreProcess()"/>
    /// And note that this method is for "patching" the property from the beatmap to the stage.
    /// So should not keep the reference of the beatmap.
    /// </summary>
    /// <param name="beatmap"></param>
    public virtual void ReloadBeatmap(IBeatmap beatmap)
    {
        // for the case that we need to get the properties from the beatmap.
    }

    #endregion

    public IEnumerable<StageElement> GetStageElements(KaraokeHitObject hitObject) =>
        hitObject switch
        {
            Lyric lyric => GetLyricStageElements(lyric),
            Note note => GetNoteStageElements(note),
            _ => Array.Empty<StageElement>()
        };

    // todo: very not sure should be better to return the applier or style/layout data.
    public IEnumerable<object> GetStageAppliers(KaraokeHitObject hitObject)
    {
        var elements = GetStageElements(hitObject);

        return hitObject switch
        {
            Lyric => ConvertToLyricStageAppliers(elements),
            Note => ConvertToNoteStageAppliers(elements),
            _ => Array.Empty<object>()
        };
    }

    public Tuple<double?, double?> GetStartAndEndTime(KaraokeHitObject hitObject) =>
        hitObject switch
        {
            Lyric lyric => GetStartAndEndTime(lyric),
            _ => throw new InvalidOperationException()
        };

    #region Stage element

    protected abstract IEnumerable<StageElement> GetLyricStageElements(Lyric lyric);

    protected abstract IEnumerable<StageElement> GetNoteStageElements(Note note);

    protected abstract IEnumerable<object> ConvertToLyricStageAppliers(IEnumerable<StageElement> elements);

    protected abstract IEnumerable<object> ConvertToNoteStageAppliers(IEnumerable<StageElement> elements);

    protected abstract Tuple<double?, double?> GetStartAndEndTime(Lyric lyric);

    #endregion
}
