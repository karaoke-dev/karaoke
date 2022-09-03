// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public abstract class LyricPropertyChangeHandler : HitObjectChangeHandler<Lyric>
    {
        [Resolved, AllowNull]
        private EditorBeatmap beatmap { get; set; }

        protected sealed override void PerformOnSelection(Action<Lyric> action)
        {
            // note: should not check lyric in the perform on selection because it will let change handler in lazer broken.
            if (beatmap.SelectedHitObjects.OfType<Lyric>().Any(IsWritePropertyLocked))
                throw new ChangeForbiddenException();

            base.PerformOnSelection(action);
        }

        protected abstract bool IsWritePropertyLocked(Lyric lyric);

        public class ChangeForbiddenException : Exception
        {
            public ChangeForbiddenException()
                : base("Should not change the property because this property is referenced by other lyric.")
            {
            }
        }
    }
}
