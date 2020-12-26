// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags;
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

        public void AutoGenerateRomaji()
        {
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();
            if (!lyrics.Any())
                return;

            changeHandler?.BeginChange();

            var selector = new RomajiTagGeneratorSelector();

            foreach (var lyric in lyrics)
            {
                var romajiTags = selector.GenerateRomajiTags(lyric);
                lyric.RomajiTags = romajiTags;
            }

            changeHandler?.EndChange();
        }

        public bool CanAutoGenerateRuby()
        {
            var selector = new RubyTagGeneratorSelector();
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();
            return lyrics.Any(lyric => selector.Generatable(lyric));
        }

        public bool CanAutoGenerateRomaji()
        {
            var selector = new RomajiTagGeneratorSelector();
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();
            return lyrics.Any(lyric => selector.Generatable(lyric));
        }
    }
}
