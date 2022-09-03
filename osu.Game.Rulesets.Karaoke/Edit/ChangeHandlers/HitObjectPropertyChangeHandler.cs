// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers
{
    public abstract class HitObjectPropertyChangeHandler<THitObject> : HitObjectChangeHandler<THitObject> where THitObject : HitObject
    {
        [Resolved, AllowNull]
        private EditorBeatmap beatmap { get; set; }

        protected sealed override void PerformOnSelection(Action<THitObject> action)
        {
            // note: should not check lyric in the perform on selection because it will let change handler in lazer broken.
            if (beatmap.SelectedHitObjects.OfType<THitObject>().Any(IsWritePropertyLocked))
                throw new ChangeForbiddenException();

            base.PerformOnSelection(action);
        }

        protected abstract bool IsWritePropertyLocked(THitObject hitObject);

        public class ChangeForbiddenException : Exception
        {
            public ChangeForbiddenException()
                : base("Should not change the property because this property is referenced by other lyric.")
            {
            }
        }
    }
}
