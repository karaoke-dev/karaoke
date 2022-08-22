// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public interface ILyricReferenceChangeHandler
    {
        void UpdateReferenceLyric(Lyric? referenceLyric);

        void SwitchToReferenceLyricConfig();

        void SwitchToSyncLyricConfig();

        void AdjustLyricConfig<TConfig>(Action<TConfig> action) where TConfig : IReferenceLyricPropertyConfig;
    }
}
