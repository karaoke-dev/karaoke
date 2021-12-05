// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Languages
{
    public interface ILanguagesChangeHandler
    {
        void Add(CultureInfo culture);

        void Remove(CultureInfo culture);
    }
}
