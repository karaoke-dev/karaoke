// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics.Carets;

public abstract partial class DrawableCaret<TCaret> : DrawableCaret where TCaret : struct, ICaretPosition
{
    [Resolved]
    protected OsuColour Colours { get; private set; } = null!;

    [Resolved]
    protected IPreviewLyricPositionProvider LyricPositionProvider { get; private set; } = null!;

    protected DrawableCaret(DrawableCaretType type)
        : base(type)
    {
    }

    protected static float GetAlpha(DrawableCaretType type) =>
        type switch
        {
            DrawableCaretType.HoverCaret => 0.5f,
            DrawableCaretType.Caret => 1,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

    public sealed override void ApplyCaretPosition(ICaretPosition caret)
    {
        if (caret is not TCaret tCaret)
            throw new InvalidCastException();

        ApplyCaretPosition(tCaret);
    }

    public sealed override void TriggerDisallowEditEffect()
    {
        TriggerDisallowEditEffect(Colours);
    }

    protected abstract void ApplyCaretPosition(TCaret caret);

    protected abstract void TriggerDisallowEditEffect(OsuColour colour);
}

public abstract partial class DrawableCaret : CompositeDrawable
{
    public readonly DrawableCaretType Type;

    protected DrawableCaret(DrawableCaretType type)
    {
        Type = type;
    }

    public abstract void ApplyCaretPosition(ICaretPosition caret);

    public abstract void TriggerDisallowEditEffect();
}
