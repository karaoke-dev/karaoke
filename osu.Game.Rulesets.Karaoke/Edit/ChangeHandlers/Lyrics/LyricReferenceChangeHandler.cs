// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricReferenceChangeHandler : HitObjectChangeHandler<Lyric>, ILyricReferenceChangeHandler
    {
        public void UpdateReferenceLyric(Lyric? referenceLyric)
        {
            if (referenceLyric != null && !HitObjects.Contains(referenceLyric))
                throw new InvalidOperationException($"{nameof(referenceLyric)} should in the beatmap.");

            PerformOnSelection(lyric =>
            {
                if (referenceLyric == lyric)
                    throw new InvalidOperationException($"{nameof(referenceLyric)} should not be the same instance as {nameof(lyric)}");

                if (referenceLyric?.ReferenceLyric != null)
                    throw new InvalidOperationException($"{nameof(referenceLyric)} should not contains another reference lyric.");

                lyric.ReferenceLyric = referenceLyric;

                if (lyric.ReferenceLyric == null)
                {
                    lyric.ReferenceLyricConfig = null;
                }
                else if (lyric.ReferenceLyricConfig == null)
                {
                    // todo: not really sure should use sync config if lyric text are similar.
                    lyric.ReferenceLyricConfig = new ReferenceLyricConfig();
                }
            });
        }

        public void SwitchToReferenceLyricConfig()
        {
            PerformOnSelection(lyric =>
            {
                if (lyric == null)
                    throw new InvalidOperationException($"{nameof(lyric)} must have reference lyric.");

                lyric.ReferenceLyricConfig = new ReferenceLyricConfig();
            });
        }

        public void SwitchToSyncLyricConfig()
        {
            PerformOnSelection(lyric =>
            {
                if (lyric == null)
                    throw new InvalidOperationException($"{nameof(lyric)} must have reference lyric.");

                lyric.ReferenceLyricConfig = new SyncLyricConfig();
            });
        }

        public void AdjustLyricConfig<TConfig>(Action<TConfig> action) where TConfig : IReferenceLyricPropertyConfig
        {
            PerformOnSelection(lyric =>
            {
                if (lyric.ReferenceLyricConfig is not TConfig config)
                    throw new InvalidOperationException($"{nameof(config)} must be the type of ${typeof(TConfig)}.");

                action.Invoke(config);
            });
        }
    }
}
