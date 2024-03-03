// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Graphics.Sprites;

public enum LyricDisplayProperty
{
    /// <summary>
    /// Display main text only.
    /// </summary>
    None = 1 << 0,

    /// <summary>
    /// Display <see cref="RubyTag.Text"/> as top text.
    /// </summary>
    TopText = 1 << 1,

    /// <summary>
    /// Display bottom text.
    /// </summary>
    /// <example>
    /// Display the <see cref="TimeTag.RomanisedSyllable"/> as bottom text if <see cref="LyricDisplayType.Lyric"/>.<br/>
    /// Display the <see cref="Lyric.Text"/> as bottom text if <see cref="LyricDisplayType.RomanisedSyllable"/>.<br/>
    /// </example>
    BottomText = 1 << 2,

    /// <summary>
    /// Display both top and bottom text.
    /// </summary>
    Both = TopText | BottomText,
}
