// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Stages.Classic;

public class ClassicLyricTimingInfoGeneratorConfig : GeneratorConfig
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
}
