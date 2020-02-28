// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Objects.Drawables;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Rulesets.Scoring;
using osu.Game.Skinning;
using osuTK;
using osuTK.Graphics;
using osu.Game.Rulesets.Karaoke.Judgements;
using System;
using osu.Game.Rulesets.Judgements;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables
{
    public class DrawableLyricLine : DrawableKaraokeHitObject
    {
        private readonly KarakeSpriteText karaokeText;
        private readonly OsuSpriteText translateText;

        /// <summary>
        /// Invoked when a <see cref="JudgementResult"/> has been applied by this <see cref="DrawableHitObject"/> or a nested <see cref="DrawableHitObject"/>.
        /// </summary>
        public event Action<DrawableHitObject, JudgementResult> OnLyricStart;

        public new LyricLine HitObject => (LyricLine)base.HitObject;

        public DrawableLyricLine(LyricLine hitObject)
            : base(hitObject)
        {
            Scale = new Vector2(2f);
            AutoSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                karaokeText = new KarakeSpriteText(),
                translateText = new OsuSpriteText
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.TopLeft,
                    Text = hitObject.TranslateText
                }
            };

            hitObject.TextBindable.BindValueChanged(text =>
            {
                karaokeText.Text = text.NewValue;
            }, true);

            hitObject.TimeTagsBindable.BindValueChanged(timeTags =>
            {
                karaokeText.TimeTags = timeTags.NewValue;
            }, true);

            hitObject.RubyTagsBindable.BindValueChanged(rubyTags =>
            {
                ApplyRuby();
            }, true);

            hitObject.RomajiTagsBindable.BindValueChanged(romajiTags =>
            {
                ApplyRomaji();
            }, true);

            hitObject.FontIndexBindable.BindValueChanged(index =>
            {
                ApplySkin(CurrentSkin, false);
            }, true);

            hitObject.LayoutIndexBindable.BindValueChanged(index =>
            {
                ApplySkin(CurrentSkin, false);
            }, true);

            hitObject.TranslateTextBindable.BindValueChanged(text =>
            {
                translateText.Text = text.NewValue ?? "";
            }, true);

            LifetimeEnd = hitObject.EndTime + 1000;
        }

        protected override void ClearInternal(bool disposeChildren = true)
        {
        }

        protected virtual void ApplyRuby()
        {
            karaokeText.Rubies = DisplayRuby ? HitObject.RubyTags?.Select(x => new PositionText(x.Text, x.StartIndex, x.EndIndex)).ToArray() : null;
        }

        protected virtual void ApplyRomaji()
        {
            karaokeText.Romajies = DisplayRomaji ? HitObject.RomajiTags?.Select(x => new PositionText(x.Text, x.StartIndex, x.EndIndex)).ToArray() : null;
        }

        protected override void ApplySkin(ISkinSource skin, bool allowFallback)
        {
            base.ApplySkin(skin, allowFallback);

            if (CurrentSkin == null)
                return;

            var karaokeFont = skin.GetConfig<KaraokeSkinLookup, KaraokeFont>(new KaraokeSkinLookup(KaraokeSkinConfiguration.LyricStyle, HitObject.FontIndex))?.Value;
            if (karaokeFont != null)
                ApplyFont(karaokeFont);

            var karaokeLayout = skin.GetConfig<KaraokeSkinLookup, KaraokeLayout>(new KaraokeSkinLookup(KaraokeSkinConfiguration.LyricLayout, HitObject.LayoutIndex))?.Value;
            if (karaokeLayout != null)
                ApplyLayout(karaokeLayout);
        }

        protected virtual void ApplyFont(KaraokeFont font)
        {
            // From text sample
            karaokeText.FrontTextTexture = new SolidTexture { SolidColor = Color4.Blue }; // font.FrontTextBrushInfo.TextBrush.ConvertToTextureSample();
            karaokeText.FrontBorderTexture = font.FrontTextBrushInfo.BorderBrush.ConvertToTextureSample();
            karaokeText.FrontTextShadowTexture = font.FrontTextBrushInfo.ShadowBrush.ConvertToTextureSample();

            // Back text sample
            karaokeText.BackTextTexture = font.BackTextBrushInfo.TextBrush.ConvertToTextureSample();
            karaokeText.BackBorderTexture = font.BackTextBrushInfo.BorderBrush.ConvertToTextureSample();
            karaokeText.BackTextShadowTexture = font.BackTextBrushInfo.ShadowBrush.ConvertToTextureSample();

            // Apply text font info
            var lyricFont = font.LyricTextFontInfo.LyricTextFontInfo;
            karaokeText.Font = new FontUsage(size: lyricFont.CharSize); // TODO : FontName and Bold
            karaokeText.Border = lyricFont.EdgeSize > 0;
            karaokeText.BorderRadius = lyricFont.EdgeSize;

            var rubyFont = font.RubyTextFontInfo.LyricTextFontInfo;
            karaokeText.RubyFont = new FontUsage(size: rubyFont.CharSize); // TODO : FontName and Bold

            var romajiFont = font.RomajiTextFontInfo.LyricTextFontInfo;
            karaokeText.RomajiFont = new FontUsage(size: romajiFont.CharSize); // TODO : FontName and Bold

            // Apply shadow
            karaokeText.Shadow = font.UseShadow;
            karaokeText.ShadowOffset = font.ShadowOffset;
        }

        protected virtual void ApplyLayout(KaraokeLayout layout)
        {
            // Layout relative to parent
            Anchor = layout.Alignment;
            Origin = layout.Alignment;
            Margin = new MarginPadding
            {
                Left = layout.Alignment.HasFlag(Anchor.x0) ? layout.HorizontalMargin : 0,
                Right = layout.Alignment.HasFlag(Anchor.x2) ? layout.HorizontalMargin : 0,
                Top = layout.Alignment.HasFlag(Anchor.y0) ? layout.VerticalMargin : 0,
                Bottom = layout.Alignment.HasFlag(Anchor.y2) ? layout.VerticalMargin : 0
            };
            Padding = new MarginPadding(30);

            // Layout to text
            karaokeText.Continuous = layout.Continuous;
            karaokeText.KaraokeTextSmartHorizon = layout.SmartHorizon;
            karaokeText.Spacing = new Vector2(layout.LyricsInterval, karaokeText.Spacing.Y);

            // Ruby
            karaokeText.RubySpacing = new Vector2(layout.RubyInterval, karaokeText.RubySpacing.Y);
            karaokeText.RubyAlignment = layout.RubyAlignment;
            karaokeText.RubyMargin = layout.RubyMargin;

            // Romaji
            karaokeText.RomajiSpacing = new Vector2(layout.RomajiInterval, karaokeText.RomajiSpacing.Y);
            karaokeText.RomajiAlignment = layout.RomajiAlignment;
            karaokeText.RomajiMargin = layout.RomajiMargin;
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            var judgement = Result.Judgement as KaraokeLyricJudgement ?? throw new ArgumentNullException();
            var lyricStartOffset = timeOffset + HitObject.LyricDuration;

            if (lyricStartOffset < 0)
            {
                judgement.Time = LyricTime.NotYet;
            }
            else if (!HitObject.HitWindows.CanBeHit(lyricStartOffset) && judgement.Time != LyricTime.Available)
            {
                // Apply start hit result
                judgement.Time = LyricTime.Available;
                OnLyricStart?.Invoke(this, Result);
            }
            else if (!HitObject.HitWindows.CanBeHit(timeOffset))
            {
                judgement.Time = LyricTime.Exceed;
                // Apply end hit result
                ApplyResult(r =>
                {
                    r.Type = HitResult.Meh;
                });
            }
        }

        protected override void UpdateStateTransforms(ArmedState state)
        {
            base.UpdateStateTransforms(state);

            using (BeginDelayedSequence(HitObject.Duration, true))
            {
                const float fade_out_time = 500;
                this.FadeOut(fade_out_time);
            }
        }

        public override double LifetimeStart
        {
            get => base.LifetimeStart;
            set
            {
                base.LifetimeStart = value;
                karaokeText.LifetimeStart = value;
                translateText.LifetimeStart = value;
            }
        }

        public override double LifetimeEnd
        {
            get => base.LifetimeEnd;
            set
            {
                base.LifetimeEnd = value;
                karaokeText.LifetimeEnd = value;
                translateText.LifetimeEnd = value;
            }
        }

        private bool displayRuby;

        public bool DisplayRuby
        {
            get => displayRuby;
            set
            {
                if (displayRuby == value)
                    return;

                displayRuby = value;
                ApplyRuby();
            }
        }

        private bool displayRomaji;

        public bool DisplayRomaji
        {
            get => displayRomaji;
            set
            {
                if (displayRomaji == value)
                    return;

                displayRomaji = value;
                ApplyRomaji();
            }
        }
    }
}
