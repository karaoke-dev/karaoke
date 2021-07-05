// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagAutoGenerateSection : AutoGenerateSection
    {
        [Resolved]
        private LyricManager lyricManager { get; set; }

        protected override Dictionary<Lyric, string> GetDisableSelectingLyrics(Lyric[] lyrics)
            => lyrics.Where(x => x.Language == null)
                     .ToDictionary(k => k, i => "Before generate time-tag, need to assign language first.");

        protected override void Apply(Lyric[] lyrics)
            => lyricManager.AutoGenerateTimeTags(lyrics);

        protected override InvalidLyricAlertTextContainer CreateInvalidLyricAlertTextContainer()
            => new InvalidLyricLanguageAlertTextContainer();

        protected class InvalidLyricLanguageAlertTextContainer : InvalidLyricAlertTextContainer
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
