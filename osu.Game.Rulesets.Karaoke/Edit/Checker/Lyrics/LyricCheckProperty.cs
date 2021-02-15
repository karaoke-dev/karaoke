// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Edit.Checker.Lyrics
{
    public enum LyricCheckProperty
    {
        /// <summary>
        /// Check start time and end time is valid.
        /// </summary>
        Time = 1,

        /// <summary>
        /// Check tim-tag is overlaggping or not.
        /// </summary>
        TimeTag = 2,

        /// <summary>
        /// Check ruby is out of range or overlapping.
        /// </summary>
        Ruby = 4,

        /// <summary>
        /// Check ropmaji is out of range or overlapping.
        /// </summary>
        Romaji = 8,

        /// <summary>
        /// Check all property in lyric.
        /// </summary>
        All = Time | TimeTag | Ruby | Romaji,
    }
}
