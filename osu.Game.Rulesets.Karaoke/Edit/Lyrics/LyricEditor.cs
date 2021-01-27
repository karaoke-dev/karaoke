// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Skinning;
using osuTK.Input;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class LyricEditor : Container, IKeyBindingHandler<KaraokeEditAction>
    {
        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [Resolved(canBeNull: true)]
        private TimeTagManager timeTagManager { get; set; }

        [Resolved(canBeNull: true)]
        private LyricManager lyricManager { get; set; }

        [Cached]
        private LyricEditorStateManager stateManager;

        private readonly KaraokeLyricEditorSkin skin;
        private readonly DrawableLyricEditList container;

        public LyricEditor()
        {
            Child = new SkinProvidingContainer(skin = new KaraokeLyricEditorSkin())
            {
                RelativeSizeAxes = Axes.Both,
                Child = container = new DrawableLyricEditList
                {
                    RelativeSizeAxes = Axes.Both,
                }
            };

            Add(stateManager = new LyricEditorStateManager());
        }

        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
            return dependencies;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            foreach (var obj in beatmap.HitObjects)
                Schedule(() => addHitObject(obj));

            if (lyricManager != null)
                container.OnOrderChanged += lyricManager.ChangeLyricOrder;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            beatmap.HitObjectAdded += addHitObject;
            beatmap.HitObjectRemoved += removeHitObject;

            stateManager.MoveCursor(MovingCursorAction.First);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (timeTagManager == null)
                return false;

            if (stateManager.Mode != Mode.TypingMode)
                return false;

            var position = stateManager.BindableCursorPosition.Value;

            switch (e.Key)
            {
                case Key.BackSpace:
                    // delete single character.
                    var deletedSuccess = lyricManager.DeleteLyricText(position);
                    if (deletedSuccess)
                        stateManager.MoveCursor(MovingCursorAction.Left);
                    return deletedSuccess;

                default:
                    return false;
            }
        }

        public bool OnPressed(KaraokeEditAction action)
        {
            if (timeTagManager == null)
                return false;

            var isMoving = HandleMovingEvent(action);
            if (isMoving)
                return true;

            switch (stateManager.Mode)
            {
                case Mode.ViewMode:
                    return false;

                case Mode.EditMode:
                    return false;

                case Mode.TypingMode:
                    // will handle in OnKeyDown
                    return false;

                case Mode.RecordMode:
                    return HandleSetTimeEvent(action);

                case Mode.TimeTagEditMode:
                    return HandleCreateOrDeleterTimeTagEvent(action);

                default:
                    throw new IndexOutOfRangeException(nameof(stateManager.Mode));
            }
        }

        public void OnReleased(KaraokeEditAction action)
        {
        }

        protected bool HandleMovingEvent(KaraokeEditAction action)
        {
            // moving cursor action
            switch (action)
            {
                case KaraokeEditAction.Up:
                    return stateManager.MoveCursor(MovingCursorAction.Up);

                case KaraokeEditAction.Down:
                    return stateManager.MoveCursor(MovingCursorAction.Down);

                case KaraokeEditAction.Left:
                    return stateManager.MoveCursor(MovingCursorAction.Left);

                case KaraokeEditAction.Right:
                    return stateManager.MoveCursor(MovingCursorAction.Right);

                case KaraokeEditAction.First:
                    return stateManager.MoveCursor(MovingCursorAction.First);

                case KaraokeEditAction.Last:
                    return stateManager.MoveCursor(MovingCursorAction.Last);

                default:
                    return false;
            }
        }

        protected bool HandleSetTimeEvent(KaraokeEditAction action)
        {
            if (timeTagManager == null)
                return false;

            var currentTimeTag = stateManager.BindableRecordCursorPosition.Value;

            switch (action)
            {
                case KaraokeEditAction.ClearTime:
                    return timeTagManager.ClearTimeTagTime(currentTimeTag);

                case KaraokeEditAction.SetTime:
                    var setTimeSuccess = timeTagManager.SetTimeTagTime(currentTimeTag);
                    if (setTimeSuccess)
                        stateManager.MoveCursor(MovingCursorAction.Next);
                    return setTimeSuccess;

                default:
                    return false;
            }
        }

        protected bool HandleCreateOrDeleterTimeTagEvent(KaraokeEditAction action)
        {
            if (timeTagManager == null)
                return false;

            var position = stateManager.BindableCursorPosition.Value;

            switch (action)
            {
                case KaraokeEditAction.Create:
                    return timeTagManager.AddTimeTagByPosition(position);

                case KaraokeEditAction.Remove:
                    return timeTagManager.RemoveTimeTagByPosition(position);

                default:
                    return false;
            }
        }

        private void addHitObject(HitObject hitObject)
        {
            // see how `DrawableEditRulesetWrapper` do
            if (hitObject is Lyric lyric)
            {
                container.Items.Add(lyric);
            }
        }

        private void removeHitObject(HitObject hitObject)
        {
            if (!(hitObject is Lyric lyric))
                return;

            container.Items.Remove(lyric);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (beatmap == null)
                return;

            beatmap.HitObjectAdded -= addHitObject;
            beatmap.HitObjectRemoved -= removeHitObject;
        }

        public float FontSize
        {
            get => skin.FontSize;
            set => skin.FontSize = value;
        }

        public Mode Mode
        {
            get => stateManager.Mode;
            set => stateManager.SetMode(value);
        }

        public LyricFastEditMode LyricFastEditMode
        {
            get => stateManager.FastEditMode;
            set => stateManager.SetFastEditMode(value);
        }

        public RecordingMovingCursorMode RecordingMovingCursorMode
        {
            get => stateManager.RecordingMovingCursorMode;
            set => stateManager.SetRecordingMovingCursorMode(value);
        }
    }
}
