// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Ja;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Zh;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using System;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class TimeTagManager : Component
    {
        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [Resolved(CanBeNull = true)]
        private IEditorChangeHandler changeHandler { get; set; }

        /// <summary>
        /// Will auto-detect each <see cref="Lyric"/> 's <see cref="Lyric.TimeTags"/> and apply on them.
        /// </summary>
        public void AutoGenerateTimeTags()
        {
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();
            if (!lyrics.Any())
                return;

            changeHandler?.BeginChange();

            var selector = new TimeTagGeneratorSelector();

            foreach (var lyric in lyrics)
            {
                var timeTags = selector.GenerateTimeTags(lyric);
                lyric.TimeTags = timeTags;
            }

            changeHandler?.EndChange();
        }

        public class TimeTagGeneratorSelector
        {
            private readonly Lazy<JaTimeTagGenerator> jaTimeTagGenerator;
            private readonly Lazy<ZhTimeTagGenerator> zhTimeTagGenerator;

            public TimeTagGeneratorSelector()
            {
                jaTimeTagGenerator = new Lazy<JaTimeTagGenerator>(() =>
                {
                    // todo : get config from setting.
                    var config = new JaTimeTagGeneratorConfig();
                    return new JaTimeTagGenerator(config);
                });
                zhTimeTagGenerator = new Lazy<ZhTimeTagGenerator>(() =>
                {
                    // todo : get config from setting.
                    var config = new ZhTimeTagGeneratorConfig();
                    return new ZhTimeTagGenerator(config);
                });
            }

            public TimeTag[] GenerateTimeTags(Lyric lyric)
            {
                // lazy to generate language detector and apply it's setting
                switch (lyric.Language?.LCID)
                {
                    case 17:
                    case 1041:
                        return jaTimeTagGenerator.Value.CreateTimeTags(lyric);

                    case 1028:
                        return zhTimeTagGenerator.Value.CreateTimeTags(lyric);

                    default:
                        return null;
                }
            }
        }
    }
}
