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
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Default;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Fonts;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Layouts;
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

            SingersBindable.BindValueChanged(index => { ApplySkin(CurrentSkin, false); });
            LayoutIndexBindable.BindValueChanged(index => { ApplySkin(CurrentSkin, false); });
            TranslateTextBindable.BindCollectionChanged((_, args) => { ApplyTranslate(); });
        }

        protected override void OnApply()
        {
            base.OnApply();

            lyricPieces.Clear();
            lyricPieces.Add(new DefaultLyricPiece(HitObject));

            SingersBindable.BindTo(HitObject.SingersBindable);
            LayoutIndexBindable.BindTo(HitObject.LayoutIndexBindable);
            TranslateTextBindable.BindTo(HitObject.TranslateTextBindable);
        }

        protected override void OnFree()
        {
            base.OnFree();

            SingersBindable.UnbindFrom(HitObject.SingersBindable);
            LayoutIndexBindable.UnbindFrom(HitObject.LayoutIndexBindable);
            TranslateTextBindable.UnbindFrom(HitObject.TranslateTextBindable);
        }

        protected virtual void ApplyTranslate()
        {
            if (DisplayTranslateLanguage == null)
            {
                translateText.Text = (string)null;
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

            skin.GetConfig<KaraokeSkinLookup, LyricFont>(new KaraokeSkinLookup(KaraokeSkinConfiguration.LyricStyle, HitObject.Singers))?.BindValueChanged(karaokeFont =>
            {
                if (karaokeFont.NewValue != null)
                    ApplyFont(karaokeFont.NewValue);
            }, true);

            skin.GetConfig<KaraokeSkinLookup, LyricLayout>(new KaraokeSkinLookup(KaraokeSkinConfiguration.LyricLayout, HitObject.LayoutIndex))?.BindValueChanged(karaokeLayout =>
            {
                if (karaokeLayout.NewValue != null)
                    ApplyLayout(karaokeLayout.NewValue);
            }, true);
        }

        protected virtual void ApplyFont(LyricFont font)
        {
            foreach (var lyricPiece in lyricPieces)
            {
                lyricPiece.ApplyFont(font);

                // Apply text font info
                var lyricFont = font.LyricTextFontInfo.LyricTextFontInfo;
                lyricPiece.Font = getFont(KaraokeRulesetSetting.MainFont, lyricFont);

                var rubyFont = font.RubyTextFontInfo.LyricTextFontInfo;
                lyricPiece.RubyFont = getFont(KaraokeRulesetSetting.RubyFont, rubyFont);

                var romajiFont = font.RomajiTextFontInfo.LyricTextFontInfo;
                lyricPiece.RomajiFont = getFont(KaraokeRulesetSetting.RomajiFont, romajiFont);
            }

            // Apply translate font.
            translateText.Font = getFont(KaraokeRulesetSetting.TranslateFont);

            FontUsage getFont(KaraokeRulesetSetting setting, FontUsage? skinFont = null)
            {
                var forceUseDefault = forceUseDefaultFont();
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

        protected virtual void ApplyLayout(LyricLayout layout)
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

            foreach (var lyricPiece in lyricPieces)
            {
                // Layout to text
                lyricPiece.Continuous = layout.Continuous;
                lyricPiece.KaraokeTextSmartHorizon = layout.SmartHorizon;
                lyricPiece.Spacing = new Vector2(layout.LyricsInterval, lyricPiece.Spacing.Y);

                // Ruby
                lyricPiece.RubySpacing = new Vector2(layout.RubyInterval, lyricPiece.RubySpacing.Y);
                lyricPiece.RubyAlignment = layout.RubyAlignment;
                lyricPiece.RubyMargin = layout.RubyMargin;

                // Romaji
                lyricPiece.RomajiSpacing = new Vector2(layout.RomajiInterval, lyricPiece.RomajiSpacing.Y);
                lyricPiece.RomajiAlignment = layout.RomajiAlignment;
                lyricPiece.RomajiMargin = layout.RomajiMargin;
            }
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (!(Result.Judgement is KaraokeLyricJudgement judgement))
                return;

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

            using (BeginDelayedSequence(HitObject.Duration))
            {
                const float fade_out_time = 500;
                this.FadeOut(fade_out_time);
            }
        }

        private bool displayRuby = true;

        public bool DisplayRuby
        {
            get => displayRuby;
            set
            {
                if (displayRuby == value)
                    return;

                displayRuby = value;
                Schedule(() => lyricPieces.ForEach(x => x.DisplayRuby = displayRuby));
            }
        }

        private bool displayRomaji = true;

        public bool DisplayRomaji
        {
            get => displayRomaji;
            set
            {
                if (displayRomaji == value)
                    return;

                displayRomaji = value;
                Schedule(() => lyricPieces.ForEach(x => x.DisplayRomaji = displayRomaji));
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
