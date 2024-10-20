// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Stages.Classic;

public class ClassicLyricLayoutCategoryGeneratorConfig : GeneratorConfig
{
    /// <summary>
    /// How may lyric can be in the stage at the same time.
    /// </summary>
    [ConfigSource("Lyric amount", "How may lyric can be in the stage at the same time.")]
    public BindableInt LyricRowAmount { get; } = new(2)
    {
        MinValue = 2,
        MaxValue = 4,
    };

    /// <summary>
    /// Should auto-create the mapping to the lyric or mapping by user.
    /// </summary>
    [ConfigSource("Apply mapping to the lyric", "Auto-apply the mapping or mapping by user.")]
    public BindableBool ApplyMappingToTheLyric { get; } = new(true);

    /// <summary>
    /// Adjust the <see cref="ClassicLyricLayout.HorizontalMargin"/> in the <see cref="ClassicLyricLayout"/>
    /// </summary>
    [ConfigSource("Horizontal margin", "The margin between lyric and the border of the playfield.")]
    public BindableFloat HorizontalMargin { get; } = new()
    {
        MinValue = 32,
        MaxValue = 100,
    };
}
