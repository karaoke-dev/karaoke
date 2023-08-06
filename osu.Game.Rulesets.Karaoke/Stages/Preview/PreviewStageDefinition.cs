// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Stages.Preview;

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
    public float FadingOffsetPosition { get; set; } = 64;

    /// <summary>
    /// Fade-in easing for the lyric showing to the screen.
    /// </summary>
    public Easing FadeInEasing { get; set; } = Easing.OutCirc;

    /// <summary>
    /// Fade-out easing for the lyric hiding from the screen.
    /// </summary>
    public Easing FadeOutEasing { get; set; } = Easing.OutCirc;

    /// <summary>
    /// Easing for the lyric move to the start position.
    /// </summary>
    public Easing MovingInEasing { get; set; } = Easing.OutCirc;

    /// <summary>
    /// Easing for the lyric move out to the end position.
    /// </summary>
    public Easing MoveOutEasing { get; set; } = Easing.OutCirc;

    #endregion

    #region Active/inactive effect

    /// <summary>
    /// The alpha for the lyric that is not active.
    /// </summary>
    public float InactiveAlpha { get; set; } = 0.3f;

    /// <summary>
    /// Time for the inactive state to the active stage.
    /// </summary>
    public double ActiveTime { get; set; } = 350;

    /// <summary>
    /// The alpha easing for the inactive to the active state.
    /// </summary>
    public Easing ActiveEasing { get; set; } = Easing.OutCirc;

    #endregion

    #region Lyrics arrangement

    /// <summary>
    /// Maximum of lyrics in the stage.
    /// </summary>
    public int NumberOfLyrics { get; set; } = 5;

    /// <summary>
    /// The height for the single lyric.
    /// </summary>
    public float LyricHeight { get; set; } = 64;

    /// <summary>
    /// The duration for the time moving up.
    /// </summary>
    public double LineMovingTime { get; set; } = 500;

    /// <summary>
    /// The easing for the time moving up.
    /// </summary>
    public Easing LineMovingEasing { get; set; } = Easing.OutCirc;

    /// <summary>
    /// If the first lyric is moved-up, the offset for the second lyric to be moved-up.
    /// </summary>
    public double LineMovingOffsetTime { get; set; } = 50;

    #endregion
}
