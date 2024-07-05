// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using osu.Framework.Bindables;
using osu.Framework.Configuration.Tracking;
using osu.Framework.Graphics.Sprites;
using osu.Game.Configuration;
using osu.Game.Rulesets.Configuration;
using osu.Game.Rulesets.Karaoke.Bindables;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Configuration;

public class KaraokeRulesetConfigManager : RulesetConfigManager<KaraokeRulesetSetting>
{
    public KaraokeRulesetConfigManager(SettingsStore? settings, RulesetInfo ruleset, int? variant = null)
        : base(settings, ruleset, variant)
    {
    }

    protected override void InitialiseDefaults()
    {
        base.InitialiseDefaults();

        // Visual
        SetDefault(KaraokeRulesetSetting.ScrollTime, 5000.0, 1000.0, 10000.0, 100.0);
        SetDefault(KaraokeRulesetSetting.ScrollDirection, KaraokeScrollingDirection.Left);
        SetDefault(KaraokeRulesetSetting.DisplayNoteRubyText, false);
        SetDefault(KaraokeRulesetSetting.ShowCursor, true);
        SetDefault(KaraokeRulesetSetting.NoteAlpha, 1, 0.2, 1, 0.01);
        SetDefault(KaraokeRulesetSetting.LyricAlpha, 1, 0.2, 1, 0.01);

        // Translation
        SetDefault(KaraokeRulesetSetting.PreferTranslationLanguage, new CultureInfo("en-US"));

        // Pitch
        SetDefault(KaraokeRulesetSetting.OverridePitchAtGameplay, false);
        SetDefault(KaraokeRulesetSetting.Pitch, 0, -10, 10);
        SetDefault(KaraokeRulesetSetting.OverrideVocalPitchAtGameplay, false);
        SetDefault(KaraokeRulesetSetting.VocalPitch, 0, -10, 10);
        SetDefault(KaraokeRulesetSetting.OverrideScoringPitchAtGameplay, false);
        SetDefault(KaraokeRulesetSetting.ScoringPitch, 0, -10, 10);

        // Playback
        SetDefault(KaraokeRulesetSetting.OverridePlaybackSpeedAtGameplay, false);
        SetDefault(KaraokeRulesetSetting.PlaybackSpeed, 0, -10, 10);

        // Practice
        SetDefault(KaraokeRulesetSetting.PracticePreemptTime, 3000.0, 0.0, 5000.0, 100.0);

        // Device
        SetDefault(KaraokeRulesetSetting.MicrophoneDevice, string.Empty);

        // Font
        SetDefault(KaraokeRulesetSetting.MainFont, new FontUsage("Torus", 48, "Bold"), 48f, 48f);
        SetDefault(KaraokeRulesetSetting.RubyFont, new FontUsage("Torus", 20, "Bold"), 8f, 48f);
        SetDefault(KaraokeRulesetSetting.RubyMargin, 5, 0, 20);
        SetDefault(KaraokeRulesetSetting.RomanisationFont, new FontUsage("Torus", 20, "Bold"), 8f, 48f);
        SetDefault(KaraokeRulesetSetting.RomanisationMargin, 0, 0, 20);
        SetDefault(KaraokeRulesetSetting.ForceUseDefaultFont, false);
        SetDefault(KaraokeRulesetSetting.TranslationFont, new FontUsage("Torus", 18, "Bold"), 10f, 48f);
        SetDefault(KaraokeRulesetSetting.ForceUseDefaultTranslationFont, false);
        SetDefault(KaraokeRulesetSetting.NoteFont, new FontUsage("Torus", 12, "Bold"), 10f, 32f);
        SetDefault(KaraokeRulesetSetting.ForceUseDefaultNoteFont, false);
    }

    protected override void AddBindable<TBindable>(KaraokeRulesetSetting lookup, Bindable<TBindable> bindable)
    {
        switch (lookup)
        {
            case KaraokeRulesetSetting.PreferTranslationLanguage:
                // todo : need to hve a default value here because it will cause error if object is null while saving.
                base.AddBindable(lookup, new BindableCultureInfo(bindable.Value as CultureInfo));
                break;

            case KaraokeRulesetSetting.MainFont:
            case KaraokeRulesetSetting.RubyFont:
            case KaraokeRulesetSetting.RomanisationFont:
            case KaraokeRulesetSetting.TranslationFont:
            case KaraokeRulesetSetting.NoteFont:
                base.AddBindable(lookup, new BindableFontUsage(TypeUtils.ChangeType<FontUsage>(bindable.Value)));
                break;

            default:
                base.AddBindable(lookup, bindable);
                break;
        }
    }

    protected BindableFontUsage SetDefault(KaraokeRulesetSetting setting, FontUsage fontUsage, float? minFontSize = null, float? maxFontSize = null)
    {
        base.SetDefault(setting, fontUsage);

        // Should not use base.setDefault's value because it will return Bindable<FontUsage>, not BindableFontUsage
        var bindable = GetOriginalBindable<FontUsage>(setting);
        if (bindable is not BindableFontUsage bindableFontUsage)
            throw new InvalidCastException(nameof(bindable));

        // Assign size restriction in here.
        if (minFontSize.HasValue) bindableFontUsage.MinFontSize = minFontSize.Value;
        if (maxFontSize.HasValue) bindableFontUsage.MaxFontSize = maxFontSize.Value;

        return bindableFontUsage;
    }

    public override TrackedSettings CreateTrackedSettings() => new()
    {
        new TrackedSetting<double>(KaraokeRulesetSetting.ScrollTime, v => new SettingDescription(v, "Scroll Time", $"{v}ms")),
        new TrackedSetting<bool>(KaraokeRulesetSetting.DisplayNoteRubyText, b => new SettingDescription(b, "Toggle display", b ? "Show" : "Hide")),
        new TrackedSetting<bool>(KaraokeRulesetSetting.ShowCursor, b => new SettingDescription(b, "Cursor display", b ? "Show" : "Hide")),
        new TrackedSetting<CultureInfo>(KaraokeRulesetSetting.PreferTranslationLanguage, c => new SettingDescription(c, "Translation language", CultureInfoUtils.GetLanguageDisplayText(c))),
        new TrackedSetting<string>(KaraokeRulesetSetting.MicrophoneDevice, d => new SettingDescription(d, "Change to the new microphone device", d)),
    };
}

public enum KaraokeRulesetSetting
{
    // Visual
    ScrollTime,
    ScrollDirection,
    DisplayNoteRubyText,
    ShowCursor,
    NoteAlpha,
    LyricAlpha,

    // Translation
    PreferTranslationLanguage,

    // Pitch
    OverridePitchAtGameplay,
    Pitch,
    OverrideVocalPitchAtGameplay,
    VocalPitch,
    OverrideScoringPitchAtGameplay,
    ScoringPitch,

    // Playback
    OverridePlaybackSpeedAtGameplay,
    PlaybackSpeed,

    // Practice
    PracticePreemptTime,

    // Device
    MicrophoneDevice,

    // Font
    MainFont,
    RubyFont,
    RubyMargin,
    RomanisationFont,
    RomanisationMargin,
    ForceUseDefaultFont,
    TranslationFont,
    ForceUseDefaultTranslationFont,
    NoteFont,
    ForceUseDefaultNoteFont,
}
