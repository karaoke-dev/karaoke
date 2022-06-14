// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public abstract class TextTagAutoGenerateSection : Section
    {
        protected sealed override LocalisableString Title => "Auto generate";

        protected abstract class TextTagAutoGenerateSubsection : AutoGenerateSubsection
        {
            protected TextTagAutoGenerateSubsection(LyricAutoGenerateProperty autoGenerateProperty)
                : base(autoGenerateProperty)
            {
            }

            protected override InvalidLyricAlertTextContainer CreateInvalidLyricAlertTextContainer()
                => new InvalidLyricLanguageAlertTextContainer();

            private class InvalidLyricLanguageAlertTextContainer : InvalidLyricAlertTextContainer
            {
                private const string language_mode = "LANGUAGE_MODE";

                public InvalidLyricLanguageAlertTextContainer()
                {
                    SwitchToEditorMode(language_mode, "edit language mode", LyricEditorMode.Language);
                    Text = $"Seems some lyric missing language, go to [{language_mode}] to fill the language.";
                }
            }
        }
    }
}
