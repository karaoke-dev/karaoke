// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.Languages;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages
{
    public class LanguageAutoGenerateSection : AutoGenerateSection
    {
        [Resolved]
        private ILyricLanguageChangeHandler lyricLanguageChangeHandler { get; set; }

        protected override Dictionary<Lyric, string> GetDisableSelectingLyrics(IEnumerable<Lyric> lyrics)
            => lyrics.Where(x => string.IsNullOrEmpty(x.Text))
                     .ToDictionary(k => k, _ => "Should have text in lyric.");

        protected override void Apply()
            => lyricLanguageChangeHandler.AutoGenerate();

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
