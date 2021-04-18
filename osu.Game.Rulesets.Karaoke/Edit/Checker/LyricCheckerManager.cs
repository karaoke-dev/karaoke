﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Configs;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Checker
{
    /// <summary>
    /// This manager is for register and able to get invalid change by bindable.
    /// Note : this manager will be removed if able to get check result directly.
    /// </summary>
    public class LyricCheckerManager : Component
    {
        public BindableDictionary<Lyric, Issue[]> BindableReports = new BindableDictionary<Lyric, Issue[]>();

        private CheckInvalidRubyRomajiLyrics lyricChecker;

        public void CheckLyrics(List<HitObject> lyrics)
        {
            if (lyrics == null)
                throw new ArgumentNullException(nameof(lyrics));

            if (lyricChecker == null)
                throw new NullReferenceException(nameof(lyricChecker));

            var result = lyricChecker.Run(new Beatmap
            {
                HitObjects = lyrics
            });

            // re-calculate and add
            foreach (var lyric in lyrics)
            {
                // save issue to list.
                var issues = result.Where(x => x.HitObjects.Contains(lyric)).ToArray();
                if (!BindableReports.Contains(lyric))
                    BindableReports.Add(lyric, issues);
                else
                    BindableReports[lyric] = issues;
            }
        }

        public void CheckLyric(Lyric lyric)
            => CheckLyrics(new List<HitObject> { lyric });

        protected void RemoveFromCheckList(Lyric lyric)
            => BindableReports.Remove(lyric);

        [BackgroundDependencyLoader(true)]
        private void load(EditorBeatmap beatmap, KaraokeRulesetEditCheckerConfigManager rulesetEditCheckerConfigManager)
        {
            var config = rulesetEditCheckerConfigManager?.Get<LyricCheckerConfig>(KaraokeRulesetEditCheckerSetting.Lyric) ?? new LyricCheckerConfig().CreateDefaultConfig();
            lyricChecker = new CheckInvalidRubyRomajiLyrics(config);

            // load lyric in here
            CheckLyrics(beatmap.HitObjects.Where(x => x is Lyric).ToList());

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
