// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Checker.Lyrics
{
    /// <summary>
    /// This manager is for register and able to get invalid change by bindable.
    /// </summary>
    public class LyricCheckerManager : Component
    {
        public BindableDictionary<Lyric, LyricCheckReport> BindableReports = new BindableDictionary<Lyric, LyricCheckReport>();

        private LyricChecker lyricChecker;

        public void CheckLyrics(List<Lyric> lyrics, LyricCheckProperty checkProperty = LyricCheckProperty.All)
        {
            if (lyrics == null)
                throw new ArgumentNullException(nameof(lyrics));

            if (lyricChecker == null)
                throw new NullReferenceException(nameof(lyricChecker));

            // re-calculate and add
            foreach (var lyric in lyrics)
            {
                // create report record if not have.
                if (!BindableReports.Contains(lyric))
                    BindableReports.Add(lyric, new LyricCheckReport());

                var report = BindableReports[lyric];
                if (checkProperty.HasFlag(LyricCheckProperty.Time))
                    report.TimeInvalid = lyricChecker.CheckInvalidLyricTime(lyric);

                if (checkProperty.HasFlag(LyricCheckProperty.TimeTag))
                    report.InvalidTimeTags = lyricChecker.CheckInvalidTimeTags(lyric);

                if (checkProperty.HasFlag(LyricCheckProperty.Ruby))
                    report.InvalidRubyTags = lyricChecker.CheckInvalidRubyTags(lyric);

                if (checkProperty.HasFlag(LyricCheckProperty.Romaji))
                    report.InvalidRomajiTags = lyricChecker.CheckInvalidRomajiTags(lyric);
            }
        }

        public void CheckLyric(Lyric lyric, LyricCheckProperty checkProperty = LyricCheckProperty.All)
            => CheckLyrics(new List<Lyric> { lyric }, checkProperty);

        protected void RemoveFromCheckList(Lyric lyric)
            => BindableReports.Remove(lyric);

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap beatmap, KaraokeRulesetEditCheckerConfigManager rulesetEditCheckerConfigManager)
        {
            var config = rulesetEditCheckerConfigManager.Get<LyricCheckerConfig>(KaraokeRulesetEditCheckerSetting.Lyric);
            lyricChecker = new LyricChecker(config);

            // load lyric in here
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();
            CheckLyrics(lyrics);

            // need to check is there any lyric added or removed.
            beatmap.HitObjectAdded += e =>
            {
                if (e is Lyric lyric)
                    CheckLyric(lyric);
            };
            beatmap.HitObjectRemoved += e =>
            {
                if (e is Lyric lyric)
                    RemoveFromCheckList(lyric);
            };
        }
    }
}
