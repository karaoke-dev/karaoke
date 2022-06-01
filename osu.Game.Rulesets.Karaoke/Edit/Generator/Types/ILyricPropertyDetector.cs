// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Types
{
    /// <summary>
    /// Base interface of the detector.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILyricPropertyDetector<out T>
    {
        /// <summary>
        /// Determined if detect property from lyric is supported.
        /// </summary>
        /// <param name="lyric"></param>
        /// <returns></returns>
        bool CanDetect(Lyric lyric);

        /// <summary>
        /// Detect the property from the lyric.
        /// </summary>
        /// <param name="lyric"></param>
        /// <returns></returns>
        T Detect(Lyric lyric);
    }
}
