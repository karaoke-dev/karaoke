// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.ReferenceLyric;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Reference
{
    public class ReferenceLyricAutoGenerateSection : Section
    {
        protected override LocalisableString Title => "Auto generate";

        public ReferenceLyricAutoGenerateSection()
        {
            Children = new[]
            {
                new ReferenceLyricAutoGenerateSubsection()
            };
        }

        private class ReferenceLyricAutoGenerateSubsection : AutoGenerateSubsection
        {
            public ReferenceLyricAutoGenerateSubsection()
                : base(LyricAutoGenerateProperty.DetectReferenceLyric)
            {
            }

            protected override InvalidLyricAlertTextContainer CreateInvalidLyricAlertTextContainer()
                => new InvalidLyricLanguageAlertTextContainer();

            protected override ConfigButton CreateConfigButton()
                => new ReferenceLyricAutoGenerateConfigButton();

            protected class InvalidLyricLanguageAlertTextContainer : InvalidLyricAlertTextContainer
            {
                private const string language_mode = "LANGUAGE_MODE";

                public InvalidLyricLanguageAlertTextContainer()
                {
                    SwitchToEditorMode(language_mode, "edit language mode", LyricEditorMode.Language);
                    Text = $"Seems some lyric missing language, go to [{language_mode}] to fill the language.";
                }
            }

            protected class ReferenceLyricAutoGenerateConfigButton : ConfigButton
            {
                public override Popover GetPopover()
                    => new ReferenceLyricDetectorConfigPopover();
            }
        }
    }
}
