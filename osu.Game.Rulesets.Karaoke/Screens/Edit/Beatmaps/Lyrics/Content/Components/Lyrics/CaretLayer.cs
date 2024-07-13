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
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics.Carets;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;

public partial class CaretLayer : Layer
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
            updateDrawableCaret(e.NewValue, DrawableCaretState.Idle);
            updateDrawableCaret(e.NewValue, DrawableCaretState.Hover);
        }, true);

        bindableHoverCaretPosition.BindValueChanged(e =>
        {
            applyTheCaretPosition(e.NewValue, DrawableCaretState.Hover);
        }, true);

        bindableCaretPosition.BindValueChanged(e =>
        {
            applyTheCaretPosition(e.NewValue, DrawableCaretState.Idle);
        }, true);

        bindableRangeCaretPosition.BindValueChanged(e =>
        {
            applyRangeCaretPosition(e.NewValue, DrawableCaretState.Idle);
        }, true);
    }

    private void updateDrawableCaret(ICaretPositionAlgorithm? algorithm, DrawableCaretState state)
    {
        var oldCaret = getDrawableCaret(state);
        if (oldCaret != null)
            RemoveInternal(oldCaret, true);

        var caret = createCaret(algorithm, state);
        if (caret == null)
            return;

        caret.Hide();

        AddInternal(caret);

        static DrawableCaret? createCaret(ICaretPositionAlgorithm? algorithm, DrawableCaretState state) =>
            algorithm?.GetCaretPositionType() switch
            {
                Type t when t == typeof(CreateRubyTagCaretPosition) => new DrawableCreateRubyTagCaret(state),
                Type t when t == typeof(CuttingCaretPosition) => new DrawableCuttingCaret(state),
                Type t when t == typeof(RecordingTimeTagCaretPosition) => new DrawableRecordingTimeTagCaret(state),
                Type t when t == typeof(CreateRemoveTimeTagCaretPosition) => new DrawableCreateRemoveTimeTagCaret(state),
                Type t when t == typeof(TypingCaretPosition) => new DrawableTypingCaret(state),
                _ => null,
            };
    }

    private void applyTheCaretPosition(ICaretPosition? position, DrawableCaretState state)
    {
        if (position == null)
            return;

        var caret = getDrawableCaret(state);
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

    private void applyRangeCaretPosition(RangeCaretPosition? rangeCaretPosition, DrawableCaretState state)
    {
        if (rangeCaretPosition == null)
            return;

        var caret = getDrawableCaret(state);
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

    private DrawableCaret? getDrawableCaret(DrawableCaretState state)
        => InternalChildren.OfType<DrawableCaret>().FirstOrDefault(x => x.State == state);

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
