// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.ComponentModel;
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
        public IBindable<ICaretPosition> BindableHoverCaretPosition => bindableHoverCaretPosition;
        public IBindable<ICaretPosition> BindableCaretPosition => bindableCaretPosition;

        public IBindable<ICaretPositionAlgorithm> BindableCaretPositionAlgorithm => bindableCaretPositionAlgorithm;

        private readonly Bindable<ICaretPosition> bindableHoverCaretPosition = new();
        private readonly Bindable<ICaretPosition> bindableCaretPosition = new();
        private readonly Bindable<ICaretPositionAlgorithm> bindableCaretPositionAlgorithm = new();

        private ICaretPositionAlgorithm algorithm => bindableCaretPositionAlgorithm.Value;

        private readonly BindableList<HitObject> selectedHitObjects = new();
        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();

        // it might be special for create time-tag mode.
        private readonly IBindable<TimeTagEditMode> bindableTimeTagEditMode = new Bindable<TimeTagEditMode>();
        private readonly IBindable<CreateTimeTagEditMode> bindableCreateTimeTagEditMode = new Bindable<CreateTimeTagEditMode>();
        private readonly IBindable<MovingTimeTagCaretMode> bindableCreateMovingCaretMode = new Bindable<MovingTimeTagCaretMode>();
        private readonly IBindable<MovingTimeTagCaretMode> bindableRecordingMovingCaretMode = new Bindable<MovingTimeTagCaretMode>();
        private readonly IBindable<bool> bindableRecordingChangeTimeWhileMovingTheCaret = new Bindable<bool>();

        private readonly IBindableList<Lyric> bindableLyrics;

        [Resolved]
        private EditorClock editorClock { get; set; }

        public LyricCaretState(IBindableList<Lyric> bindableLyrics)
        {
            this.bindableLyrics = bindableLyrics;
            this.bindableLyrics.BindCollectionChanged((a, b) =>
            {
                // should reset caret position if not in the list.
                var caretLyric = BindableCaretPosition.Value?.Lyric;

                // should adjust hover lyric if lyric has been deleted.
                if (caretLyric != null && !bindableLyrics.Contains(caretLyric))
                {
                    // if delete the current lyric, most of cases should move up.
                    MoveCaret(MovingCaretAction.Up);
                }

                refreshAlgorithmAndCaretPosition();
            });

            bindableMode.BindValueChanged(e =>
            {
                // Should refresh algorithm until all component loaded.
                Schedule(refreshAlgorithmAndCaretPosition);
            });

            bindableTimeTagEditMode.BindValueChanged(e =>
            {
                refreshAlgorithmAndCaretPosition();
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

            refreshAlgorithmAndCaretPosition();

            // should move the caret to first.
            MoveCaret(MovingCaretAction.First);
        }

        private void refreshAlgorithmAndCaretPosition()
        {
            // refresh algorithm
            bindableCaretPositionAlgorithm.Value = getAlgorithmByMode();

            // refresh caret position
            var lyric = bindableCaretPosition.Value?.Lyric;
            bindableCaretPosition.Value = getCaretPosition(algorithm, lyric);
            bindableHoverCaretPosition.Value = null;

            // should update selection if selected lyric changed.
            postProcess();

            static ICaretPosition getCaretPosition(ICaretPositionAlgorithm algorithm, Lyric lyric)
            {
                if (algorithm == null)
                    return null;

                if (lyric == null)
                    return algorithm.MoveToFirst();

                return algorithm.MoveToTarget(lyric);
            }
        }

        private ICaretPositionAlgorithm getAlgorithmByMode()
        {
            var lyrics = bindableLyrics.ToArray();
            var lyricEditorMode = bindableMode.Value;
            return lyricEditorMode switch
            {
                LyricEditorMode.View => null,
                LyricEditorMode.Manage => new CuttingCaretPositionAlgorithm(lyrics),
                LyricEditorMode.Typing => new TypingCaretPositionAlgorithm(lyrics),
                LyricEditorMode.Language => null,
                LyricEditorMode.EditRuby => new NavigateCaretPositionAlgorithm(lyrics),
                LyricEditorMode.EditRomaji => new NavigateCaretPositionAlgorithm(lyrics),
                LyricEditorMode.EditTimeTag => getTimeTagModeAlgorithm(),
                LyricEditorMode.EditNote => new NavigateCaretPositionAlgorithm(lyrics),
                LyricEditorMode.Singer => new NavigateCaretPositionAlgorithm(lyrics),
                _ => throw new InvalidOperationException(nameof(lyricEditorMode))
            };

            ICaretPositionAlgorithm getTimeTagModeAlgorithm()
            {
                var timeTagEditMode = bindableTimeTagEditMode.Value;
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
        private void load(EditorBeatmap beatmap, ILyricEditorState state, ITimeTagModeState timeTagModeState, KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
        {
            selectedHitObjects.BindTo(beatmap.SelectedHitObjects);
            bindableMode.BindTo(state.BindableMode);

            bindableTimeTagEditMode.BindTo(timeTagModeState.BindableEditMode);

            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.CreateTimeTagEditMode, bindableCreateTimeTagEditMode);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.CreateTimeTagMovingCaretMode, bindableCreateMovingCaretMode);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.RecordingTimeTagMovingCaretMode, bindableRecordingMovingCaretMode);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.RecordingChangeTimeWhileMovingTheCaret, bindableRecordingChangeTimeWhileMovingTheCaret);
        }

        public bool MoveCaret(MovingCaretAction action)
        {
            if (algorithm == null)
                return false;

            var currentPosition = bindableCaretPosition.Value;

            var position = action switch
            {
                MovingCaretAction.Up => algorithm.MoveUp(currentPosition),
                MovingCaretAction.Down => algorithm.MoveDown(currentPosition),
                MovingCaretAction.Left => algorithm.MoveLeft(currentPosition),
                MovingCaretAction.Right => algorithm.MoveRight(currentPosition),
                MovingCaretAction.First => algorithm.MoveToFirst(),
                MovingCaretAction.Last => algorithm.MoveToLast(),
                _ => throw new InvalidEnumArgumentException(nameof(action))
            };

            if (position == null)
                return false;

            MoveCaretToTargetPosition(position);
            return true;
        }

        public void MoveCaretToTargetPosition(Lyric lyric)
        {
            if (lyric == null)
                throw new ArgumentNullException(nameof(lyric));

            var caretPosition = algorithm?.MoveToTarget(lyric);
            if (caretPosition == null)
                return;

            MoveCaretToTargetPosition(caretPosition);
        }

        public void MoveCaretToTargetPosition(ICaretPosition position)
        {
            if (position == null)
                throw new ArgumentNullException(nameof(position));

            if (position.Lyric == null)
                return;

            bool movable = CaretPositionMovable(position);

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

            if (position.Lyric == null)
                return;

            if (!CaretPositionMovable(position))
                return;

            bindableHoverCaretPosition.Value = position;
        }

        public void ClearHoverCaretPosition()
        {
            bindableHoverCaretPosition.Value = null;
        }

        public bool CaretPositionMovable(ICaretPosition position)
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

            void navigateToTimeByCaretPosition(ICaretPosition position)
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
