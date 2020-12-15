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

        [BackgroundDependencyLoader]
        private void load()
        {
            foreach (var obj in beatmap.HitObjects)
                Schedule(() => addHitObject(obj));
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            beatmap.HitObjectAdded += addHitObject;
            beatmap.HitObjectRemoved += removeHitObject;

            timeTagManager?.MoveCursor(CursorAction.First);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (timeTagManager == null)
                return false;

            // moving cursor action
            switch (e.Key)
            {
                case Key.Up:
                    return timeTagManager.MoveCursor(CursorAction.MoveUp);

                case Key.Down:
                    return timeTagManager.MoveCursor(CursorAction.MoveDown);

                case Key.Left:
                    return timeTagManager.MoveCursor(CursorAction.MoveLeft);

                case Key.Right:
                    return timeTagManager.MoveCursor(CursorAction.MoveRight);

                case Key.PageUp:
                    return timeTagManager.MoveCursor(CursorAction.First);

                case Key.PageDown:
                    return timeTagManager.MoveCursor(CursorAction.Last);
            }

            // edit time tag action
            var currentTimeTag = timeTagManager?.BindableCursorPosition?.Value;

            switch (e.Key)
            {
                case Key.BackSpace:
                    return (bool)timeTagManager?.ClearTimeTagTime(currentTimeTag);

                case Key.Space:
                    var setTimeSuccess = (bool)timeTagManager?.SetTimeTagTime(currentTimeTag);
                    if (setTimeSuccess)
                        timeTagManager.MoveCursor(CursorAction.MoveRight);
                    return setTimeSuccess;

                case Key.N:
                    var createdTimeTag = timeTagManager?.AddTimeTag(currentTimeTag);
                    if (createdTimeTag != null)
                        timeTagManager.MoveCursorToTargetPosition(createdTimeTag);
                    return createdTimeTag != null;

                case Key.Delete:
                    timeTagManager?.MoveCursor(CursorAction.MoveRight);
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

        public Mode Mode { get; set; }

        public LyricFastEditMode LyricFastEditMode { get; set; }
    }
}
