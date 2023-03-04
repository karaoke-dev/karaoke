// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics
{
    /// <summary>
    /// Base interface of the detector.
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <typeparam name="TConfig"></typeparam>
    public abstract class LyricPropertyDetector<TProperty, TConfig> : PropertyDetector<Lyric, TProperty, TConfig>
        where TConfig : IHasConfig, new()
    {
        protected LyricPropertyDetector(TConfig config)
            : base(config)
        {
        }
    }
}
