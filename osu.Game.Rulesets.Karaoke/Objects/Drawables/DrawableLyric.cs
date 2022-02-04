// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Judgements;
using osu.Game.Rulesets.Karaoke.Skinning.Default;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables
{
    public class DrawableLyric : DrawableKaraokeHitObject
    {
        private Container<DefaultLyricPiece> lyricPieces;
        private OsuSpriteText translateText;

        [Resolved(canBeNull: true)]
        private KaraokeRulesetConfigManager config { get; set; }

        private readonly BindableBool useTranslateBindable = new();
        private readonly Bindable<CultureInfo> preferLanguageBindable = new();
        private readonly BindableBool displayRubyBindable = new();
        private readonly BindableBool displayRomajiBindable = new();

        private readonly Bindable<FontUsage> mainFontUsageBindable = new();
        private readonly Bindable<FontUsage> rubyFontUsageBindable = new();
        private readonly Bindable<int> rubyMarginBindable = new();
        private readonly Bindable<FontUsage> romajiFontUsageBindable = new();
        private readonly Bindable<int> romajiMarginBindable = new();
        private readonly Bindable<FontUsage> translateFontUsageBindable = new();

        private readonly IBindableList<int> singersBindable = new BindableList<int>();
        private readonly IBindable<int> layoutIndexBindable = new Bindable<int>();
        private readonly BindableDictionary<CultureInfo, string> translateTextBindable = new();

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
            // todo: it's a reservable size, should be removed eventually.
            Padding = new MarginPadding(30);
        }

        [BackgroundDependencyLoader(true)]
        private void load([CanBeNull] KaraokeSessionStatics session)
        {
            Scale = new Vector2((float)(config?.Get<double>(KaraokeRulesetSetting.LyricScale) ?? 2));
            AutoSizeAxes = Axes.Both;

            AddInternal(lyricPieces = new Container<DefaultLyricPiece>
            {
                AutoSizeAxes = Axes.Both,
            });
            AddInternal(translateText = new OsuSpriteText
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.TopLeft,
            });

            if (session != null)
            {
                // gameplay.
                session.BindWith(KaraokeRulesetSession.UseTranslate, useTranslateBindable);
                session.BindWith(KaraokeRulesetSession.PreferLanguage, preferLanguageBindable);
                session.BindWith(KaraokeRulesetSession.DisplayRuby, displayRubyBindable);
                session.BindWith(KaraokeRulesetSession.DisplayRomaji, displayRomajiBindable);
            }
            else if (config != null)
            {
                // preview lyric effect.
                config.BindWith(KaraokeRulesetSetting.UseTranslate, useTranslateBindable);
                config.BindWith(KaraokeRulesetSetting.PreferLanguage, preferLanguageBindable);
                config.BindWith(KaraokeRulesetSetting.DisplayRuby, displayRubyBindable);
                config.BindWith(KaraokeRulesetSetting.DisplayRomaji, displayRomajiBindable);
            }

            useTranslateBindable.BindValueChanged(_ => applyTranslate(), true);
            preferLanguageBindable.BindValueChanged(_ => applyTranslate(), true);
            displayRubyBindable.BindValueChanged(e => lyricPieces.ForEach(x => x.DisplayRuby = e.NewValue));
            displayRomajiBindable.BindValueChanged(e => lyricPieces.ForEach(x => x.DisplayRomaji = e.NewValue));

            if (config != null)
            {
                config.BindWith(KaraokeRulesetSetting.MainFont, mainFontUsageBindable);
                config.BindWith(KaraokeRulesetSetting.RubyFont, rubyFontUsageBindable);
                config.BindWith(KaraokeRulesetSetting.RubyMargin, rubyMarginBindable);
                config.BindWith(KaraokeRulesetSetting.RomajiFont, romajiFontUsageBindable);
                config.BindWith(KaraokeRulesetSetting.RomajiMargin, romajiMarginBindable);
                config.BindWith(KaraokeRulesetSetting.TranslateFont, translateFontUsageBindable);
            }

            mainFontUsageBindable.BindValueChanged(_ => updateLyricConfig());
            rubyFontUsageBindable.BindValueChanged(_ => updateLyricConfig());
            rubyMarginBindable.BindValueChanged(_ => updateLyricConfig());
            romajiFontUsageBindable.BindValueChanged(_ => updateLyricConfig());
            romajiMarginBindable.BindValueChanged(_ => updateLyricConfig());
            translateFontUsageBindable.BindValueChanged(_ => updateLyricConfig());

            // property in hitobject.
            singersBindable.BindCollectionChanged((_, _) => { updateFontStyle(); });
            layoutIndexBindable.BindValueChanged(_ => { updateLayout(); });
            translateTextBindable.BindCollectionChanged((_, _) => { applyTranslate(); });
        }

        protected override void OnApply()
        {
            base.OnApply();

            lyricPieces.Clear();
            lyricPieces.Add(new DefaultLyricPiece(HitObject));
            ApplySkin(CurrentSkin, false);

            singersBindable.BindTo(HitObject.SingersBindable);
            layoutIndexBindable.BindTo(HitObject.LayoutIndexBindable);
            translateTextBindable.BindTo(HitObject.TranslateTextBindable);
        }

        protected override void OnFree()
        {
            base.OnFree();

            singersBindable.UnbindFrom(HitObject.SingersBindable);
            layoutIndexBindable.UnbindFrom(HitObject.LayoutIndexBindable);
            translateTextBindable.UnbindFrom(HitObject.TranslateTextBindable);
        }

        protected override void ApplySkin(ISkinSource skin, bool allowFallback)
        {
            base.ApplySkin(skin, allowFallback);

            updateFontStyle();
            updateLyricConfig();
            updateLayout();
        }

        private void updateFontStyle()
        {
            if (CurrentSkin == null)
                return;

            if (HitObject == null)
                return;

            var lyricStyle = CurrentSkin.GetConfig<Lyric, LyricStyle>(HitObject)?.Value;
            lyricStyle?.ApplyTo(this);
        }

        private void updateLyricConfig()
        {
            if (CurrentSkin == null)
                return;

            if (HitObject == null)
                return;

            var lyricConfig = CurrentSkin.GetConfig<Lyric, LyricConfig>(HitObject)?.Value;

            foreach (var lyricPiece in lyricPieces)
            {
                // Apply text font info
                var mainFont = lyricConfig?.MainTextFont;
                lyricPiece.Font = getFont(KaraokeRulesetSetting.MainFont, mainFont);

                var rubyFont = lyricConfig?.RubyTextFont;
                lyricPiece.DisplayRuby = displayRubyBindable.Value;
                lyricPiece.RubyFont = getFont(KaraokeRulesetSetting.RubyFont, rubyFont);
                lyricPiece.RubyMargin = rubyMarginBindable.Value;

                var romajiFont = lyricConfig?.RomajiTextFont;
                lyricPiece.DisplayRomaji = displayRomajiBindable.Value;
                lyricPiece.RomajiFont = getFont(KaraokeRulesetSetting.RomajiFont, romajiFont);
                lyricPiece.RomajiMargin = romajiMarginBindable.Value;

                // todo: should make some config like KaraokeRulesetSetting.RomajiFont into KaraokeRulesetSetting.DefaultFontConfig.
                if (lyricConfig == null)
                    continue;

                // Layout to text
                lyricPiece.KaraokeTextSmartHorizon = lyricConfig.SmartHorizon;
                lyricPiece.Spacing = new Vector2(lyricConfig.LyricsInterval, lyricPiece.Spacing.Y);

                // Ruby
                lyricPiece.RubySpacing = new Vector2(lyricConfig.RubyInterval, lyricPiece.RubySpacing.Y);
                lyricPiece.RubyAlignment = lyricConfig.RubyAlignment;
                lyricPiece.RubyMargin = lyricConfig.RubyMargin;

                // Romaji
                lyricPiece.RomajiSpacing = new Vector2(lyricConfig.RomajiInterval, lyricPiece.RomajiSpacing.Y);
                lyricPiece.RomajiAlignment = lyricConfig.RomajiAlignment;
                lyricPiece.RomajiMargin = lyricConfig.RomajiMargin;
            }

            // Apply translate font.
            translateText.Font = getFont(KaraokeRulesetSetting.TranslateFont);

            FontUsage getFont(KaraokeRulesetSetting setting, FontUsage? skinFont = null)
            {
                bool forceUseDefault = forceUseDefaultFont();
                var font = config?.Get<FontUsage>(setting) ?? FontUsage.Default;

                if (forceUseDefault || skinFont == null)
                    return font;

                return font.With(size: skinFont.Value.Size);

                bool forceUseDefaultFont()
                {
                    switch (setting)
                    {
                        case KaraokeRulesetSetting.MainFont:
                        case KaraokeRulesetSetting.RubyFont:
                        case KaraokeRulesetSetting.RomajiFont:
                            return config?.Get<bool>(KaraokeRulesetSetting.ForceUseDefaultFont) ?? false;

                        case KaraokeRulesetSetting.TranslateFont:
                            return config?.Get<bool>(KaraokeRulesetSetting.ForceUseDefaultTranslateFont) ?? false;

                        default:
                            throw new InvalidOperationException(nameof(setting));
                    }
                }
            }
        }

        private void updateLayout()
        {
            if (CurrentSkin == null)
                return;

            if (HitObject == null)
                return;

            var layout = CurrentSkin.GetConfig<Lyric, LyricLayout>(HitObject)?.Value;
            layout?.ApplyTo(this);
        }

        private void applyTranslate()
        {
            var language = preferLanguageBindable.Value;
            bool needTranslate = useTranslateBindable.Value;

            if (!needTranslate || language == null)
            {
                translateText.Text = (string)null;
            }
            else
            {
                if (translateTextBindable.TryGetValue(language, out string translate))
                    translateText.Text = translate;
            }
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (Result.Judgement is not KaraokeLyricJudgement judgement)
                return;

            double lyricStartOffset = timeOffset + HitObject.LyricDuration;

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

            using (BeginDelayedSequence(HitObject.Duration))
            {
                const float fade_out_time = 500;
                this.FadeOut(fade_out_time);
            }
        }

        public void ApplyToLyricPieces(Action<DefaultLyricPiece> action)
        {
            foreach (var lyricPiece in lyricPieces)
                action?.Invoke(lyricPiece);
        }

        public void ApplyToTranslateText(Action<OsuSpriteText> action)
        {
            action?.Invoke(translateText);
        }
    }
}
