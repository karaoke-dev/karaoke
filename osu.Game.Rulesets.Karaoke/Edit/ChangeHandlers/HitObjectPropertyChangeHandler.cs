// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;

public abstract partial class HitObjectPropertyChangeHandler<THitObject> : HitObjectChangeHandler<THitObject>, IHitObjectPropertyChangeHandler
    where THitObject : HitObject
{
    [Resolved]
    private EditorBeatmap beatmap { get; set; } = null!;

    protected sealed override void PerformOnSelection(Action<THitObject> action)
    {
        // note: should not check lyric in the perform on selection because it will let change handler in lazer broken.
        if (beatmap.SelectedHitObjects.OfType<THitObject>().Any(IsWritePropertyLocked))
            throw new ChangeForbiddenException();

        base.PerformOnSelection(action);
    }

    protected abstract bool IsWritePropertyLocked(THitObject hitObject);

    public virtual bool IsSelectionsLocked()
        => beatmap.SelectedHitObjects.OfType<THitObject>().Any(IsWritePropertyLocked);

    public class ChangeForbiddenException : InvalidOperationException
    {
        public ChangeForbiddenException()
            : base("This property might be locked or it's a reference property.")
        {
        }
    }
}
