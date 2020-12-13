// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Skinning.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components
{
    public class DrawableEditLyric : DrawableLyric
    {
        public Action ApplyFontAction;

        public DrawableEditLyric(Lyric lyric)
            : base(lyric)
        {
            DisplayRuby = true;
            DisplayRomaji = true;
        }

        protected override void ApplyFont(KaraokeFont font)
        {
            base.ApplyFont(font);

            if (TimeTagsBindable.Value == null)
                return;

            ApplyFontAction?.Invoke();
        }

        protected override void ApplyLayout(KaraokeLayout layout)
        {
            base.ApplyLayout(layout);
            Padding = new MarginPadding(0);
        }

        protected override void UpdateStartTimeStateTransforms()
        {
            // Do not fade-in / fade-out while changing armed state.
        }

        public override double LifetimeStart
        {
            get => double.MinValue;
            set => base.LifetimeStart = double.MinValue;
        }

        public override double LifetimeEnd
        {
            get => double.MaxValue;
            set => base.LifetimeEnd = double.MaxValue;
        }

        public float GetPercentageWidth(int startIndex, int endIndex, float percentage = 0)
            => karaokeText.GetPercentageWidth(startIndex, endIndex, percentage);
    }
}
