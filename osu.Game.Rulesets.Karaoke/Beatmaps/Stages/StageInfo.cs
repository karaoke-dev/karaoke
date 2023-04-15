// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Stages;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages;

public abstract class StageInfo
{
    public IStageEffectApplier GetStageAppliers(KaraokeHitObject hitObject)
    {
        var elements = getStageElements(hitObject);

        return hitObject switch
        {
            Lyric => ConvertToLyricStageAppliers(elements),
            Note => ConvertToNoteStageAppliers(elements),
            _ => throw new InvalidOperationException()
        };
    }

    private IEnumerable<StageElement> getStageElements(KaraokeHitObject hitObject) =>
        hitObject switch
        {
            Lyric lyric => GetLyricStageElements(lyric),
            Note note => GetNoteStageElements(note),
            _ => Array.Empty<StageElement>()
        };

    public Tuple<double?, double?> GetStartAndEndTime(KaraokeHitObject hitObject) =>
        hitObject switch
        {
            Lyric lyric => GetStartAndEndTime(lyric),
            _ => throw new InvalidOperationException()
        };

    #region Stage element

    protected abstract IEnumerable<StageElement> GetLyricStageElements(Lyric lyric);

    protected abstract IEnumerable<StageElement> GetNoteStageElements(Note note);

    protected abstract IStageEffectApplier ConvertToLyricStageAppliers(IEnumerable<StageElement> elements);

    protected abstract IStageEffectApplier ConvertToNoteStageAppliers(IEnumerable<StageElement> elements);

    protected abstract Tuple<double?, double?> GetStartAndEndTime(Lyric lyric);

    #endregion
}
