// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages;

public abstract class StageInfo
{
    public IEnumerable<IStageElement> GetStageElements(KaraokeHitObject hitObject) =>
        hitObject switch
        {
            Lyric lyric => GetLyricStageElements(lyric),
            Note note => GetNoteStageElements(note),
            _ => Array.Empty<IStageElement>()
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

    protected abstract IEnumerable<IStageElement> GetLyricStageElements(Lyric lyric);

    protected abstract IEnumerable<IStageElement> GetNoteStageElements(Note note);

    protected abstract IEnumerable<object> ConvertToLyricStageAppliers(IEnumerable<IStageElement> elements);

    protected abstract IEnumerable<object> ConvertToNoteStageAppliers(IEnumerable<IStageElement> elements);

    protected abstract Tuple<double?, double?> GetStartAndEndTime(Lyric lyric);

    #endregion
}
