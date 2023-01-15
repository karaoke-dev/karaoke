// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Lyrics.Language;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Language
{
    public partial class LanguageAutoGenerateSubsection : LyricEditorAutoGenerateSubsection
    {
        private const string typing_mode = "TYPING_MODE";

        public LanguageAutoGenerateSubsection()
            : base(LyricAutoGenerateProperty.DetectLanguage)
        {
        }

        protected override DescriptionFormat CreateInvalidDescriptionFormat()
            => new()
            {
                Text = $"Seems some lyric has no texts, go to [{DescriptionFormat.LINK_KEY_ACTION}]({typing_mode}) to fill the text.",
                Actions = new Dictionary<string, IDescriptionAction>
                {
                    {
                        typing_mode, new SwitchModeDescriptionAction
                        {
                            Text = "typing mode",
                            Mode = LyricEditorMode.Texting
                        }
                    }
                }
            };

        protected override ConfigButton CreateConfigButton()
            => new LanguageAutoGenerateConfigButton();

        protected partial class LanguageAutoGenerateConfigButton : ConfigButton
        {
            public override Popover GetPopover()
                => new LanguageDetectorConfigPopover();
        }
    }
}
