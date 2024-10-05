// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;
using osu.Game.Storyboards.Drawables;

namespace osu.Game.Rulesets.Karaoke.Stages.Commands;

public interface IStageCommand
{
    /// <summary>
    /// The start time of the stage command.
    /// </summary>
    double StartTime { get; }

    /// <summary>
    /// The end time of the stage command.
    /// </summary>
    double EndTime { get; }

    /// <summary>
    /// The name of the <see cref="Drawable"/> property affected by this stage command.
    /// Used to apply initial property values based on the list of commands given in <see cref="Drawable"/>.
    /// </summary>
    string PropertyName { get; }

    /// <summary>
    /// Sets the value of the corresponding property in <see cref="Drawable"/> to the start value of this command.
    /// </summary>
    /// <param name="d">The target drawable.</param>
    void ApplyInitialValue<TDrawable>(TDrawable d)
        where TDrawable : Drawable;

    /// <summary>
    /// Applies the transforms described by this stage command to the target drawable.
    /// </summary>
    /// <param name="d">The target drawable.</param>
    /// <returns>The sequence of transforms applied to the target drawable.</returns>
    TransformSequence<TDrawable> ApplyTransforms<TDrawable>(TDrawable d)
        where TDrawable : Drawable;
}
