// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.RubyRomaji;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RubyTagAutoGenerateSection : TextTagAutoGenerateSection
    {
        [Resolved]
        private RubyRomajiManager rubyRomaji { get; set; }

        protected override Dictionary<Lyric, string> GetDisableSelectingLyrics(Lyric[] lyrics)
            => lyrics.Where(x => x.Language == null)
                     .ToDictionary(k => k, _ => "Before generate ruby-tag, need to assign language first.");

        protected override void Apply(Lyric[] lyrics)
            => rubyRomaji.AutoGenerateLyricRuby(lyrics);
    }
}
