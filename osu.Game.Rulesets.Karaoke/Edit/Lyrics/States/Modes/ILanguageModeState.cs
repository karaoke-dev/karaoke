// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Language;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes
{
    public interface ILanguageModeState : IHasEditModeState<LanguageEditMode>, IHasSpecialAction<LanguageEditModeSpecialAction>
    {
    }
}
