// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Screens.Edit;
using Component = osu.Framework.Graphics.Component;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

public partial class LyricCaretState : Component, ILyricCaretState
{
    public IBindable<ICaretPosition?> BindableHoverCaretPosition => bindableHoverCaretPosition;
    public IBindable<ICaretPosition?> BindableCaretPosition => bindableCaretPosition;
    public IBindable<RangeCaretPosition?> BindableRangeCaretPosition => bindableRangeCaretPosition;
    public IBindable<Lyric?> BindableFocusedLyric => bindableFocusedLyric;

    private readonly Bindable<ICaretPosition?> bindableHoverCaretPosition = new();
    private readonly Bindable<ICaretPosition?> bindableCaretPosition = new();
    private readonly Bindable<RangeCaretPosition?> bindableRangeCaretPosition = new();
    private readonly Bindable<Lyric?> bindableFocusedLyric = new();

    public IBindable<ICaretPositionAlgorithm?> BindableCaretPositionAlgorithm => bindableCaretPositionAlgorithm;

    private ICaretPositionAlgorithm? algorithm => bindableCaretPositionAlgorithm.Value;
    private readonly Bindable<ICaretPositionAlgorithm?> bindableCaretPositionAlgorithm = new();

    private readonly IBindableList<Lyric> bindableLyrics = new BindableList<Lyric>();

    private readonly IBindable<EditorModeWithEditStep> bindableModeWithEditStep = new Bindable<EditorModeWithEditStep>();

    // it might be special for create time-tag mode.
    private readonly IBindable<RubyTagEditMode> bindableRubyTagEditMode = new Bindable<RubyTagEditMode>();
    private readonly IBindable<MovingTimeTagCaretMode> bindableRecordingMovingCaretMode = new Bindable<MovingTimeTagCaretMode>();
    private readonly IBindable<bool> bindableRecordingChangeTimeWhileMovingTheCaret = new Bindable<bool>();

    [Resolved]
    private EditorBeatmap beatmap { get; set; } = null!;

    [Resolved]
    private EditorClock editorClock { get; set; } = null!;

    public LyricCaretState()
    {
        bindableLyrics.BindCollectionChanged((a, b) =>
        {
            // should reset caret position if not in the list.
            var caretLyric = BindableFocusedLyric.Value;

            // should adjust hover lyric if lyric has been deleted.
            if (caretLyric != null && !bindableLyrics.Contains(caretLyric))
            {
                // if delete the current lyric, most of cases should move up.
                MoveCaret(MovingCaretAction.PreviousLyric);
            }

            refreshAlgorithmAndCaretPosition();
        });

        bindableModeWithEditStep.BindValueChanged(e =>
        {
            // Should refresh algorithm until all component loaded.
            Schedule(refreshAlgorithmAndCaretPosition);
        });

        bindableRubyTagEditMode.BindValueChanged(_ =>
        {
            refreshAlgorithmAndCaretPosition();
        });

        bindableRecordingMovingCaretMode.BindValueChanged(_ =>
        {
            refreshAlgorithmAndCaretPosition();
        });

        bindableCaretPosition.BindValueChanged(e =>
        {
            bindableFocusedLyric.Value = e.NewValue?.Lyric;
        });

        refreshAlgorithmAndCaretPosition();

        // should move the caret to first.
        MoveCaret(MovingCaretAction.FirstLyric);
    }

    private void refreshAlgorithmAndCaretPosition()
    {
        // refresh algorithm
        bindableCaretPositionAlgorithm.Value = getAlgorithmByMode(bindableModeWithEditStep.Value);

        // refresh caret position
        var lyric = bindableCaretPosition.Value?.Lyric;
        bindableHoverCaretPosition.Value = null;
        bindableCaretPosition.Value = getCaretPosition(algorithm, lyric);
        bindableRangeCaretPosition.Value = null;

        // should update selection if selected lyric changed.
        postProcess();

        static ICaretPosition? getCaretPosition(ICaretPositionAlgorithm? algorithm, Lyric? lyric)
        {
            if (algorithm == null)
                return null;

            if (lyric == null)
                return algorithm.MoveToFirstLyric();

            return algorithm.MoveToTargetLyric(lyric);
        }
    }

    private ICaretPositionAlgorithm? getAlgorithmByMode(EditorModeWithEditStep editorModeWithEditStep)
    {
        var lyrics = bindableLyrics.ToArray();
        var mode = editorModeWithEditStep.Mode;

        return mode switch
        {
            LyricEditorMode.View => null,
            LyricEditorMode.EditText => getTextModeAlgorithm(editorModeWithEditStep.GetEditStep<TextEditStep>()),
            LyricEditorMode.EditReferenceLyric => new NavigateCaretPositionAlgorithm(lyrics),
            LyricEditorMode.EditLanguage => new ClickingCaretPositionAlgorithm(lyrics),
            LyricEditorMode.EditRuby => getRubyTagModeAlgorithm(),
            LyricEditorMode.EditTimeTag => getTimeTagModeAlgorithm(editorModeWithEditStep.GetEditStep<TimeTagEditStep>()),
            LyricEditorMode.EditRomanisation => new NavigateCaretPositionAlgorithm(lyrics),
            LyricEditorMode.EditNote => new NavigateCaretPositionAlgorithm(lyrics),
            LyricEditorMode.EditSinger => new NavigateCaretPositionAlgorithm(lyrics),
            _ => throw new InvalidOperationException(nameof(mode)),
        };

        ICaretPositionAlgorithm getTextModeAlgorithm(TextEditStep textEditMode) =>
            textEditMode switch
            {
                TextEditStep.Typing => new TypingCaretPositionAlgorithm(lyrics),
                TextEditStep.Split => new CuttingCaretPositionAlgorithm(lyrics),
                TextEditStep.Verify => new NavigateCaretPositionAlgorithm(lyrics),
                _ => throw new InvalidOperationException(nameof(textEditMode)),
            };

        ICaretPositionAlgorithm getRubyTagModeAlgorithm() =>
            bindableRubyTagEditMode.Value switch
            {
                RubyTagEditMode.Create => new CreateRubyTagCaretPositionAlgorithm(lyrics),
                RubyTagEditMode.Modify => new NavigateCaretPositionAlgorithm(lyrics),
                _ => throw new InvalidOperationException(nameof(bindableRubyTagEditMode.Value)),
            };

        ICaretPositionAlgorithm getTimeTagModeAlgorithm(TimeTagEditStep timeTagEditMode)
        {
            return timeTagEditMode switch
            {
                TimeTagEditStep.Create => new CreateRemoveTimeTagCaretPositionAlgorithm(lyrics),
                TimeTagEditStep.Recording => new RecordingTimeTagCaretPositionAlgorithm(lyrics) { Mode = bindableRecordingMovingCaretMode.Value },
                TimeTagEditStep.Adjust => new NavigateCaretPositionAlgorithm(lyrics),
                _ => throw new InvalidOperationException(nameof(timeTagEditMode)),
            };
        }
    }

    [BackgroundDependencyLoader]
    private void load(ILyricsProvider lyricsProvider,
                      ILyricEditorState state,
                      KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager,
                      IEditRubyModeState editRubyModeState)
    {
        bindableLyrics.BindTo(lyricsProvider.BindableLyrics);

        bindableModeWithEditStep.BindTo(state.BindableModeWithEditStep);

        bindableRubyTagEditMode.BindTo(editRubyModeState.BindableRubyTagEditMode);
        lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.RecordingTimeTagMovingCaretMode, bindableRecordingMovingCaretMode);
        lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.RecordingChangeTimeWhileMovingTheCaret, bindableRecordingChangeTimeWhileMovingTheCaret);
    }

    public bool MoveCaret(MovingCaretAction action)
    {
        var position = GetCaretPositionByAction(action);
        return moveCaretToTargetPosition(position);
    }

    public ICaretPosition? GetCaretPositionByAction(MovingCaretAction action)
    {
        if (algorithm == null)
            return null;

        var rangeCaretPosition = bindableRangeCaretPosition.Value;

        if (rangeCaretPosition != null)
        {
            if (algorithm is not IIndexCaretPositionAlgorithm indexCaretPositionAlgorithm)
                throw new InvalidOperationException("Must using the index caret position algorithm if has range caret position.");

            var startPosition = rangeCaretPosition.Start;
            var (smallerPosition, largerPosition) = rangeCaretPosition.GetRangeCaretPosition();

            return action switch
            {
                MovingCaretAction.PreviousLyric => algorithm.MoveToPreviousLyric(startPosition),
                MovingCaretAction.NextLyric => algorithm.MoveToNextLyric(startPosition),
                MovingCaretAction.FirstLyric => algorithm.MoveToFirstLyric(),
                MovingCaretAction.LastLyric => algorithm.MoveToLastLyric(),
                MovingCaretAction.PreviousIndex => isTypingCaret(algorithm) ? smallerPosition : performMoveToPreviousIndex(algorithm, smallerPosition),
                MovingCaretAction.NextIndex => isTypingCaret(algorithm) ? largerPosition : performMoveToNextIndex(algorithm, largerPosition),
                MovingCaretAction.FirstIndex => indexCaretPositionAlgorithm.MoveToFirstIndex(smallerPosition.Lyric),
                MovingCaretAction.LastIndex => indexCaretPositionAlgorithm.MoveToLastIndex(largerPosition.Lyric),
                _ => throw new InvalidEnumArgumentException(nameof(action)),
            };

            static bool isTypingCaret(ICaretPositionAlgorithm algorithm)
                => algorithm is TypingCaretPositionAlgorithm;
        }

        var currentPosition = bindableCaretPosition.Value;

        if (currentPosition != null)
        {
            return action switch
            {
                MovingCaretAction.PreviousLyric => algorithm.MoveToPreviousLyric(currentPosition),
                MovingCaretAction.NextLyric => algorithm.MoveToNextLyric(currentPosition),
                MovingCaretAction.FirstLyric => algorithm.MoveToFirstLyric(),
                MovingCaretAction.LastLyric => algorithm.MoveToLastLyric(),
                MovingCaretAction.PreviousIndex => performMoveToPreviousIndex(algorithm, currentPosition),
                MovingCaretAction.NextIndex => performMoveToNextIndex(algorithm, currentPosition),
                MovingCaretAction.FirstIndex => performMoveToFirstIndex(algorithm, currentPosition),
                MovingCaretAction.LastIndex => performMoveToLastIndex(algorithm, currentPosition),
                _ => throw new InvalidEnumArgumentException(nameof(action)),
            };
        }

        return null;

        static ICaretPosition? performMoveToPreviousIndex(ICaretPositionAlgorithm algorithm, ICaretPosition caretPosition) =>
            performMoveCaret(algorithm, caretPosition,
                (a, c) => a.MoveToPreviousIndex(c),
                (a, c) => a.MoveToPreviousLyric(c),
                (a, c) => a.MoveToLastIndex(c.Lyric));

        static ICaretPosition? performMoveToNextIndex(ICaretPositionAlgorithm algorithm, ICaretPosition caretPosition) =>
            performMoveCaret(algorithm, caretPosition,
                (a, c) => a.MoveToNextIndex(c),
                (a, c) => a.MoveToNextLyric(c),
                (a, c) => a.MoveToFirstIndex(c.Lyric));

        static ICaretPosition? performMoveToFirstIndex(ICaretPositionAlgorithm algorithm, ICaretPosition caretPosition) =>
            performMoveCaret(algorithm, caretPosition,
                (a, c) => a.MoveToFirstIndex(c.Lyric),
                (a, c) => a.MoveToPreviousLyric(c),
                (a, c) => a.MoveToFirstIndex(c.Lyric));

        static ICaretPosition? performMoveToLastIndex(ICaretPositionAlgorithm algorithm, ICaretPosition caretPosition) =>
            performMoveCaret(algorithm, caretPosition,
                (a, c) => a.MoveToLastIndex(c.Lyric),
                (a, c) => a.MoveToNextLyric(c),
                (a, c) => a.MoveToLastIndex(c.Lyric));

        static ICaretPosition? performMoveCaret(ICaretPositionAlgorithm algorithm, ICaretPosition caretPosition,
                                                Func<IIndexCaretPositionAlgorithm, IIndexCaretPosition, ICaretPosition?> action,
                                                Func<IIndexCaretPositionAlgorithm, ICaretPosition, ICaretPosition?> switchLyricAction,
                                                Func<IIndexCaretPositionAlgorithm, ICaretPosition, ICaretPosition?> getNewCaretAction)
        {
            if (algorithm is not IIndexCaretPositionAlgorithm indexCaretPositionAlgorithm)
                return null;

            if (caretPosition is not IIndexCaretPosition indexCaretPosition)
                throw new InvalidOperationException();

            // will have three cases in here:
            // 1. got duplicated value (means it's not valid to move left and right)
            // 2. got the same value (means it's not valid to move left and right)
            // 3. got unique value (OK to return the value)
            var movedCaretPosition = action(indexCaretPositionAlgorithm, indexCaretPosition);
            if (movedCaretPosition != null && !EqualityComparer<ICaretPosition>.Default.Equals(movedCaretPosition, caretPosition))
                return movedCaretPosition;

            // if the caret is not valid to go, then trying to find the valid caret position in the different lyric.
            var newLyricCaretPosition = switchLyricAction(indexCaretPositionAlgorithm, caretPosition);
            return newLyricCaretPosition == null ? null : getNewCaretAction(indexCaretPositionAlgorithm, newLyricCaretPosition);
        }
    }

    public bool MoveHoverCaretToTargetPosition(Lyric lyric)
    {
        var caretPosition = algorithm?.MoveToTargetLyric(lyric);
        return moveHoverCaretToTargetPosition(caretPosition);
    }

    public bool MoveHoverCaretToTargetPosition<TIndex>(Lyric lyric, TIndex index)
        where TIndex : notnull
    {
        if (algorithm is not IIndexCaretPositionAlgorithm indexCaretPositionAlgorithm)
            return false;

        var caretPosition = indexCaretPositionAlgorithm.MoveToTargetLyric(lyric, index);
        return moveHoverCaretToTargetPosition(caretPosition);
    }

    private bool moveHoverCaretToTargetPosition(ICaretPosition? position)
    {
        if (position == null)
            return false;

        bindableHoverCaretPosition.Value = position;

        return true;
    }

    public bool ConfirmHoverCaretPosition()
    {
        // place hover caret to target position.
        var position = BindableHoverCaretPosition.Value;
        return moveCaretToTargetPosition(position);
    }

    public bool ClearHoverCaretPosition()
    {
        bindableHoverCaretPosition.Value = null;

        return true;
    }

    public bool MoveCaretToTargetPosition(Lyric lyric)
    {
        var caretPosition = algorithm?.MoveToTargetLyric(lyric);
        return moveCaretToTargetPosition(caretPosition);
    }

    public bool MoveCaretToTargetPosition<TIndex>(Lyric lyric, TIndex index)
        where TIndex : notnull
    {
        if (algorithm is not IIndexCaretPositionAlgorithm indexCaretPositionAlgorithm)
            return false;

        var caretPosition = indexCaretPositionAlgorithm.MoveToTargetLyric(lyric, index);
        return moveCaretToTargetPosition(caretPosition);
    }

    public bool StartDragging()
    {
        if (!CaretDraggable)
            throw new InvalidOperationException("Should not call this method if the caret is not draggable");

        var caretPosition = bindableHoverCaretPosition.Value;
        if (caretPosition is not IIndexCaretPosition indexCaretPosition)
            throw new InvalidOperationException($"Binding caret position should have value and the type should be {typeof(IIndexCaretPosition)}.");

        return moveRangeCaretToTargetPosition(indexCaretPosition, indexCaretPosition, RangeCaretDraggingState.StartDrag);
    }

    public bool MoveDraggingCaretIndex<TIndex>(TIndex index)
        where TIndex : notnull
    {
        if (!CaretDraggable)
            throw new InvalidOperationException("Should not call this method if the caret is not draggable");

        var rangeCaretPosition = bindableRangeCaretPosition.Value;
        if (rangeCaretPosition == null)
            throw new InvalidOperationException("Binding range caret position should not be null.");

        if (algorithm is not IIndexCaretPositionAlgorithm indexCaretPositionAlgorithm)
            throw new InvalidOperationException("Algorithm should be index caret position algorithm.");

        var startCaretPosition = rangeCaretPosition.Start;
        var endCaretPosition = indexCaretPositionAlgorithm.MoveToTargetLyric(startCaretPosition.Lyric, index);
        return moveRangeCaretToTargetPosition(startCaretPosition, endCaretPosition, RangeCaretDraggingState.Dragging);
    }

    public bool EndDragging()
    {
        if (!CaretDraggable)
            throw new InvalidOperationException("Should not call this method if the caret is not draggable");

        var rangeCaretPosition = bindableRangeCaretPosition.Value;
        if (rangeCaretPosition == null)
            throw new InvalidOperationException("Binding range caret position should not be null.");

        return moveRangeCaretToTargetPosition(rangeCaretPosition.Start, rangeCaretPosition.End, RangeCaretDraggingState.EndDrag);
    }

    private bool moveCaretToTargetPosition(ICaretPosition? position)
    {
        if (position == null)
            return false;

        bindableHoverCaretPosition.Value = null;
        bindableCaretPosition.Value = position;
        bindableRangeCaretPosition.Value = null;

        postProcess();

        return true;
    }

    private bool moveRangeCaretToTargetPosition(IIndexCaretPosition startCaretPosition, IIndexCaretPosition? endCaretPosition, RangeCaretDraggingState draggingState)
    {
        if (endCaretPosition == null)
            return false;

        bindableHoverCaretPosition.Value = null;
        bindableCaretPosition.Value = null;
        bindableRangeCaretPosition.Value = new RangeCaretPosition(startCaretPosition, endCaretPosition, draggingState);

        if (draggingState == RangeCaretDraggingState.EndDrag)
            postProcess();

        return true;
    }

    public void SyncSelectedHitObjectWithCaret()
    {
        // should wait until beatmap loaded.
        Schedule(() =>
        {
            beatmap.SelectedHitObjects.Clear();

            if (bindableRangeCaretPosition.Value is RangeCaretPosition rangeCaretPosition)
            {
                var selectedLyrics = beatmap.HitObjects.OfType<Lyric>().Where(x => rangeCaretPosition.IsInRange(x));
                beatmap.SelectedHitObjects.AddRange(selectedLyrics);
            }
            else if (bindableCaretPosition.Value?.Lyric != null)
            {
                beatmap.SelectedHitObjects.Add(bindableCaretPosition.Value.Lyric);
            }
        });
    }

    public bool CaretEnabled => algorithm != null;

    public bool CaretDraggable =>
        algorithm switch
        {
            TypingCaretPositionAlgorithm => true,
            CreateRubyTagCaretPositionAlgorithm => true,
            _ => false,
        };

    private void postProcess()
    {
        SyncSelectedHitObjectWithCaret();

        var caretPosition = bindableCaretPosition.Value;
        navigateToTimeByCaretPosition(caretPosition);

        void navigateToTimeByCaretPosition(ICaretPosition? position)
        {
            if (position is not RecordingTimeTagCaretPosition recordingTimeTagCaretPosition)
                return;

            double? timeTagTime = recordingTimeTagCaretPosition.TimeTag.Time;
            if (timeTagTime.HasValue && !editorClock.IsRunning && bindableRecordingChangeTimeWhileMovingTheCaret.Value)
                editorClock.SeekSmoothlyTo(timeTagTime.Value);
        }
    }
}
