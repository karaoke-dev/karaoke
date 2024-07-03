// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics.Carets;

public abstract partial class DrawableRangeCaret<TCaretPosition> : DrawableCaret, ICanAcceptRangeIndex
    where TCaretPosition : struct, IIndexCaretPosition
{
    protected DrawableRangeCaret(DrawableCaretType type)
        : base(type)
    {
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

public abstract partial class DrawableCaret<TCaretPosition> : DrawableCaret
    where TCaretPosition : struct, ICaretPosition
{
    protected DrawableCaret(DrawableCaretType type)
        : base(type)
    {
    }

    public sealed override void ApplyCaretPosition(ICaretPosition caret)
    {
        if (caret is not TCaretPosition tCaret)
            throw new InvalidCastException();

        ApplyCaretPosition(tCaret);
    }

    protected abstract void ApplyCaretPosition(TCaretPosition caret);
}

public abstract partial class DrawableCaret : CompositeDrawable
{
    [Resolved]
    protected OsuColour Colours { get; private set; } = null!;

    [Resolved]
    protected IPreviewLyricPositionProvider LyricPositionProvider { get; private set; } = null!;

    public readonly DrawableCaretType Type;

    protected DrawableCaret(DrawableCaretType type)
    {
        Type = type;
    }

    protected static float GetAlpha(DrawableCaretType type) =>
        type switch
        {
            DrawableCaretType.HoverCaret => 0.5f,
            DrawableCaretType.Caret => 1,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };

    public abstract void ApplyCaretPosition(ICaretPosition caret);

    public void TriggerDisallowEditEffect()
    {
        TriggerDisallowEditEffect(Colours);
    }

    protected abstract void TriggerDisallowEditEffect(OsuColour colour);
}
