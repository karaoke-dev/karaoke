// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Stages.Classic;

public class ClassicStageDefinition : StageDefinition
{
    #region Playfield

    /// <summary>
    /// The border between <see cref="Lyric"/> to the left/right side of the playfield.
    /// </summary>
    public float BorderWidth { get; set; } = 25f;

    /// <summary>
    /// The border between <see cref="Lyric"/> to the bottom side of the playfield.
    /// </summary>
    public float BorderHeight { get; set; } = 25;

    #endregion

    #region Fade in/out effect

    /// <summary>
    /// Fade-in/out time for the lyric showing to the screen or hiding from the screen.
    /// </summary>
    public double FadeInTime { get; set; } = 150;

    /// <summary>
    /// Fade-in/out time for the lyric showing to the screen or hiding from the screen.
    /// </summary>
    public double FadeOutTime { get; set; } = 150;

    /// <summary>
    /// Fade-in easing for the lyric showing to the screen.
    /// </summary>
    public Easing FadeInEasing { get; set; } = Easing.OutCirc;

    /// <summary>
    /// Fade-out easing for the lyric hiding from the screen.
    /// </summary>
    public Easing FadeOutEasing { get; set; } = Easing.OutCirc;

    #endregion

    #region Lyrics arrangement

    /// <summary>
    /// Text scale for the lyric.
    /// </summary>
    public float LyricScale { get; set; } = 2;

    /// <summary>
    /// Line height for the lyric.
    /// </summary>
    public float LineHeight { get; set; } = 72;

    /// <summary>
    /// The delay time after first lyric appear.
    /// </summary>
    public double FirstLyricStartTimeOffset { get; set; } = 1000;

    /// <summary>
    /// The delay disappear time after touch to the <see cref="Lyric"/>'s <see cref="LyricTimingInfo.StartTime"/>
    /// </summary>
    public double LyricEndTimeOffset { get; set; } = 300;

    /// <summary>
    /// The delay disappear time after touch to the last <see cref="Lyric"/>'s <see cref="LyricTimingInfo.EndTime"/>
    /// </summary>
    public double LastLyricEndTimeOffset { get; set; } = 10000;

    #endregion
}
