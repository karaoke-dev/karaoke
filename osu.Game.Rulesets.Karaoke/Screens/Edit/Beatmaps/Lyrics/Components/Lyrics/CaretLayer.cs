// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics.Carets;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics;

public partial class CaretLayer : BaseLayer
{
    private readonly IBindable<ICaretPosition?> bindableHoverCaretPosition = new Bindable<ICaretPosition?>();
    private readonly IBindable<ICaretPosition?> bindableCaretPosition = new Bindable<ICaretPosition?>();
    private readonly IBindable<RangeCaretPosition?> bindableRangeCaretPosition = new Bindable<RangeCaretPosition?>();

    public CaretLayer(Lyric lyric)
        : base(lyric)
    {
        bindableHoverCaretPosition.BindValueChanged(e =>
        {
            updateCaretTypeAndPosition(e, DrawableCaretType.HoverCaret);
        }, true);

        bindableCaretPosition.BindValueChanged(e =>
        {
            updateCaretTypeAndPosition(e, DrawableCaretType.Caret);
        }, true);

        bindableRangeCaretPosition.BindValueChanged(e =>
        {
            if (e.NewValue == null)
            {
                // handle the case that clear the range caret position.
                applyTheCaretPosition(bindableCaretPosition.Value, DrawableCaretType.Caret);
            }
            else
            {
                applyRangeCaretPosition(e.NewValue, DrawableCaretType.Caret);
            }
        }, true);

        void updateCaretTypeAndPosition(ValueChangedEvent<ICaretPosition?> e, DrawableCaretType caretType)
        {
            var oldCaretPosition = e.OldValue;
            var newCaretPosition = e.NewValue;

            if (oldCaretPosition?.GetType() != newCaretPosition?.GetType())
                updateDrawableCaret(newCaretPosition, caretType);

            applyTheCaretPosition(newCaretPosition, caretType);
        }
    }

    private void updateDrawableCaret(ICaretPosition? position, DrawableCaretType type)
    {
        var oldCaret = getDrawableCaret(type);
        if (oldCaret != null)
            RemoveInternal(oldCaret, true);

        var caret = createCaret(position, type);
        if (caret == null)
            return;

        caret.Hide();

        AddInternal(caret);

        static DrawableCaret? createCaret(ICaretPosition? caretPositionAlgorithm, DrawableCaretType type) =>
            caretPositionAlgorithm switch
            {
                // cutting lyric
                CuttingCaretPosition => new DrawableCuttingCaret(type),
                // typing
                TypingCaretPosition => new DrawableTypingCaret(type),
                // creat ruby-tag
                CreateRubyTagCaretPosition => new DrawableCreateRubyTagCaretPosition(type),
                // creat time-tag
                TimeTagIndexCaretPosition => new DrawableTimeTagIndexCaret(type),
                // record time-tag
                TimeTagCaretPosition => new DrawableTimeTagCaret(type),
                _ => null,
            };
    }

    private void applyTheCaretPosition(ICaretPosition? position, DrawableCaretType type)
    {
        if (position == null)
            return;

        var caret = getDrawableCaret(type);
        if (caret == null)
            return;

        if (position.Lyric != Lyric)
        {
            caret.Hide();
            return;
        }

        caret.Show();
        caret.ApplyCaretPosition(position);
    }

    private void applyRangeCaretPosition(RangeCaretPosition? rangeCaretPosition, DrawableCaretType type)
    {
        if (rangeCaretPosition == null)
            return;

        if (rangeCaretPosition.Start.Lyric != Lyric || rangeCaretPosition.End.Lyric != Lyric)
            return;

        var caret = getDrawableCaret(type);
        if (caret is not ICanAcceptRangeIndex rangeIndexDrawableCaret)
            throw new InvalidOperationException("");

        rangeIndexDrawableCaret.ApplyRangeCaretPosition(rangeCaretPosition);
    }

    private DrawableCaret? getDrawableCaret(DrawableCaretType type)
        => InternalChildren.OfType<DrawableCaret>().FirstOrDefault(x => x.Type == type);

    [BackgroundDependencyLoader]
    private void load(ILyricCaretState lyricCaretState)
    {
        bindableHoverCaretPosition.BindTo(lyricCaretState.BindableHoverCaretPosition);
        bindableCaretPosition.BindTo(lyricCaretState.BindableCaretPosition);
        bindableRangeCaretPosition.BindTo(lyricCaretState.BindableRangeCaretPosition);
    }

    public override void UpdateDisableEditState(bool editable)
    {
        this.FadeTo(editable ? 1 : 0.7f, 100);
    }

    public override void TriggerDisallowEditEffect(LyricEditorMode editorMode)
    {
        InternalChildren.OfType<DrawableCaret>().ForEach(x => x.TriggerDisallowEditEffect());
    }
}
