// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public abstract class LyricPropertyChangeHandler : HitObjectPropertyChangeHandler<Lyric>, ILyricPropertyChangeHandler
    {
        [Resolved, AllowNull]
        private EditorBeatmap beatmap { get; set; }

        public virtual bool IsSelectionsLocked()
            => beatmap.SelectedHitObjects.OfType<Lyric>().Any(IsWritePropertyLocked);
    }
}
