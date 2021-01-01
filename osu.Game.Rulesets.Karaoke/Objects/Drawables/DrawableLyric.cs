// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Bindables;
using osu.Game.Rulesets.Karaoke.Judgements;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Fonts;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Layouts;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Skinning;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables
{
    public class DrawableLyric : DrawableKaraokeHitObject
    {
        protected KaraokeSpriteText KaraokeText { get; private set; }
        private OsuSpriteText translateText;

        public readonly IBindable<string> TextBindable = new Bindable<string>();
        public readonly IBindable<TimeTag[]> TimeTagsBindable = new Bindable<TimeTag[]>();
        public readonly IBindable<RubyTag[]> RubyTagsBindable = new Bindable<RubyTag[]>();
        public readonly IBindable<RomajiTag[]> RomajiTagsBindable = new Bindable<RomajiTag[]>();
        public readonly IBindable<int[]> SingersBindable = new Bindable<int[]>();
        public readonly IBindable<int> LayoutIndexBindable = new Bindable<int>();
        public readonly BindableDictionary<CultureInfo, string> TranslateTextBindable = new BindableDictionary<CultureInfo, string>();

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

            AddInternal(KaraokeText = new KaraokeSpriteText());
            AddInternal(translateText = new OsuSpriteText
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.TopLeft,
            });
            
            TextBindable.BindValueChanged(text => { KaraokeText.Text = text.NewValue; });
            TimeTagsBindable.BindValueChanged(timeTags => { KaraokeText.TimeTags = TimeTagsUtils.ToDictionary(timeTags.NewValue); });
            RubyTagsBindable.BindValueChanged(rubyTags => { ApplyRuby(); });
            RomajiTagsBindable.BindValueChanged(romajiTags => { ApplyRomaji(); });
            SingersBindable.BindValueChanged(index => { ApplySkin(CurrentSkin, false); });
            LayoutIndexBindable.BindValueChanged(index => { ApplySkin(CurrentSkin, false); });
            TranslateTextBindable.BindCollectionChanged((_, args) => { ApplyTranslate(); });
        }

        protected override void OnApply()
        {
            base.OnApply();

            TextBindable.BindTo(HitObject.TextBindable);
            TimeTagsBindable.BindTo(HitObject.TimeTagsBindable);
            RubyTagsBindable.BindTo(HitObject.RubyTagsBindable);
            RomajiTagsBindable.BindTo(HitObject.RomajiTagsBindable);
            SingersBindable.BindTo(HitObject.SingersBindable);
            LayoutIndexBindable.BindTo(HitObject.LayoutIndexBindable);
            TranslateTextBindable.BindTo(HitObject.TranslateTextBindable);
        }

        protected override void OnFree()
        {
            base.OnFree();

            TextBindable.UnbindFrom(HitObject.TextBindable);
            TimeTagsBindable.UnbindFrom(HitObject.TimeTagsBindable);
            RubyTagsBindable.UnbindFrom(HitObject.RubyTagsBindable);
            RomajiTagsBindable.UnbindFrom(HitObject.RomajiTagsBindable);
            SingersBindable.UnbindFrom(HitObject.SingersBindable);
            LayoutIndexBindable.UnbindFrom(HitObject.LayoutIndexBindable);
            TranslateTextBindable.UnbindFrom(HitObject.TranslateTextBindable);
        }

        protected virtual void ApplyRuby()
        {
            KaraokeText.Rubies = DisplayRuby ? HitObject.RubyTags?.Select(x => new PositionText(x.Text, x.StartIndex, x.EndIndex)).ToArray() : null;
        }

        protected virtual void ApplyRomaji()
        {
            KaraokeText.Romajies = DisplayRomaji ? HitObject.RomajiTags?.Select(x => new PositionText(x.Text, x.StartIndex, x.EndIndex)).ToArray() : null;
        }

        protected virtual void ApplyTranslate()
        {
            if (DisplayTranslateLanguage == null)
            {
                translateText.Text = null;
            }
            else
            {
                TranslateTextBindable.TryGetValue(DisplayTranslateLanguage, out string translate);
                translateText.Text = translate;
            }
        }

        protected override void ApplySkin(ISkinSource skin, bool allowFallback)
        {
            base.ApplySkin(skin, allowFallback);

            if (CurrentSkin == null)
                return;

            if (HitObject == null)
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
            KaraokeText.FrontTextTexture = new SolidTexture { SolidColor = Color4.Blue }; // font.FrontTextBrushInfo.TextBrush.ConvertToTextureSample();
            KaraokeText.FrontBorderTexture = font.FrontTextBrushInfo.BorderBrush.ConvertToTextureSample();
            KaraokeText.FrontTextShadowTexture = font.FrontTextBrushInfo.ShadowBrush.ConvertToTextureSample();

            // Back text sample
            KaraokeText.BackTextTexture = font.BackTextBrushInfo.TextBrush.ConvertToTextureSample();
            KaraokeText.BackBorderTexture = font.BackTextBrushInfo.BorderBrush.ConvertToTextureSample();
            KaraokeText.BackTextShadowTexture = font.BackTextBrushInfo.ShadowBrush.ConvertToTextureSample();

            // Apply text font info
            var lyricFont = font.LyricTextFontInfo.LyricTextFontInfo;
            KaraokeText.Font = new FontUsage(size: lyricFont.CharSize); // TODO : FontName and Bold
            KaraokeText.Border = lyricFont.EdgeSize > 0;
            KaraokeText.BorderRadius = lyricFont.EdgeSize;

            var rubyFont = font.RubyTextFontInfo.LyricTextFontInfo;
            KaraokeText.RubyFont = new FontUsage(size: rubyFont.CharSize); // TODO : FontName and Bold

            var romajiFont = font.RomajiTextFontInfo.LyricTextFontInfo;
            KaraokeText.RomajiFont = new FontUsage(size: romajiFont.CharSize); // TODO : FontName and Bold

            // Apply shadow
            KaraokeText.Shadow = font.UseShadow;
            KaraokeText.ShadowOffset = font.ShadowOffset;
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
            KaraokeText.Continuous = layout.Continuous;
            KaraokeText.KaraokeTextSmartHorizon = layout.SmartHorizon;
            KaraokeText.Spacing = new Vector2(layout.LyricsInterval, KaraokeText.Spacing.Y);

            // Ruby
            KaraokeText.RubySpacing = new Vector2(layout.RubyInterval, KaraokeText.RubySpacing.Y);
            KaraokeText.RubyAlignment = layout.RubyAlignment;
            KaraokeText.RubyMargin = layout.RubyMargin;

            // Romaji
            KaraokeText.RomajiSpacing = new Vector2(layout.RomajiInterval, KaraokeText.RomajiSpacing.Y);
            KaraokeText.RomajiAlignment = layout.RomajiAlignment;
            KaraokeText.RomajiMargin = layout.RomajiMargin;
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

        private bool displayRuby;

        public bool DisplayRuby
        {
            get => displayRuby;
            set
            {
                if (displayRuby == value)
                    return;

                displayRuby = value;
                Schedule(ApplyRuby);
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
                Schedule(ApplyRomaji);
            }
        }

        private CultureInfo displayTranslateLanguage;

        public CultureInfo DisplayTranslateLanguage
        {
            get => displayTranslateLanguage;
            set
            {
                if (Equals(displayTranslateLanguage, value))
                    return;

                displayTranslateLanguage = value;
                Schedule(ApplyTranslate);
            }
        }
    }
}
