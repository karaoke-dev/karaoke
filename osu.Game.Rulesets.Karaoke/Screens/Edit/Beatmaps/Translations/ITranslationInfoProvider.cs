// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Translations;

public interface ITranslationInfoProvider
{
    string? GetLyricTranslation(Lyric lyric, CultureInfo cultureInfo);
}
