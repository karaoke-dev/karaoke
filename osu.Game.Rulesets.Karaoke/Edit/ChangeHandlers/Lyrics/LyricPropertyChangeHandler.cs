// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public abstract class LyricPropertyChangeHandler : HitObjectChangeHandler<Lyric>
    {
        protected sealed override void PerformOnSelection(Action<Lyric> action)
        {
            base.PerformOnSelection(lyric =>
            {
                if (!AllowToEditIfHasReferenceLyric(lyric.ReferenceLyricConfig))
                    throw new ChangeForbiddenException();

                action.Invoke(lyric);
            });
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
