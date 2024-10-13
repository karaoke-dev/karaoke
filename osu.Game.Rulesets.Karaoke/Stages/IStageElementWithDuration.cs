// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Stages;

/// <summary>
/// A <see cref="IStageElement"/> that ends at a different time than its start time.
/// </summary>
public interface IStageElementWithDuration : IStageElement
{
    /// <summary>
    /// The time at which the <see cref="IStageElement"/> ends.
    /// This is consumed to extend the length of a stage to ensure all visuals are played to completion.
    /// </summary>
    double EndTime { get; }

    /// <summary>
    /// The time this element displays until.
    /// This is used for lifetime purposes, and includes long playing animations which don't necessarily extend
    /// a stage's play time.
    /// </summary>
    double EndTimeForDisplay { get; }

    /// <summary>
    /// The duration of the <see cref="IStageElement"/>.
    /// </summary>
    double Duration => EndTime - StartTime;
}
