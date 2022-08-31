// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Mods
{
    public class KaraokeModHiddenNote : ModHidden
    {
        public override LocalisableString Description => @"Notes fade out before you sing them!";
        public override double ScoreMultiplier => 1.06;
        public override Type[] IncompatibleMods => new[] { typeof(ModFlashlight<KaraokeHitObject>) };
        public override IconUsage? Icon => KaraokeIcon.ModHiddenNote;

        private const double fade_in_duration_multiplier = -1;
        private const double fade_out_duration_multiplier = 0.3;

        public override void ApplyToDrawableHitObject(DrawableHitObject dho)
        {
            if (dho is DrawableNote drawableNote)
            {
                var note = drawableNote.HitObject;
                note.TimeFadeIn = note.TimePreempt * fade_in_duration_multiplier;
            }

            base.ApplyToDrawableHitObject(dho);
        }

        protected override void ApplyIncreasedVisibilityState(DrawableHitObject hitObject, ArmedState state)
        {
            // todo : not really sure what this do so just copy the code in below.
            if (hitObject is not DrawableNote note)
                return;

            var h = note.HitObject;

            double fadeOutStartTime = h.StartTime - h.TimePreempt + h.TimeFadeIn;
            double fadeOutDuration = h.TimePreempt * fade_out_duration_multiplier;

            // new duration from completed fade in to end (before fading out)
            double longFadeDuration = h.EndTime - fadeOutStartTime;

            // Apply duration
            using (note.BeginAbsoluteSequence(fadeOutStartTime))
                note.FadeOut(fadeOutDuration, Easing.Out);

            // Show after exceed hit point
            using (note.BeginAbsoluteSequence(fadeOutStartTime + longFadeDuration))
                note.FadeIn(fadeOutDuration, Easing.Out);
        }

        protected override void ApplyNormalVisibilityState(DrawableHitObject hitObject, ArmedState state)
        {
            if (hitObject is not DrawableNote note)
                return;

            var h = note.HitObject;

            double fadeOutStartTime = h.StartTime - h.TimePreempt + h.TimeFadeIn;
            double fadeOutDuration = h.TimePreempt * fade_out_duration_multiplier;

            // new duration from completed fade in to end (before fading out)
            double longFadeDuration = h.EndTime - fadeOutStartTime;

            // Apply duration
            using (note.BeginAbsoluteSequence(fadeOutStartTime))
                note.FadeOut(fadeOutDuration, Easing.Out);

            // Show after exceed hit point
            using (note.BeginAbsoluteSequence(fadeOutStartTime + longFadeDuration))
                note.FadeIn(fadeOutDuration, Easing.Out);
        }
    }
}
