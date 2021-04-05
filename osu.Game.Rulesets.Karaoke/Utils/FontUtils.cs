// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class FontUtils
    {
        /// <summary>
        /// For seleting size
        /// </summary>
        /// <returns></returns>
        public static float[] DefaultFontSize()
            => new float[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

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
