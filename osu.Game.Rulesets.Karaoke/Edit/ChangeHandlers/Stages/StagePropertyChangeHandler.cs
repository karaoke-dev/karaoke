// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Stages;

/// <summary>
/// Note: will start to implement this class after the stage info is able to edit.
/// </summary>
public partial class StagePropertyChangeHandler : Component
{
    protected IEnumerable<Lyric> Lyrics => throw new NotImplementedException();

    protected void PerformOnSelection<T>(Action<T> action) where T : HitObject
    {
        throw new NotImplementedException();
    }
}
