// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

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

        private LyricEditorStateManager lyricEditorStateManager;

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
            dependencies.Cache(lyricEditorStateManager = new LyricEditorStateManager(beatmap));

            foreach (var obj in beatmap.HitObjects)
                Schedule(() => addHitObject(obj));
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            beatmap.HitObjectAdded += addHitObject;
            beatmap.HitObjectRemoved += removeHitObject;

            lyricEditorStateManager.MoveCursor(CursorAction.First);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (timeTagManager == null)
                return false;

            // moving cursor action
            switch (e.Key)
            {
                case Key.Up:
                    return lyricEditorStateManager.MoveCursor(CursorAction.MoveUp);

                case Key.Down:
                    return lyricEditorStateManager.MoveCursor(CursorAction.MoveDown);

                case Key.Left:
                    return lyricEditorStateManager.MoveCursor(CursorAction.MoveLeft);

                case Key.Right:
                    return lyricEditorStateManager.MoveCursor(CursorAction.MoveRight);

                case Key.PageUp:
                    return lyricEditorStateManager.MoveCursor(CursorAction.First);

                case Key.PageDown:
                    return lyricEditorStateManager.MoveCursor(CursorAction.Last);
            }

            // edit time tag action
            var currentTimeTag = lyricEditorStateManager.BindableRecordCursorPosition.Value;

            switch (e.Key)
            {
                case Key.BackSpace:
                    return (bool)timeTagManager?.ClearTimeTagTime(currentTimeTag);

                case Key.Space:
                    var setTimeSuccess = (bool)timeTagManager?.SetTimeTagTime(currentTimeTag);
                    if (setTimeSuccess)
                        lyricEditorStateManager.MoveCursor(CursorAction.MoveRight);
                    return setTimeSuccess;

                case Key.N:
                    var createdTimeTag = timeTagManager?.AddTimeTag(currentTimeTag);
                    if (createdTimeTag != null)
                        lyricEditorStateManager.MoveRecordCursorToTargetPosition(createdTimeTag);
                    return createdTimeTag != null;

                case Key.Delete:
                    lyricEditorStateManager.MoveCursor(CursorAction.MoveRight);
                    return (bool)timeTagManager?.RemoveTimeTag(currentTimeTag);

                default:
                    return base.OnKeyDown(e);
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
            get => lyricEditorStateManager.Mode;
            set => ScheduleAfterChildren(() => lyricEditorStateManager.SetMode(value));
        }

        public LyricFastEditMode LyricFastEditMode
        {
            get => lyricEditorStateManager.FastEditMode;
            set => ScheduleAfterChildren(() => lyricEditorStateManager.SetFastEditMode(value));
        }
    }
}
