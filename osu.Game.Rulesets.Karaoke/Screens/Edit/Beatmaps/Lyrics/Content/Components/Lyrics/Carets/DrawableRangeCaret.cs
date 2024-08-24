// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics.Carets;

public abstract partial class DrawableRangeCaret<TCaretPosition> : DrawableCaret, ICanAcceptRangeIndex
    where TCaretPosition : struct, IIndexCaretPosition
{
    private readonly IBindable<RangeCaretPosition?> bindableRangeCaretPosition = new Bindable<RangeCaretPosition?>();

    protected DrawableRangeCaret(DrawableCaretState state)
        : base(state)
    {
        // should auto hide the hover caret if selecting.
        bindableRangeCaretPosition.BindValueChanged(x =>
        {
            if (State != DrawableCaretState.Hover)
                return;

            if (x.NewValue != null)
                Hide();
        });
    }

    [BackgroundDependencyLoader]
    private void load(ILyricCaretState lyricCaretState)
    {
        bindableRangeCaretPosition.BindTo(lyricCaretState.BindableRangeCaretPosition);
    }

    public sealed override void ApplyCaretPosition(ICaretPosition caret)
    {
        if (caret is not TCaretPosition tCaret)
            throw new InvalidCastException();

        ApplyCaretPosition(tCaret);
    }

    public void ApplyRangeCaretPosition(RangeCaretPosition rangeCaretPosition)
    {
        ApplyRangeCaretPosition(rangeCaretPosition.GetRangeCaretPositionWithType<TCaretPosition>());
    }

    protected abstract void ApplyCaretPosition(TCaretPosition caret);

    protected abstract void ApplyRangeCaretPosition(RangeCaretPosition<TCaretPosition> caret);
}
