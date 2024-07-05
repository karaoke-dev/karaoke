// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Mods;

public class KaraokeModTranslation : Mod, IApplicableToDrawableHitObject
{
    public override string Name => "Translation";

    public override LocalisableString Description => "Display prefer translation by ruleset configuration.";

    public override double ScoreMultiplier => 1.0f;

    public override string Acronym => "LT";

    public void ApplyToDrawableHitObject(DrawableHitObject drawable)
    {
        if (drawable is not DrawableLyric drawableLyric)
            return;

        var preferLanguage = getPreferLanguage(drawableLyric.Dependencies);
        drawableLyric.ChangePreferTranslationLanguage(preferLanguage);
        return;

        static CultureInfo? getPreferLanguage(IReadOnlyDependencyContainer dependencyContainer)
        {
            var config = dependencyContainer.Get<KaraokeRulesetConfigManager>();
            return config.Get<CultureInfo?>(KaraokeRulesetSetting.PreferTranslationLanguage);
        }
    }
}
