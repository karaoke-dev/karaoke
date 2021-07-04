// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages
{
    public class LanguageAutoGenerateSection : AutoGenerateSection
    {
        [Resolved]
        private LyricManager lyricManager { get; set; }

        protected override Dictionary<Lyric, string> GetDisableSelectingLyrics(Lyric[] lyrics)
            => lyrics.Where(x => string.IsNullOrEmpty(x.Text))
                     .ToDictionary(k => k, i => "Should have text in lyric.");

        protected override void Apply(Lyric[] lyrics)
            => lyricManager.AutoDetectLyricLanguage(lyrics);
    }
}
