// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
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
        public BindableDictionary<Lyric, Issue[]> BindableReports = new();

        private LyricVerifier lyricVerifier;

        public void CheckLyrics(List<HitObject> lyrics)
        {
            if (lyrics == null)
                throw new ArgumentNullException(nameof(lyrics));

            if (lyricVerifier == null)
                throw new NullDependencyException(nameof(lyricVerifier));

            var fakeBeatmap = new Beatmap
            {
                HitObjects = lyrics
            };
            var result = lyricVerifier.Run(new BeatmapVerifierContext(fakeBeatmap, null)).ToArray();

            // re-calculate and add
            foreach (var lyric in lyrics.OfType<Lyric>())
            {
                // save issue to list.
                var issues = result.Where(x => x.HitObjects.Contains(lyric)).ToArray();
                if (!BindableReports.ContainsKey(lyric))
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
            lyricVerifier = new LyricVerifier(config);

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

        // It's a temp verifier for just checking lyric relative things.
        public class LyricVerifier : IBeatmapVerifier
        {
            private readonly List<ICheck> checks;

            public LyricVerifier(LyricCheckerConfig config)
            {
                checks = new List<ICheck>
                {
                    new CheckLyricText(),
                    new CheckLyricLanguage(),
                    new CheckLyricRubyTag(),
                    new CheckLyricRomajiTag(),
                    new CheckLyricSinger(),
                    new CheckLyricTranslate(),
                    new CheckInvalidRubyRomajiLyrics
                    {
                        Config = config
                    },
                    new CheckInvalidTimeLyrics
                    {
                        Config = config
                    },
                    new CheckInvalidPropertyNotes(),
                    new CheckBeatmapAvailableTranslates(),
                };
            }

            public IEnumerable<Issue> Run(BeatmapVerifierContext context) => checks.SelectMany(check => check.Run(context));
        }
    }
}
