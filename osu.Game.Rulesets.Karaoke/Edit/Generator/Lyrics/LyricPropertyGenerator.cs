// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics
{
    /// <summary>
    /// Base interface of the auto-generator.
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <typeparam name="TConfig"></typeparam>
    public abstract class LyricPropertyGenerator<TProperty, TConfig> : PropertyGenerator<Lyric, TProperty, TConfig>
        where TConfig : GeneratorConfig, new()
    {
        protected LyricPropertyGenerator(TConfig config)
            : base(config)
        {
        }
    }
}
