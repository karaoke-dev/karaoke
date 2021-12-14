// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.ComponentModel;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
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

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        public void ChangePositionAlgorithm(LyricEditorMode lyricEditorMode, MovingTimeTagCaretMode movingTimeTagCaretMode)
        {
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToArray();
            algorithm = getAlgorithmByMode(lyricEditorMode);

            ICaretPositionAlgorithm getAlgorithmByMode(LyricEditorMode mode) =>
                mode switch
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
        }

        public bool MoveCaret(MovingCaretAction action)
        {
            if (algorithm == null)
                return false;

            var currentPosition = bindableCaretPosition.Value;

            var position = action switch
            {
                MovingCaretAction.Up => algorithm.CallMethod<ICaretPosition, ICaretPosition>("MoveUp", currentPosition),
                MovingCaretAction.Down => algorithm.CallMethod<ICaretPosition, ICaretPosition>("MoveDown", currentPosition),
                MovingCaretAction.Left => algorithm.CallMethod<ICaretPosition, ICaretPosition>("MoveLeft", currentPosition),
                MovingCaretAction.Right => algorithm.CallMethod<ICaretPosition, ICaretPosition>("MoveRight", currentPosition),
                MovingCaretAction.First => algorithm.CallMethod<ICaretPosition>("MoveToFirst"),
                MovingCaretAction.Last => algorithm.CallMethod<ICaretPosition>("MoveToLast"),
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
            var caretPosition = algorithm?.CallMethod<ICaretPosition, Lyric>("MoveToTarget", lyric);

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

        public void ResetPosition(LyricEditorMode mode)
        {
            var lyric = bindableCaretPosition.Value?.Lyric;

            if (algorithm != null)
            {
                if (lyric != null)
                {
                    bindableCaretPosition.Value = algorithm.CallMethod<ICaretPosition, Lyric>("MoveToTarget", lyric);
                    bindableHoverCaretPosition.Value = algorithm.CallMethod<ICaretPosition, Lyric>("MoveToTarget", lyric);
                }
                else
                {
                    bindableCaretPosition.Value = algorithm.CallMethod<ICaretPosition>("MoveToFirst");
                    bindableHoverCaretPosition.Value = algorithm.CallMethod<ICaretPosition>("MoveToFirst");
                }
            }
            else
            {
                bindableCaretPosition.Value = null;
                bindableHoverCaretPosition.Value = null;
            }

            updateEditorBeatmapSelectedHitObject(lyric);
        }

        public bool CaretPositionMovable(ICaretPosition position)
        {
            return algorithm?.CallMethod<bool, ICaretPosition>("PositionMovable", position) ?? false;
        }

        public bool CaretEnabled => algorithm != null;

        private void updateEditorBeatmapSelectedHitObject(Lyric lyric)
        {
            beatmap.SelectedHitObjects.Clear();

            if (lyric != null)
                beatmap.SelectedHitObjects.Add(lyric);
        }
    }
}
