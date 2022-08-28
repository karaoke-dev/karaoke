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
        protected void PrepareLyricWithSyncConfig(Lyric hitObject, bool selected = true)
        {
            // allow to pre-assign the config.
            hitObject.ReferenceLyric = new Lyric();
            hitObject.ReferenceLyricConfig ??= new SyncLyricConfig();

            PrepareHitObjects(new[] { hitObject }, selected);
        }

        protected void TriggerHandlerChangedWithChangeForbiddenException(Action<TChangeHandler> c)
        {
            TriggerHandlerChangedWithException<LyricPropertyChangeHandler.ChangeForbiddenException>(c);
        }
    }
}
