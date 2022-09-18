// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using J2N.Collections.Generic;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.Language;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Language
{
    public class LanguageAutoGenerateSubsection : AutoGenerateSubsection
    {
        private const string typing_mode = "TYPING_MODE";

        public LanguageAutoGenerateSubsection()
            : base(LyricAutoGenerateProperty.DetectLanguage)
        {
        }

        protected override DescriptionFormat CreateInvalidLyricDescriptionFormat()
            => new()
            {
                Text = $"Seems some lyric has no texts, go to [{DescriptionFormat.LINK_KEY_EDIT_MODE}]({typing_mode}) to fill the text.",
                EditModes = new Dictionary<string, SwitchMode>
                {
                    {
                        typing_mode, new SwitchMode
                        {
                            Text = "typing mode",
                            Mode = LyricEditorMode.Texting
                        }
                    }
                }
            };

        protected override ConfigButton CreateConfigButton()
            => new LanguageAutoGenerateConfigButton();

        protected class LanguageAutoGenerateConfigButton : ConfigButton
        {
            public override Popover GetPopover()
                => new LanguageDetectorConfigPopover();
        }
    }
}
