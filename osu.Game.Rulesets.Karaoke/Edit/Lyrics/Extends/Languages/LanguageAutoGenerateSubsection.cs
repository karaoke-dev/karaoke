// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.Languages;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages
{
    public class LanguageAutoGenerateSubsection : AutoGenerateSubsection
    {
        public LanguageAutoGenerateSubsection()
            : base(LyricAutoGenerateProperty.DetectLanguage)
        {
        }

        protected override InvalidLyricAlertTextContainer CreateInvalidLyricAlertTextContainer()
            => new InvalidLyricTextAlertTextContainer();

        protected override ConfigButton CreateConfigButton()
            => new LanguageAutoGenerateConfigButton();

        protected class InvalidLyricTextAlertTextContainer : InvalidLyricAlertTextContainer
        {
            private const string edit_mode = "TYPING_MODE";

            public InvalidLyricTextAlertTextContainer()
            {
                SwitchToEditorMode(edit_mode, "typing mode", LyricEditorMode.Typing);
                Text = $"Seems some lyric has no texts, go to [{edit_mode}] to fill the text.";
            }
        }

        protected class LanguageAutoGenerateConfigButton : ConfigButton
        {
            public override Popover GetPopover()
                => new LanguageDetectorConfigPopover();
        }
    }
}
