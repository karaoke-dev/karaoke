// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;
using Component = osu.Framework.Graphics.Component;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States
{
    public class LyricCaretState : Component, ILyricCaretState
    {
        public IBindable<ICaretPosition?> BindableHoverCaretPosition => bindableHoverCaretPosition;
        public IBindable<ICaretPosition?> BindableCaretPosition => bindableCaretPosition;
        public IBindable<Lyric?> BindableFocusedLyric => bindableFocusedLyric;

        private readonly Bindable<ICaretPosition?> bindableHoverCaretPosition = new();
        private readonly Bindable<ICaretPosition?> bindableCaretPosition = new();
        private readonly Bindable<Lyric?> bindableFocusedLyric = new();

        private ICaretPositionAlgorithm? algorithm;

        private readonly IBindableList<Lyric> bindableLyrics = new BindableList<Lyric>();

        private readonly BindableList<HitObject> selectedHitObjects = new();
        private readonly IBindable<ModeWithSubMode> bindableModeAndSubMode = new Bindable<ModeWithSubMode>();

        // it might be special for create time-tag mode.
        private readonly IBindable<CreateTimeTagEditMode> bindableCreateTimeTagEditMode = new Bindable<CreateTimeTagEditMode>();
        private readonly IBindable<MovingTimeTagCaretMode> bindableCreateMovingCaretMode = new Bindable<MovingTimeTagCaretMode>();
        private readonly IBindable<MovingTimeTagCaretMode> bindableRecordingMovingCaretMode = new Bindable<MovingTimeTagCaretMode>();
        private readonly IBindable<bool> bindableRecordingChangeTimeWhileMovingTheCaret = new Bindable<bool>();

        [Resolved, AllowNull]
        private EditorClock editorClock { get; set; }

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

            bindableModeAndSubMode.BindValueChanged(e =>
            {
                // Should refresh algorithm until all component loaded.
                Schedule(refreshAlgorithmAndCaretPosition);
            });

            bindableCreateTimeTagEditMode.BindValueChanged(_ =>
            {
                refreshAlgorithmAndCaretPosition();
            });

            bindableCreateMovingCaretMode.BindValueChanged(_ =>
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
            algorithm = getAlgorithmByMode(bindableModeAndSubMode.Value);

            // refresh caret position
            var lyric = bindableCaretPosition.Value?.Lyric;
            bindableCaretPosition.Value = getCaretPosition(algorithm, lyric);
            bindableHoverCaretPosition.Value = null;

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

        private ICaretPositionAlgorithm? getAlgorithmByMode(ModeWithSubMode modeWithSubMode)
        {
            var lyrics = bindableLyrics.ToArray();
            var mode = modeWithSubMode.Mode;
            var subMode = modeWithSubMode.SubMode;

            return mode switch
            {
                LyricEditorMode.View => null,
                LyricEditorMode.Texting => subMode is TextingEditMode textingEditMode ? getTextingmodeAlgorithm(textingEditMode) : throw new InvalidCastException(),
                LyricEditorMode.Reference => new NavigateCaretPositionAlgorithm(lyrics),
                LyricEditorMode.Language => new ClickingCaretPositionAlgorithm(lyrics),
                LyricEditorMode.EditRuby => new NavigateCaretPositionAlgorithm(lyrics),
                LyricEditorMode.EditRomaji => new NavigateCaretPositionAlgorithm(lyrics),
                LyricEditorMode.EditTimeTag => subMode is TimeTagEditMode timeTagEditMode ? getTimeTagModeAlgorithm(timeTagEditMode) : throw new InvalidCastException(),
                LyricEditorMode.EditNote => new NavigateCaretPositionAlgorithm(lyrics),
                LyricEditorMode.Singer => new NavigateCaretPositionAlgorithm(lyrics),
                _ => throw new InvalidOperationException(nameof(mode))
            };

            ICaretPositionAlgorithm getTextingmodeAlgorithm(TextingEditMode textingEditMode)
            {
                return textingEditMode switch
                {
                    TextingEditMode.Typing => new TypingCaretPositionAlgorithm(lyrics),
                    TextingEditMode.Split => new CuttingCaretPositionAlgorithm(lyrics),
                    _ => throw new InvalidOperationException(nameof(textingEditMode))
                };
            }

            ICaretPositionAlgorithm getTimeTagModeAlgorithm(TimeTagEditMode timeTagEditMode)
            {
                return timeTagEditMode switch
                {
                    TimeTagEditMode.Create => getCreateTimeTagEditModeAlgorithm(lyrics, bindableCreateTimeTagEditMode.Value, bindableCreateMovingCaretMode.Value),
                    TimeTagEditMode.Recording => new TimeTagCaretPositionAlgorithm(lyrics) { Mode = bindableRecordingMovingCaretMode.Value },
                    TimeTagEditMode.Adjust => new NavigateCaretPositionAlgorithm(lyrics),
                    _ => throw new InvalidOperationException(nameof(timeTagEditMode))
                };

                static ICaretPositionAlgorithm getCreateTimeTagEditModeAlgorithm(Lyric[] lyrics, CreateTimeTagEditMode createTimeTagEditMode, MovingTimeTagCaretMode movingTimeTagCaretMode) =>
                    createTimeTagEditMode switch
                    {
                        CreateTimeTagEditMode.Create => new TimeTagIndexCaretPositionAlgorithm(lyrics) { Mode = movingTimeTagCaretMode },
                        CreateTimeTagEditMode.Modify => new TimeTagCaretPositionAlgorithm(lyrics) { Mode = movingTimeTagCaretMode },
                        _ => throw new InvalidOperationException(nameof(createTimeTagEditMode))
                    };
            }
        }

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap beatmap, ILyricsProvider lyricsProvider, ILyricEditorState state, KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
        {
            selectedHitObjects.BindTo(beatmap.SelectedHitObjects);

            bindableLyrics.BindTo(lyricsProvider.BindableLyrics);

            bindableModeAndSubMode.BindTo(state.BindableModeAndSubMode);

            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.CreateTimeTagEditMode, bindableCreateTimeTagEditMode);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.CreateTimeTagMovingCaretMode, bindableCreateMovingCaretMode);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.RecordingTimeTagMovingCaretMode, bindableRecordingMovingCaretMode);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.RecordingChangeTimeWhileMovingTheCaret, bindableRecordingChangeTimeWhileMovingTheCaret);
        }

        public bool MoveCaret(MovingCaretAction action)
        {
            var position = GetCaretPositionByAction(action);
            if (position == null)
                return false;

            MoveCaretToTargetPosition(position);
            return true;
        }

        public ICaretPosition? GetCaretPositionByAction(MovingCaretAction action)
        {
            if (algorithm == null)
                return null;

            var currentPosition = bindableCaretPosition.Value;
            if (currentPosition == null)
                return null;

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
                _ => throw new InvalidEnumArgumentException(nameof(action))
            };

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
                // 3. got unique value(OK to return the value)
                var movedCaretPosition = action(indexCaretPositionAlgorithm, indexCaretPosition);
                if (movedCaretPosition != null && !EqualityComparer<ICaretPosition>.Default.Equals(movedCaretPosition, caretPosition))
                    return movedCaretPosition;

                // if the caret is not valid to go, then trying to find the valid caret position in the different lyric.
                var newLyricCaretPosition = switchLyricAction(indexCaretPositionAlgorithm, caretPosition);
                return newLyricCaretPosition == null ? null : getNewCaretAction(indexCaretPositionAlgorithm, newLyricCaretPosition);
            }
        }

        public void MoveCaretToTargetPosition(Lyric lyric)
        {
            if (lyric == null)
                throw new ArgumentNullException(nameof(lyric));

            var caretPosition = algorithm?.MoveToTargetLyric(lyric);
            if (caretPosition == null)
                return;

            MoveCaretToTargetPosition(caretPosition);
        }

        public void MoveCaretToTargetPosition(ICaretPosition position)
        {
            if (position == null)
                throw new ArgumentNullException(nameof(position));

            bool movable = caretPositionMovable(position);

            // stop moving the caret if forbidden by algorithm calculation.
            if (!movable)
                return;

            bindableHoverCaretPosition.Value = null;
            bindableCaretPosition.Value = position;

            postProcess();
        }

        public void MoveHoverCaretToTargetPosition(ICaretPosition position)
        {
            if (position == null)
                throw new ArgumentNullException(nameof(position));

            if (!caretPositionMovable(position))
                return;

            bindableHoverCaretPosition.Value = position;
        }

        public void ClearHoverCaretPosition()
        {
            bindableHoverCaretPosition.Value = null;
        }

        private bool caretPositionMovable(ICaretPosition position)
            => algorithm?.PositionMovable(position) ?? false;

        public void SyncSelectedHitObjectWithCaret()
        {
            selectedHitObjects.Clear();

            var lyric = bindableCaretPosition.Value?.Lyric;
            if (lyric != null)
                selectedHitObjects.Add(lyric);
        }

        public bool CaretEnabled => algorithm != null;

        private void postProcess()
        {
            SyncSelectedHitObjectWithCaret();

            var caretPosition = bindableCaretPosition.Value;
            navigateToTimeByCaretPosition(caretPosition);

            void navigateToTimeByCaretPosition(ICaretPosition? position)
            {
                if (position is not TimeTagCaretPosition timeTagCaretPosition)
                    return;

                double? timeTagTime = timeTagCaretPosition.TimeTag.Time;
                if (timeTagTime.HasValue && !editorClock.IsRunning && bindableRecordingChangeTimeWhileMovingTheCaret.Value)
                    editorClock.SeekSmoothlyTo(timeTagTime.Value);
            }
        }
    }
}
