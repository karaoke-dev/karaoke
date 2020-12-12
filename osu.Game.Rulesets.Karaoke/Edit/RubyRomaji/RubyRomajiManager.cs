// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.RubyRomaji
{
    public class RubyRomajiManager : Component
    {
        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [Resolved(CanBeNull = true)]
        private IEditorChangeHandler changeHandler { get; set; }

        /// <summary>
        /// Will auto-detect each <see cref="Lyric"/> 's <see cref="Lyric.RubyTags"/> and apply on them.
        /// </summary>
        public void AutoGenerateRubyTags()
        {
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();
            if (!lyrics.Any())
                return;

            changeHandler?.BeginChange();

            var selector = new RubyTagGeneratorSelector();

            foreach (var lyric in lyrics)
            {
                var rubyTags = selector.GenerateRubyTags(lyric);
                lyric.RubyTags = rubyTags;
            }

            changeHandler?.EndChange();
        }

        public class RubyTagGeneratorSelector
        {
            private readonly Lazy<JaRubyTagGenerator> jaRubyTagGenerator;

            public RubyTagGeneratorSelector()
            {
                jaRubyTagGenerator = new Lazy<JaRubyTagGenerator>(() =>
                {
                    // todo : get config from setting.
                    var config = new JaRubyTagGeneratorConfig();
                    return new JaRubyTagGenerator(config);
                });
            }

            public RubyTag[] GenerateRubyTags(Lyric lyric)
            {
                // lazy to generate language detector and apply it's setting
                switch (lyric.Language.LCID)
                {
                    case 17:
                    case 1041:
                        return jaRubyTagGenerator.Value.CreateRubyTags(lyric);

                    default:
                        return null;
                }
            }
        }
    }
}
