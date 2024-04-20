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
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics.Carets;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics;

public partial class CaretLayer : BaseLayer
{
    private readonly IBindable<ICaretPositionAlgorithm?> bindableCaretPositionAlgorithm = new Bindable<ICaretPositionAlgorithm?>();

    private readonly IBindable<ICaretPosition?> bindableHoverCaretPosition = new Bindable<ICaretPosition?>();
    private readonly IBindable<ICaretPosition?> bindableCaretPosition = new Bindable<ICaretPosition?>();
    private readonly IBindable<RangeCaretPosition?> bindableRangeCaretPosition = new Bindable<RangeCaretPosition?>();

    public CaretLayer(Lyric lyric)
        : base(lyric)
    {
        bindableCaretPositionAlgorithm.BindValueChanged(e =>
        {
            updateDrawableCaret(e.NewValue, DrawableCaretType.HoverCaret);
            updateDrawableCaret(e.NewValue, DrawableCaretType.Caret);
        }, true);

        bindableHoverCaretPosition.BindValueChanged(e =>
        {
            applyTheCaretPosition(e.NewValue, DrawableCaretType.HoverCaret);
        }, true);

        bindableCaretPosition.BindValueChanged(e =>
        {
            applyTheCaretPosition(e.NewValue, DrawableCaretType.Caret);
        }, true);

        bindableRangeCaretPosition.BindValueChanged(e =>
        {
            applyRangeCaretPosition(e.NewValue, DrawableCaretType.Caret);
        }, true);
    }

    private void updateDrawableCaret(ICaretPositionAlgorithm? algorithm, DrawableCaretType type)
    {
        var oldCaret = getDrawableCaret(type);
        if (oldCaret != null)
            RemoveInternal(oldCaret, true);

        var caret = createCaret(algorithm, type);
        if (caret == null)
            return;

        caret.Hide();

        AddInternal(caret);

        static DrawableCaret? createCaret(ICaretPositionAlgorithm? algorithm, DrawableCaretType type) =>
            algorithm?.GetCaretPositionType() switch
            {
                Type t when t == typeof(CreateRubyTagCaretPosition) => new DrawableCreateRubyTagCaret(type),
                Type t when t == typeof(CuttingCaretPosition) => new DrawableCuttingCaret(type),
                Type t when t == typeof(TimeTagCaretPosition) => new DrawableTimeTagCaret(type),
                Type t when t == typeof(TimeTagIndexCaretPosition) => new DrawableTimeTagIndexCaret(type),
                Type t when t == typeof(TypingCaretPosition) => new DrawableTypingCaret(type),
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

        var caret = getDrawableCaret(type);
        if (caret == null)
            throw new InvalidOperationException("Should be able to get the drawable caret.");

        if (rangeCaretPosition.Start.Lyric != Lyric || rangeCaretPosition.End.Lyric != Lyric)
        {
            caret.Hide();
            return;
        }

        caret.Show();

        if (caret is not ICanAcceptRangeIndex rangeIndexDrawableCaret)
            throw new InvalidOperationException("Caret should be able to accept range index.");

        rangeIndexDrawableCaret.ApplyRangeCaretPosition(rangeCaretPosition);
    }

    private DrawableCaret? getDrawableCaret(DrawableCaretType type)
        => InternalChildren.OfType<DrawableCaret>().FirstOrDefault(x => x.Type == type);

    [BackgroundDependencyLoader]
    private void load(ILyricCaretState lyricCaretState)
    {
        bindableCaretPositionAlgorithm.BindTo(lyricCaretState.BindableCaretPositionAlgorithm);

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
