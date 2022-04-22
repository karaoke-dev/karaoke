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
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Karaoke.Skinning.Default;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
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
        private readonly BindableDictionary<CultureInfo, string> translateTextBindable = new();

        public event Action<DrawableLyric> OnLyricStart;
        public event Action<DrawableLyric> OnLyricEnd;

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
            translateTextBindable.BindCollectionChanged((_, _) => { applyTranslate(); });
        }

        protected override void OnApply()
        {
            base.OnApply();

            lyricPieces.Clear();
            lyricPieces.Add(new DefaultLyricPiece(HitObject));
            ApplySkin(CurrentSkin, false);

            singersBindable.BindTo(HitObject.SingersBindable);
            translateTextBindable.BindTo(HitObject.TranslateTextBindable);
        }

        protected override void OnFree()
        {
            base.OnFree();

            singersBindable.UnbindFrom(HitObject.SingersBindable);
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
            lyricConfig?.ApplyTo(this);
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
            if (timeOffset + HitObject.LyricDuration >= 0 && HitObject.HitWindows.CanBeHit(timeOffset + HitObject.LyricDuration))
            {
                // note: CheckForResult will not being triggered when roll-back the time.
                // so there's no need to consider the case while roll-back.
                OnLyricStart?.Invoke(this);
                return;
            }

            if (timeOffset >= 0 && HitObject.HitWindows.CanBeHit(timeOffset))
            {
                OnLyricEnd?.Invoke(this);

                // Apply end hit result
                ApplyResult(r => { r.Type = KaraokeLyricHitWindows.DEFAULT_HIT_RESULT; });
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
