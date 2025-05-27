// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Localisation;
using osu.Game.Configuration;
using osu.Game.Rulesets.Karaoke.Bindables;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Mods;

public class KaraokeModTranslation : Mod, IApplicableToDrawableHitObject
{
    public override string Name => $"Translation to {CultureInfo.Value}";

    public override LocalisableString Description => "Display prefer translation by ruleset configuration.";

    public override double ScoreMultiplier => 1.0f;

    public override string Acronym => "LT";

    [SettingSource("Default language", "Select default language", 0, SettingControlType = typeof(LanguageSettingsControl))]
    public BindableCultureInfo CultureInfo { get; } = new(new CultureInfo("en-US"));

    public void ApplyToDrawableHitObject(DrawableHitObject drawable)
    {
        if (drawable is not DrawableLyric drawableLyric)
            return;

        drawableLyric.ChangePreferTranslationLanguage(CultureInfo.Value);
    }
}
