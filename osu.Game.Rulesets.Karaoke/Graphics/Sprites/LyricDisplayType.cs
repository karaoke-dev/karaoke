// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Graphics.Sprites;

public enum LyricDisplayType
{
    /// <summary>
    /// Display the lyric as center of the text.
    /// </summary>
    /// <example>
    /// Top: <see cref="RubyTag.Text"/><br/>
    /// Center: <see cref="Lyric.Text"/><br/>
    /// Bottom: <see cref="TimeTag.RomanisedSyllable"/><br/>
    /// </example>
    Lyric,

    /// <summary>
    /// Display the romanised lyric as center of the text.
    /// </summary>
    /// <example>
    /// Top: <see cref="RubyTag.Text"/><br/>
    /// Center: <see cref="TimeTag.RomanisedSyllable"/><br/>
    /// Bottom: <see cref="Lyric.Text"/><br/>
    /// </example>
    RomanisedSyllable,
}
