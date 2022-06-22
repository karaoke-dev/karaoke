// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Localisation;
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
        bool CanDetect(Lyric lyric) => GetInvalidMessage(lyric) == null;

        /// <summary>
        /// Will get the invalid message if lyric property is not able to be detected.
        /// </summary>
        /// <param name="lyric"></param>
        /// <returns></returns>
        LocalisableString? GetInvalidMessage(Lyric lyric);

        /// <summary>
        /// Detect the property from the lyric.
        /// </summary>
        /// <param name="lyric"></param>
        /// <returns></returns>
        T Detect(Lyric lyric);
    }
}
