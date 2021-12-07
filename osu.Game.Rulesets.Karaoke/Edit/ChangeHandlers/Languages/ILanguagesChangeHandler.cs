// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Languages
{
    public interface ILanguagesChangeHandler
    {
        BindableList<CultureInfo> Languages { get; }

        void Add(CultureInfo culture);

        void Remove(CultureInfo culture);

        bool IsLanguageContainsTranslate(CultureInfo cultureInfo);
    }
}
