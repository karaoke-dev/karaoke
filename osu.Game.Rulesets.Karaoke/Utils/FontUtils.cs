// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class FontUtils
    {
        /// <summary>
        /// For selecting size
        /// </summary>
        /// <returns></returns>
        public static float[] DefaultFontSize()
            => new float[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

        public static float[] DefaultFontSize(float? minSize = null, float? maxSize = null)
            => DefaultFontSize().Where(x =>
            {
                if (minSize.HasValue && x < minSize.Value)
                    return false;

                if (maxSize.HasValue && x > maxSize.Value)
                    return false;

                return true;
            }).ToArray();

        /// <summary>
        /// For selecting preview size in editor.
        /// </summary>
        /// <returns></returns>
        public static float[] DefaultPreviewFontSize()
            => new float[] { 12, 14, 16, 18, 20, 22, 24, 26, 28, 32, 36, 40, 48 };

        public static string GetText(float fontSize)
            => $"{fontSize} px";
    }
}
