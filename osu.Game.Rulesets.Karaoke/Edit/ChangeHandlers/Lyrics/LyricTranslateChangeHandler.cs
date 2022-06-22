// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using System.Globalization;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricTranslateChangeHandler : HitObjectChangeHandler<Lyric>, ILyricTranslateChangeHandler
    {
        public void UpdateTranslate(CultureInfo cultureInfo, string translate)
        {
            CheckExactlySelectedOneHitObject();

            PerformOnSelection(lyric =>
            {
                // should not save translate if is null or empty or whitespace
                if (string.IsNullOrWhiteSpace(translate))
                {
                    if (lyric.Translates.ContainsKey(cultureInfo))
                        lyric.Translates.Remove(cultureInfo);
                }
                else
                {
                    if (!lyric.Translates.TryAdd(cultureInfo, translate))
                        lyric.Translates[cultureInfo] = translate;
                }
            });
        }
    }
}
