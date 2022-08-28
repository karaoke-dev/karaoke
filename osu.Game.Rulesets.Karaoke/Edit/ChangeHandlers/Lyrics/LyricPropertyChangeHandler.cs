// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
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
            if (beatmap.SelectedHitObjects.OfType<Lyric>().Any(lyric => !AllowToEditIfHasReferenceLyric(lyric.ReferenceLyricConfig)))
                throw new ChangeForbiddenException();

            base.PerformOnSelection(action);
        }

        protected virtual bool AllowToEditIfHasReferenceLyric(IReferenceLyricPropertyConfig? config)
        {
            if (config == null)
                return true;

            return config switch
            {
                SyncLyricConfig => false,
                ReferenceLyricConfig => true,
                _ => throw new ArgumentOutOfRangeException(nameof(config), config, "unknown config.")
            };
        }

        public class ChangeForbiddenException : Exception
        {
            public ChangeForbiddenException()
                : base("Should not change the property because this property is referenced by other lyric.")
            {
            }
        }
    }
}
