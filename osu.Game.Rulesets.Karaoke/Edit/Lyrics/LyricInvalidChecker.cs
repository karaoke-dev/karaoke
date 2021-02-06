// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class LyricInvalidChecker : Component
    {
        [Resolved]
        private KaraokeRulesetEditConfigManager configManager { get; set; }

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        public Lyric[] InvalidTimeLyrics()
        {
            return null;
        }

        public bool InvalidLyricTime(Lyric lyric)
        {
            return false;
        }

        public KeyValuePair<TimeTag, double>[] FindInvalidTimeTagTime(Lyric lyric)
        {
            return null;
        }

        public RubyTag[] CheckInvalidRubyRange(Lyric lyric)
        {
            return null;
        }

        public RubyTag[] CheckDuplicatedRubyPosition(Lyric lyric)
        {
            return null;
        }

        public RomajiTag[] CheckInvalidRomajiRange(Lyric lyric)
        {
            return null;
        }

        public RomajiTag[] CheckDuplicatedRomajiPosition(Lyric lyric)
        {
            return null;
        }
    }
}
