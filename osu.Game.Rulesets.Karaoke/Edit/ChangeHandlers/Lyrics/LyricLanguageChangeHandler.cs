// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricLanguageChangeHandler : LyricPropertyChangeHandler, ILyricLanguageChangeHandler
    {
        public void SetLanguage(CultureInfo? language)
        {
            PerformOnSelection(lyric =>
            {
                if (EqualityComparer<CultureInfo?>.Default.Equals(language, lyric.Language))
                    return;

                lyric.Language = language;
            });
        }
    }
}
