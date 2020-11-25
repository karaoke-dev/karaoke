// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Judgements;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Skinning;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables
{
    public class DrawableLyric : DrawableKaraokeHitObject
    {
        private KarakeSpriteText karaokeText;
        private OsuSpriteText translateText;

        public readonly IBindable<string> TextBindable = new Bindable<string>();
        public readonly IBindable<Tuple<TimeTagIndex, double?>[]> TimeTagsBindable = new Bindable<Tuple<TimeTagIndex, double?>[]>();
        public readonly IBindable<RubyTag[]> RubyTagsBindable = new Bindable<RubyTag[]>();
        public readonly IBindable<RomajiTag[]> RomajiTagsBindable = new Bindable<RomajiTag[]>();
        public readonly IBindable<int[]> SingersBindable = new Bindable<int[]>();
        public readonly IBindable<int> LayoutIndexBindable = new Bindable<int>();
        public readonly IBindable<string> TranslateTextBindable = new Bindable<string>();

        /// <summary>
        /// Invoked when a <see cref="JudgementResult"/> has been applied by this <see cref="DrawableHitObject"/> or a nested <see cref="DrawableHitObject"/>.
        /// </summary>
        public event Action<DrawableHitObject, JudgementResult> OnLyricStart;

        public new Lyric HitObject => (Lyric)base.HitObject;

        public DrawableLyric()
            : this(null)
        {
        }

        public DrawableLyric([CanBeNull] Lyric hitObject)
            : base(hitObject)
        {
        }

        [BackgroundDependencyLoader]
        private void load()
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
                }
            };

            LifetimeEnd = HitObject.EndTime + 1000;

            TextBindable.BindValueChanged(text => { karaokeText.Text = text.NewValue; });
            TimeTagsBindable.BindValueChanged(timeTags => { karaokeText.TimeTags = TimeTagsUtils.ToDictionary(timeTags.NewValue); });
            RubyTagsBindable.BindValueChanged(rubyTags => { ApplyRuby(); });
            RomajiTagsBindable.BindValueChanged(romajiTags => { ApplyRomaji(); });
            SingersBindable.BindValueChanged(index => { ApplySkin(CurrentSkin, false); });
            LayoutIndexBindable.BindValueChanged(index => { ApplySkin(CurrentSkin, false); });
            TranslateTextBindable.BindValueChanged(text => { translateText.Text = text.NewValue ?? ""; });
        }

        protected override void OnApply(HitObject hitObject)
        {
            base.OnApply(hitObject);

            TextBindable.BindTo(HitObject.TextBindable);
            TimeTagsBindable.BindTo(HitObject.TimeTagsBindable);
            RubyTagsBindable.BindTo(HitObject.RubyTagsBindable);
            RomajiTagsBindable.BindTo(HitObject.RomajiTagsBindable);
            SingersBindable.BindTo(HitObject.SingersBindable);
            LayoutIndexBindable.BindTo(HitObject.LayoutIndexBindable);
            TranslateTextBindable.BindTo(HitObject.TranslateTextBindable);
        }

        protected override void OnFree(HitObject hitObject)
        {
            base.OnFree(hitObject);

            TextBindable.UnbindFrom(HitObject.TextBindable);
            TimeTagsBindable.UnbindFrom(HitObject.TimeTagsBindable);
            RubyTagsBindable.UnbindFrom(HitObject.RubyTagsBindable);
            RomajiTagsBindable.UnbindFrom(HitObject.RomajiTagsBindable);
            SingersBindable.UnbindFrom(HitObject.SingersBindable);
            LayoutIndexBindable.UnbindFrom(HitObject.LayoutIndexBindable);
            TranslateTextBindable.UnbindFrom(HitObject.TranslateTextBindable);
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();

            // Manually set to reduce the number of future alive objects to a bare minimum.
            LifetimeStart = HitObject.StartTime - HitObject.TimePreempt;
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

            skin.GetConfig<KaraokeSkinLookup, KaraokeFont>(new KaraokeSkinLookup(KaraokeSkinConfiguration.LyricStyle, HitObject.Singers))?.BindValueChanged(karaokeFont =>
            {
                if (karaokeFont.NewValue != null)
                    ApplyFont(karaokeFont.NewValue);
            }, true);

            skin.GetConfig<KaraokeSkinLookup, KaraokeLayout>(new KaraokeSkinLookup(KaraokeSkinConfiguration.LyricLayout, HitObject.LayoutIndex))?.BindValueChanged(karaokeLayout =>
            {
                if (karaokeLayout.NewValue != null)
                    ApplyLayout(karaokeLayout.NewValue);
            }, true);
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
            var judgement = Result.Judgement as KaraokeLyricJudgement;
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
                ApplyResult(r => { r.Type = HitResult.Meh; });
            }
        }

        protected override void UpdateStartTimeStateTransforms()
        {
            base.UpdateStartTimeStateTransforms();

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
                Schedule(() => ApplyRuby());
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
                Schedule(() => ApplyRomaji());
            }
        }
    }
}
