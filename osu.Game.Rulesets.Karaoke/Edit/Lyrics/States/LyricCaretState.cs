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

        private readonly Bindable<ICaretPosition> bindableHoverCaretPosition = new();
        private readonly Bindable<ICaretPosition> bindableCaretPosition = new();

        private ICaretPositionAlgorithm algorithm;

        private readonly BindableList<HitObject> selectedHitObjects = new();
        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
        private readonly IBindable<MovingTimeTagCaretMode> bindableCreateMovingCaretMode = new Bindable<MovingTimeTagCaretMode>();
        private readonly IBindable<MovingTimeTagCaretMode> bindableRecordingMovingCaretMode = new Bindable<MovingTimeTagCaretMode>();

        private readonly IBindableList<Lyric> lyrics;

        public LyricCaretState(IBindableList<Lyric> lyrics)
        {
            this.lyrics = lyrics;
            this.lyrics.BindCollectionChanged((a, b) =>
            {
                // should reset caret position if not in the list.
                var caretLyric = BindableCaretPosition.Value?.Lyric;

                // should adjust hover lyric if lyric has been deleted.
                if (caretLyric != null && !lyrics.Contains(caretLyric))
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
            var mode = bindableMode.Value;
            var recordingMovingCaretMode = mode == LyricEditorMode.RecordTimeTag
                ? bindableRecordingMovingCaretMode.Value
                : bindableCreateMovingCaretMode.Value;

            // refresh algorithm
            algorithm = getAlgorithmByMode(lyrics.ToArray(), mode, recordingMovingCaretMode);

            // refresh caret position
            var lyric = bindableCaretPosition.Value?.Lyric;
            bindableCaretPosition.Value = getCaretPosition(algorithm, lyric);
            bindableHoverCaretPosition.Value = getCaretPosition(algorithm, lyric);

            // should update selection if selected lyric changed.
            updateEditorBeatmapSelectedHitObject(bindableCaretPosition.Value?.Lyric);

            static ICaretPositionAlgorithm getAlgorithmByMode(Lyric[] lyrics, LyricEditorMode lyricEditorMode, MovingTimeTagCaretMode movingTimeTagCaretMode) =>
                lyricEditorMode switch
                {
                    LyricEditorMode.Manage => new CuttingCaretPositionAlgorithm(lyrics),
                    LyricEditorMode.Typing => new TypingCaretPositionAlgorithm(lyrics),
                    LyricEditorMode.EditRuby => new NavigateCaretPositionAlgorithm(lyrics),
                    LyricEditorMode.EditRomaji => new NavigateCaretPositionAlgorithm(lyrics),
                    LyricEditorMode.CreateTimeTag => new TimeTagIndexCaretPositionAlgorithm(lyrics) { Mode = movingTimeTagCaretMode },
                    LyricEditorMode.RecordTimeTag => new TimeTagCaretPositionAlgorithm(lyrics) { Mode = movingTimeTagCaretMode },
                    LyricEditorMode.AdjustTimeTag => new NavigateCaretPositionAlgorithm(lyrics),
                    LyricEditorMode.EditNote => new NavigateCaretPositionAlgorithm(lyrics),
                    LyricEditorMode.Layout => new NavigateCaretPositionAlgorithm(lyrics),
                    LyricEditorMode.Singer => new NavigateCaretPositionAlgorithm(lyrics),
                    _ => null
                };

            static ICaretPosition getCaretPosition(ICaretPositionAlgorithm algorithm, Lyric lyric)
            {
                if (algorithm == null)
                    return null;

                if (lyric == null)
                    return algorithm.MoveToFirst();

                return algorithm.MoveToTarget(lyric);
            }
        }

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap beatmap, ILyricEditorState state, KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
        {
            selectedHitObjects.BindTo(beatmap.SelectedHitObjects);
            bindableMode.BindTo(state.BindableMode);

            bindableCreateMovingCaretMode.BindTo(lyricEditorConfigManager.GetBindable<MovingTimeTagCaretMode>(KaraokeRulesetLyricEditorSetting.CreateTimeTagMovingCaretMode));
            bindableRecordingMovingCaretMode.BindTo(lyricEditorConfigManager.GetBindable<MovingTimeTagCaretMode>(KaraokeRulesetLyricEditorSetting.RecordingTimeTagMovingCaretMode));
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

            bool hasAlgorithm = algorithm != null;
            var caretPosition = algorithm?.MoveToTarget(lyric);

            if (hasAlgorithm && caretPosition == null)
                return;

            // remain state:
            // 1. has no caret position because has no algorithm.
            // 2. has caret position.
            // should update beatmap selected object in both cases.
            updateEditorBeatmapSelectedHitObject(lyric);

            if (caretPosition == null)
                return;

            bindableHoverCaretPosition.Value = null;
            bindableCaretPosition.Value = caretPosition;
        }

        public void MoveCaretToTargetPosition(ICaretPosition position)
        {
            if (position == null)
                throw new ArgumentNullException(nameof(position));

            if (position.Lyric == null)
                return;

            bool hasAlgorithm = algorithm != null;
            bool movable = hasAlgorithm && CaretPositionMovable(position);

            // stop moving the caret if forbidden by algorithm calculation.
            if (hasAlgorithm && !movable)
                return;

            // remain state:
            // 1. cannot move because has no algorithm.
            // 2. can move the caret.
            // should update beatmap selected object in both cases.
            updateEditorBeatmapSelectedHitObject(position.Lyric);

            if (!movable)
                return;

            bindableHoverCaretPosition.Value = null;
            bindableCaretPosition.Value = position;
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
        {
            return algorithm?.PositionMovable(position) ?? false;
        }

        public bool CaretEnabled => algorithm != null;

        private void updateEditorBeatmapSelectedHitObject(HitObject hitObject)
        {
            selectedHitObjects.Clear();

            if (hitObject != null)
                selectedHitObjects.Add(hitObject);
        }
    }
}
