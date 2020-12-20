// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Skinning;
using osuTK.Input;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class LyricEditor : Container
    {
        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [Resolved(canBeNull: true)]
        private TimeTagManager timeTagManager { get; set; }

        [Resolved(canBeNull: true)]
        private LyricManager lyricManager { get; set; }

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
            // todo : might not place into here.
            dependencies.Cache(stateManager = new LyricEditorStateManager(beatmap));

            foreach (var obj in beatmap.HitObjects)
                Schedule(() => addHitObject(obj));
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            beatmap.HitObjectAdded += addHitObject;
            beatmap.HitObjectRemoved += removeHitObject;

            stateManager.MoveCursor(CursorAction.First);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (timeTagManager == null)
                return false;

            var isMoving = HandleMovingEvent(e.Key);
            if (isMoving)
                return true;

            switch (stateManager.Mode)
            {
                case Mode.ViewMode:
                    return false;

                case Mode.EditMode:
                    return false;

                case Mode.TypingMode:
                    return HandleTypingEvent(e.Key);

                case Mode.RecordMode:
                    return HandleSetTimeEvent(e.Key);

                case Mode.TimeTagEditMode:
                    return HandleCreateOrDeleterTimeTagEvent(e.Key);

                default:
                    throw new IndexOutOfRangeException(nameof(stateManager.Mode));
            }
        }

        protected bool HandleMovingEvent(Key key)
        {
            // moving cursor action
            switch (key)
            {
                case Key.Up:
                    return stateManager.MoveCursor(CursorAction.MoveUp);

                case Key.Down:
                    return stateManager.MoveCursor(CursorAction.MoveDown);

                case Key.Left:
                    return stateManager.MoveCursor(CursorAction.MoveLeft);

                case Key.Right:
                    return stateManager.MoveCursor(CursorAction.MoveRight);

                case Key.PageUp:
                    return stateManager.MoveCursor(CursorAction.First);

                case Key.PageDown:
                    return stateManager.MoveCursor(CursorAction.Last);

                default:
                    return false;
            }
        }

        protected bool HandleTypingEvent(Key key)
        {
            if (timeTagManager == null)
                return false;

            var position = stateManager.BindableCursorPosition.Value;

            switch (key)
            {
                case Key.BackSpace:
                    // delete single character.
                    var deletedSuccess = lyricManager.DeleteLyricText(position);
                    if (deletedSuccess)
                        stateManager.MoveCursor(CursorAction.MoveLeft);
                    return deletedSuccess;

                default:
                    return false;
            }
        }

        protected bool HandleSetTimeEvent(Key key)
        {
            if (timeTagManager == null)
                return false;

            var currentTimeTag = stateManager.BindableRecordCursorPosition.Value;

            switch (key)
            {
                case Key.BackSpace:
                    return timeTagManager.ClearTimeTagTime(currentTimeTag);

                case Key.Space:
                    var setTimeSuccess = timeTagManager.SetTimeTagTime(currentTimeTag);
                    if (setTimeSuccess)
                        stateManager.MoveCursor(CursorAction.MoveRight);
                    return setTimeSuccess;

                default:
                    return false;
            }
        }

        protected bool HandleCreateOrDeleterTimeTagEvent(Key key)
        {
            if (timeTagManager == null)
                return false;

            var position = stateManager.BindableCursorPosition.Value;

            switch (key)
            {
                case Key.N:
                    return timeTagManager.AddTimeTagByPosition(position);

                case Key.Delete:
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
            set => ScheduleAfterChildren(() => stateManager.SetMode(value));
        }

        public LyricFastEditMode LyricFastEditMode
        {
            get => stateManager.FastEditMode;
            set => ScheduleAfterChildren(() => stateManager.SetFastEditMode(value));
        }
    }
}
