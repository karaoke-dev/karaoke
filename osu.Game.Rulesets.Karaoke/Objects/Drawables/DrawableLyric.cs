// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

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
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables;

public partial class DrawableLyric : DrawableKaraokeHitObject
{
    private Container<DrawableKaraokeSpriteText> lyricPieces;
    private OsuSpriteText translateText;

    [Resolved(canBeNull: true)]
    private KaraokeRulesetConfigManager config { get; set; }

    private readonly BindableBool useTranslateBindable = new();
    private readonly Bindable<CultureInfo> preferLanguageBindable = new();
    private readonly Bindable<LyricDisplayType> displayTypeBindable = new();
    private readonly Bindable<LyricDisplayProperty> displayPropertyBindable = new();

    private readonly Bindable<FontUsage> mainFontUsageBindable = new();
    private readonly Bindable<FontUsage> rubyFontUsageBindable = new();
    private readonly Bindable<int> rubyMarginBindable = new();
    private readonly Bindable<FontUsage> romanisationFontUsageBindable = new();
    private readonly Bindable<int> romanisationMarginBindable = new();
    private readonly Bindable<FontUsage> translateFontUsageBindable = new();

    private readonly IBindableDictionary<Singer, SingerState[]> singersBindable = new BindableDictionary<Singer, SingerState[]>();
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
    }

    [BackgroundDependencyLoader(true)]
    private void load([CanBeNull] KaraokeSessionStatics session)
    {
        AutoSizeAxes = Axes.Both;

        AddInternal(lyricPieces = new Container<DrawableKaraokeSpriteText>
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
            session.BindWith(KaraokeRulesetSession.DisplayType, displayTypeBindable);
            session.BindWith(KaraokeRulesetSession.DisplayProperty, displayPropertyBindable);
        }
        else if (config != null)
        {
            // preview lyric effect.
            config.BindWith(KaraokeRulesetSetting.UseTranslate, useTranslateBindable);
            config.BindWith(KaraokeRulesetSetting.PreferLanguage, preferLanguageBindable);
            config.BindWith(KaraokeRulesetSetting.DisplayType, displayTypeBindable);
            config.BindWith(KaraokeRulesetSetting.DisplayProperty, displayPropertyBindable);
        }

        useTranslateBindable.BindValueChanged(_ => applyTranslate(), true);
        preferLanguageBindable.BindValueChanged(_ => applyTranslate(), true);
        displayTypeBindable.BindValueChanged(e => lyricPieces.ForEach(x => x.DisplayType = e.NewValue));
        displayPropertyBindable.BindValueChanged(e => lyricPieces.ForEach(x => x.DisplayProperty = e.NewValue));

        if (config != null)
        {
            config.BindWith(KaraokeRulesetSetting.MainFont, mainFontUsageBindable);
            config.BindWith(KaraokeRulesetSetting.RubyFont, rubyFontUsageBindable);
            config.BindWith(KaraokeRulesetSetting.RubyMargin, rubyMarginBindable);
            config.BindWith(KaraokeRulesetSetting.RomanisationFont, romanisationFontUsageBindable);
            config.BindWith(KaraokeRulesetSetting.RomanisationMargin, romanisationMarginBindable);
            config.BindWith(KaraokeRulesetSetting.TranslateFont, translateFontUsageBindable);
        }

        mainFontUsageBindable.BindValueChanged(_ => updateLyricFontInfo());
        rubyFontUsageBindable.BindValueChanged(_ => updateLyricFontInfo());
        rubyMarginBindable.BindValueChanged(_ => updateLyricFontInfo());
        romanisationFontUsageBindable.BindValueChanged(_ => updateLyricFontInfo());
        romanisationMarginBindable.BindValueChanged(_ => updateLyricFontInfo());
        translateFontUsageBindable.BindValueChanged(_ => updateLyricFontInfo());

        // property in hitobject.
        singersBindable.BindCollectionChanged((_, _) => { updateFontStyle(); });
        translateTextBindable.BindCollectionChanged((_, _) => { applyTranslate(); });
    }

    protected override void OnApply()
    {
        base.OnApply();

        lyricPieces.Clear();
        lyricPieces.Add(new DrawableKaraokeSpriteText(HitObject));
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
        updateLyricFontInfo();
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

    private void updateLyricFontInfo()
    {
        if (CurrentSkin == null)
            return;

        if (HitObject == null)
            return;

        var lyricFontInfo = CurrentSkin.GetConfig<Lyric, LyricFontInfo>(HitObject)?.Value;
        lyricFontInfo?.ApplyTo(this);
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

    public void ApplyToTranslateText(Action<OsuSpriteText> action)
    {
        action?.Invoke(translateText);
    }
}
