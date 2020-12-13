// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate.Components
{
    public class LanguageDropdown : OsuDropdown<CultureInfo>
    {
        protected override string GenerateItemText(CultureInfo item)
            => item.DisplayName;
    }
}
