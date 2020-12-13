// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Skinning.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Lyrics
{
    public class DrawableEditLyric : DrawableLyric
    {
        public Action<KaraokeFont> ApplyFontAction;

        public DrawableEditLyric(Lyric lyric)
            : base(lyric)
        {
            DisplayRuby = true;
            DisplayRomaji = true;
        }

        protected override void ApplyFont(KaraokeFont font)
        {
            ApplyFontAction?.Invoke(font);
            base.ApplyFont(font);
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

        public int GetHoverIndex(float position)
        {
            var text = karaokeText.Text;
            if (string.IsNullOrEmpty(text))
                return 0;

            for (int i = 0; i < text.Length; i++)
            {
                if (GetPercentageWidth(i, i + 1, 0.5f) > position)
                    return i;
            }
            return text.Length;
        }
    }
}
