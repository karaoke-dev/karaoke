// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.ReferenceLyric;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;

public partial class LyricReferenceChangeHandler : LyricPropertyChangeHandler, ILyricReferenceChangeHandler
{
    #region Auto-Generate

    public bool CanGenerate()
    {
        var detector = GetDetector<Lyric?, ReferenceLyricDetectorConfig>(HitObjects);
        return CanDetect(detector);
    }

    public IDictionary<Lyric, LocalisableString> GetGeneratorNotSupportedLyrics()
    {
        var detector = GetDetector<Lyric?, ReferenceLyricDetectorConfig>(HitObjects);
        return GetInvalidMessageFromDetector(detector);
    }

    public void AutoGenerate()
    {
        var detector = GetDetector<Lyric?, ReferenceLyricDetectorConfig>(HitObjects);

        PerformOnSelection(lyric =>
        {
            var referencedLyric = detector.Detect(lyric);
            lyric.ReferenceLyricId = referencedLyric?.ID;

            // technically this property should be assigned by beatmap processor, but should be OK to assign here for testing purpose.
            lyric.ReferenceLyric = referencedLyric;

            if (lyric.ReferenceLyricId != null && lyric.ReferenceLyricConfig is not SyncLyricConfig)
                lyric.ReferenceLyricConfig = new SyncLyricConfig();
        });
    }

    #endregion

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

            lyric.ReferenceLyricId = referenceLyric?.ID;

            if (lyric.ReferenceLyricId == null)
            {
                lyric.ReferenceLyricConfig = null;
            }
            else if (lyric.ReferenceLyricConfig == null)
            {
                // todo: not really sure should use sync config if lyric text are similar.
                lyric.ReferenceLyricConfig = new ReferenceLyricConfig();
            }

            TriggerHitObjectUpdate(lyric);
        });
    }

    public void SwitchToReferenceLyricConfig()
    {
        PerformOnSelection(lyric =>
        {
            if (lyric.ReferenceLyric == null)
                throw new InvalidOperationException($"{nameof(lyric)} must have reference lyric.");

            lyric.ReferenceLyricConfig = new ReferenceLyricConfig();
        });
    }

    public void SwitchToSyncLyricConfig()
    {
        PerformOnSelection(lyric =>
        {
            if (lyric.ReferenceLyric == null)
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

    protected override bool IsWritePropertyLocked(Lyric lyric)
        => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.ReferenceLyric), nameof(Lyric.ReferenceLyricConfig));
}
