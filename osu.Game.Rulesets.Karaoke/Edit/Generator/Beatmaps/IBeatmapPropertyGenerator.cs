// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps
{
    /// <summary>
    /// Base interface of the auto-generator.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBeatmapPropertyGenerator<out T>
    {
        /// <summary>
        /// Determined if detect property from <see cref="KaraokeBeatmap"/> is supported.
        /// </summary>
        /// <param name="beatmap"></param>
        /// <returns></returns>
        bool CanGenerate(KaraokeBeatmap beatmap) => GetInvalidMessage(beatmap) == null;

        /// <summary>
        /// Will get the invalid message if beatmap property is not able to be generated.
        /// </summary>
        /// <param name="beatmap"></param>
        /// <returns></returns>
        LocalisableString? GetInvalidMessage(KaraokeBeatmap beatmap);

        /// <summary>
        /// Generate the property from the beatmap.
        /// </summary>
        /// <param name="beatmap"></param>
        /// <returns></returns>
        T Generate(KaraokeBeatmap beatmap);
    }
}
