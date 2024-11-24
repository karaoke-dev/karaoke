// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Stages;

public interface IStageElement
{
    double StartTime { get; }

    Drawable CreateDrawable();
}

public static class StageElementExtensions
{
    /// <summary>
    /// Returns the end time of this stage element.
    /// </summary>
    /// <remarks>
    /// This returns the <see cref="IStageElementWithDuration.EndTime"/> where available, falling back to <see cref="IStageElement.StartTime"/> otherwise.
    /// </remarks>
    /// <param name="element">The stage element.</param>
    /// <returns>The end time of this element.</returns>
    public static double GetEndTime(this IStageElement element) => (element as IStageElementWithDuration)?.EndTime ?? element.StartTime;
}
