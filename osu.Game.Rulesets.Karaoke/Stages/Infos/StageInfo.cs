// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI.Stages;

namespace osu.Game.Rulesets.Karaoke.Stages.Infos;

public abstract class StageInfo
{
    public IPlayfieldStageApplier GetPlayfieldStageApplier()
        => CreatePlayfieldStageApplier();

    public abstract IHitObjectCommandProvider? CreateHitObjectCommandProvider<TObject>() where TObject : KaraokeHitObject;

    public IEnumerable<StageElement> GetStageElements(KaraokeHitObject hitObject) =>
        hitObject switch
        {
            Lyric lyric => GetLyricStageElements(lyric),
            Note note => GetNoteStageElements(note),
            _ => Array.Empty<StageElement>(),
        };

    public Tuple<double?, double?> GetStartAndEndTime(KaraokeHitObject hitObject) =>
        hitObject switch
        {
            Lyric lyric => GetStartAndEndTime(lyric),
            _ => throw new InvalidOperationException(),
        };

    #region Stage element

    protected abstract IPlayfieldStageApplier CreatePlayfieldStageApplier();

    protected abstract IEnumerable<StageElement> GetLyricStageElements(Lyric lyric);

    protected abstract IEnumerable<StageElement> GetNoteStageElements(Note note);

    protected abstract Tuple<double?, double?> GetStartAndEndTime(Lyric lyric);

    #endregion
}
