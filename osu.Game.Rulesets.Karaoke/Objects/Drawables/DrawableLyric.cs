// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Extensions.ObjectExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables;

public partial class DrawableLyric : DrawableKaraokeHitObject
{
    private Container<DrawableKaraokeSpriteText> lyricPieces = null!;
    private OsuSpriteText translationText = null!;

    private readonly BindableBool useTranslationBindable = new();
    private readonly Bindable<CultureInfo> preferLanguageBindable = new();

    private readonly Bindable<FontUsage> mainFontUsageBindable = new();
    private readonly Bindable<FontUsage> rubyFontUsageBindable = new();
    private readonly Bindable<int> rubyMarginBindable = new();
    private readonly Bindable<FontUsage> romanisationFontUsageBindable = new();
    private readonly Bindable<int> romanisationMarginBindable = new();
    private readonly Bindable<FontUsage> translationFontUsageBindable = new();

    private readonly IBindableDictionary<Singer, SingerState[]> singersBindable = new BindableDictionary<Singer, SingerState[]>();
    private readonly BindableDictionary<CultureInfo, string> translationTextBindable = new();

    public event Action<DrawableLyric>? OnLyricStart;
    public event Action<DrawableLyric>? OnLyricEnd;

    public new Lyric HitObject => (Lyric)base.HitObject;

    public DrawableLyric()
        : this(null)
    {
    }

    public DrawableLyric(Lyric? hitObject)
        : base(hitObject)
    {
    }

    [BackgroundDependencyLoader(true)]
    private void load(KaraokeRulesetConfigManager? config)
    {
        AutoSizeAxes = Axes.Both;

        AddInternal(lyricPieces = new Container<DrawableKaraokeSpriteText>
        {
            AutoSizeAxes = Axes.Both,
        });
        AddInternal(translationText = new OsuSpriteText
        {
            Anchor = Anchor.BottomLeft,
            Origin = Anchor.TopLeft,
        });

        useTranslationBindable.BindValueChanged(_ => applyTranslation(), true);
        preferLanguageBindable.BindValueChanged(_ => applyTranslation(), true);

        if (config != null)
        {
            config.BindWith(KaraokeRulesetSetting.MainFont, mainFontUsageBindable);
            config.BindWith(KaraokeRulesetSetting.RubyFont, rubyFontUsageBindable);
            config.BindWith(KaraokeRulesetSetting.RubyMargin, rubyMarginBindable);
            config.BindWith(KaraokeRulesetSetting.RomanisationFont, romanisationFontUsageBindable);
            config.BindWith(KaraokeRulesetSetting.RomanisationMargin, romanisationMarginBindable);
            config.BindWith(KaraokeRulesetSetting.TranslationFont, translationFontUsageBindable);
        }

        mainFontUsageBindable.BindValueChanged(_ => updateLyricFontInfo());
        rubyFontUsageBindable.BindValueChanged(_ => updateLyricFontInfo());
        rubyMarginBindable.BindValueChanged(_ => updateLyricFontInfo());
        romanisationFontUsageBindable.BindValueChanged(_ => updateLyricFontInfo());
        romanisationMarginBindable.BindValueChanged(_ => updateLyricFontInfo());
        translationFontUsageBindable.BindValueChanged(_ => updateLyricFontInfo());

        // property in hitobject.
        singersBindable.BindCollectionChanged((_, _) => { updateFontStyle(); });
        translationTextBindable.BindCollectionChanged((_, _) => { applyTranslation(); });
    }

    public void ChangeDisplayType(LyricDisplayType lyricDisplayType)
    {
        lyricPieces.ForEach(x => x.DisplayType = lyricDisplayType);
    }

    public void ChangeDisplayProperty(LyricDisplayProperty lyricDisplayProperty)
    {
        lyricPieces.ForEach(x => x.DisplayProperty = lyricDisplayProperty);
    }

    public void ChangePreferTranslationLanguage(CultureInfo? language)
    {
        if (language != null && translationTextBindable.TryGetValue(language, out string? translation))
            translationText.Text = translation;
        else
        {
            translationText.Text = string.Empty;
        }
    }

    protected override void OnApply()
    {
        base.OnApply();

        lyricPieces.Clear();
        lyricPieces.Add(new DrawableKaraokeSpriteText(HitObject));
        ApplySkin(CurrentSkin, false);

        singersBindable.BindTo(HitObject.SingersBindable);
        translationTextBindable.BindTo(HitObject.TranslationsBindable);
    }

    protected override void OnFree()
    {
        base.OnFree();

        singersBindable.UnbindFrom(HitObject.SingersBindable);
        translationTextBindable.UnbindFrom(HitObject.TranslationsBindable);
    }

    protected override void ApplySkin(ISkinSource skin, bool allowFallback)
    {
        base.ApplySkin(skin, allowFallback);

        updateFontStyle();
        updateLyricFontInfo();
    }

    private void updateFontStyle()
    {
        if (CurrentSkin == null)
            return;

        if (HitObject.IsNull())
            return;

        var lyricStyle = CurrentSkin.GetConfig<Lyric, LyricStyle>(HitObject)?.Value;
        lyricStyle?.ApplyTo(this);
    }

    private void updateLyricFontInfo()
    {
        if (CurrentSkin == null)
            return;

        if (HitObject.IsNull())
            return;

        var lyricFontInfo = CurrentSkin.GetConfig<Lyric, LyricFontInfo>(HitObject)?.Value;
        lyricFontInfo?.ApplyTo(this);
    }

    private void applyTranslation()
    {
        var language = preferLanguageBindable.Value;
        bool useTranslation = useTranslationBindable.Value;

        if (!useTranslation || language == null)
        {
            translationText.Text = string.Empty;
        }
        else
        {
            if (translationTextBindable.TryGetValue(language, out string? translation))
                translationText.Text = translation;
        }
    }

    protected override void CheckForResult(bool userTriggered, double timeOffset)
    {
        var timingInfo = HitObject.LyricTimingInfo;
        if (timingInfo == null)
            return;

        if (timeOffset + timingInfo.Duration >= 0 && HitObject.HitWindows.CanBeHit(timeOffset + timingInfo.Duration))
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
            ApplyResult(KaraokeLyricHitWindows.DEFAULT_HIT_RESULT);
        }
    }

    protected override void UpdateInitialTransforms()
    {
        base.UpdateInitialTransforms();

        lyricPieces.ForEach(x => x.RefreshStateTransforms());
    }

    public void ApplyToLyricPieces(Action<DrawableKaraokeSpriteText> action)
    {
        foreach (var lyricPiece in lyricPieces)
            action?.Invoke(lyricPiece);
    }

    public void ApplyToTranslationText(Action<OsuSpriteText> action)
    {
        action?.Invoke(translationText);
    }
}
