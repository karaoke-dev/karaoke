// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public abstract class TextTagAutoGenerateSection : AutoGenerateSection
    {
        protected override InvalidLyricAlertTextContainer CreateInvalidLyricAlertTextContainer()
            => new InvalidLyricLanguageAlertTextContainer();

        protected class InvalidLyricLanguageAlertTextContainer : InvalidLyricAlertTextContainer
        {
            private const string language_mode = "LANGUAGE_MODE";

            public InvalidLyricLanguageAlertTextContainer()
            {
                SwitchToModel(language_mode, "edit language mode", LyricEditorMode.Language);
                Text = $"Seems some lyric missing language, go to [{language_mode}] to fill the language.";
            }
        }
    }
}
