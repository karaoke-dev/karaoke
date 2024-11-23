// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI.Stages;

namespace osu.Game.Rulesets.Karaoke.Stages.Infos;

public abstract class StageInfo
{
    #region Stage element

    public IEnumerable<StageElement> GetStageElements(KaraokeHitObject hitObject) =>
        hitObject switch
        {
            Lyric lyric => GetLyricStageElements(lyric),
            Note note => GetNoteStageElements(note),
            _ => Array.Empty<StageElement>(),
        };

    protected abstract IEnumerable<StageElement> GetLyricStageElements(Lyric lyric);

    protected abstract IEnumerable<StageElement> GetNoteStageElements(Note note);

    #endregion

    #region Provider

    public abstract IPlayfieldStageApplier GetPlayfieldStageApplier();

    public abstract IPlayfieldCommandProvider CreatePlayfieldCommandProvider(bool displayNotePlayfield);

    public abstract IStageElementProvider? CreateStageElementProvider(bool displayNotePlayfield);

    public abstract IHitObjectCommandProvider? CreateHitObjectCommandProvider<TObject>() where TObject : KaraokeHitObject;

    #endregion
}
