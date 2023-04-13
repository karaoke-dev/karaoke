// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Preview;

public class PreviewStageDefinition : StageDefinition
{
    #region Playfield

    /// <summary>
    /// The <see cref="PreviewStyle"/> will use it's own blur level.
    /// </summary>
    public double BlueLevel { get; set; } = 0.5f;

    /// <summary>
    /// The <see cref="PreviewStyle"/> will use it's own dim level.
    /// </summary>
    public double DimLevel { get; set; } = 0.5f;

    #endregion

    #region Fade in/out effect

    /// <summary>
    /// Fade-in/out time for the lyric showing to the screen or hiding from the screen.
    /// </summary>
    public double FadingTime { get; set; } = 300;

    /// <summary>
    /// The offset position for the lyric showing to the screen or hiding from the screen.
    /// </summary>
    public double FadingOffsetPosition { get; set; } = 64;

    /// <summary>
    /// Fade-in easing for the lyric showing to the screen.
    /// </summary>
    public Easing FadeInEasing { get; set; } = Easing.InCirc;

    /// <summary>
    /// Fade-out easing for the lyric hiding from the screen.
    /// </summary>
    public Easing FadeOutEasing { get; set; } = Easing.InCirc;

    #endregion

    #region Active/inactive effect

    /// <summary>
    /// The alpha for the lyric that is not active.
    /// </summary>
    public float InactiveAlpha { get; set; } = 0.5f;

    /// <summary>
    /// Time for the inactive state to the active stage.
    /// </summary>
    public double ActiveTime { get; set; }

    /// <summary>
    /// The alpha easing for the inactive to the active state.
    /// </summary>
    public Easing ActiveEasing { get; set; } = Easing.InCirc;

    /// <summary>
    /// The alpha easing for the active to the inactive state.
    /// </summary>
    public Easing InactiveEasing { get; set; } = Easing.InCirc;

    #endregion

    #region Lyrics arrangement

    /// <summary>
    /// Maximum of lyrics in the stage.
    /// </summary>
    public int LinesOfLyric { get; set; } = 5;

    /// <summary>
    /// The height for the single lyric.
    /// </summary>
    public float LyricHeight { get; set; } = 64;

    /// <summary>
    /// The duration for the time moving up.
    /// </summary>
    public double LineMovingTime { get; set; } = 100;

    /// <summary>
    /// The easing for the time moving up.
    /// </summary>
    public Easing LineMovingEasing { get; set; } = Easing.InCirc;

    /// <summary>
    /// If the first lyric is moved-up, the offset for the second lyric to be moved-up.
    /// </summary>
    public double LineMovingOffset { get; set; } = 100;

    #endregion
}
