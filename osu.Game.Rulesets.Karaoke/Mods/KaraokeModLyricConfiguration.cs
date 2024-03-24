// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Game.Configuration;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Mods;

public class KaraokeModLyricConfiguration : Mod, IApplicableToDrawableHitObject
{
    public override string Name => "Adjust Lyric Display";
    public override LocalisableString Description => "Determined display lyric or romanisation, show the ruby, translation or not.";
    public override double ScoreMultiplier => 1.0f;
    public override string Acronym => "AL";

    [SettingSource("Display type", "Display the lyric or romanisation as main text.", 0)]
    public Bindable<LyricDisplayType> DisplayType { get; } = new();

    [SettingSource("Display property", "Display the top text or bottom text.", 1)]
    public Bindable<LyricDisplayProperty> DisplayProperty { get; } = new(LyricDisplayProperty.Both);

    public void ApplyToDrawableHitObject(DrawableHitObject drawable)
    {
        if (drawable is not DrawableLyric drawableLyric)
            return;

        drawableLyric.ChangeDisplayType(DisplayType.Value);
        drawableLyric.ChangeDisplayProperty(DisplayProperty.Value);
    }
}
