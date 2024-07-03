// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;

public partial class LyricTranslationChangeHandler : LyricPropertyChangeHandler, ILyricTranslationChangeHandler
{
    public void UpdateTranslation(CultureInfo cultureInfo, string translation)
    {
        CheckExactlySelectedOneHitObject();

        PerformOnSelection(lyric =>
        {
            // should not save translation if is null or empty or whitespace
            if (string.IsNullOrWhiteSpace(translation))
            {
                if (lyric.Translations.ContainsKey(cultureInfo))
                    lyric.Translations.Remove(cultureInfo);
            }
            else
            {
                if (!lyric.Translations.TryAdd(cultureInfo, translation))
                    lyric.Translations[cultureInfo] = translation;
            }
        });
    }

    protected override bool IsWritePropertyLocked(Lyric lyric)
        => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.Translations));
}
