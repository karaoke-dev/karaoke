// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public interface ILyricTranslateChangeHandler : ILyricPropertyChangeHandler
    {
        void UpdateTranslate(CultureInfo cultureInfo, string translate);
    }
}
