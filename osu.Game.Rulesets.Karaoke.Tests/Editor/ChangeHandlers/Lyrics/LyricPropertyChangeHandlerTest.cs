// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics
{
    public abstract class LyricPropertyChangeHandlerTest<TChangeHandler> : BaseHitObjectChangeHandlerTest<TChangeHandler, Lyric>
        where TChangeHandler : LyricPropertyChangeHandler, new()
    {
        protected Lyric PrepareLyricWithSyncConfig(Lyric referencedLyric, IReferenceLyricPropertyConfig? config = null, bool selected = true)
        {
            var lyric = new Lyric
            {
                ReferenceLyric = referencedLyric,
                ReferenceLyricConfig = config ?? new SyncLyricConfig()
            };

            PrepareHitObjects(new[] { lyric }, selected);

            return lyric;
        }

        protected void TriggerHandlerChangedWithChangeForbiddenException(Action<TChangeHandler> c)
        {
            TriggerHandlerChangedWithException<LyricPropertyChangeHandler.ChangeForbiddenException>(c);
        }
    }
}
