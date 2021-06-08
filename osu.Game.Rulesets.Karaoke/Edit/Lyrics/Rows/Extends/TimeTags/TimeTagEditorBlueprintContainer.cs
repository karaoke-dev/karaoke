// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Components.Timelines.Summary.Parts;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.TimeTags
{
    public class TimeTagEditorBlueprintContainer : ExtendBlueprintContainer<TimeTag>
    {
        [Resolved]
        private ILyricEditorState state { get; set; }

        [Resolved(CanBeNull = true)]
        private TimeTagEditor timeline { get; set; }

        [Resolved]
        private EditorClock editorClock { get; set; }

        [Resolved]
        private LyricManager lyricManager { get; set; }

        [UsedImplicitly]
        private readonly Bindable<TimeTag[]> timeTags;

        protected readonly Lyric Lyric;

        public TimeTagEditorBlueprintContainer(Lyric lyric)
        {
            Lyric = lyric;
            timeTags = lyric.TimeTagsBindable.GetBoundCopy();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            SelectedItems.BindTo(state.SelectedTimeTags);

            // Add time-tag into blueprint container
            RegistBindable(timeTags);
        }

        protected override IEnumerable<SelectionBlueprint<TimeTag>> SortForMovement(IReadOnlyList<SelectionBlueprint<TimeTag>> blueprints)
            => blueprints.OrderBy(b => b.Item.Time);

        protected override bool ApplySnapResult(SelectionBlueprint<TimeTag>[] blueprints, SnapResult result)
        {
            if (!base.ApplySnapResult(blueprints, result))
                return false;

            if (result.Time == null)
                return false;

            var timeTagBlueprints = blueprints.OfType<TimeTagEditorHitObjectBlueprint>();
            var firstDragBlueprint = timeTagBlueprints.FirstOrDefault();
            if (firstDragBlueprint == null)
                return false;

            var offset = result.Time.Value - firstDragBlueprint.PreviewTime;
            if (offset == 0)
                return false;

            // todo : should not save separately.
            foreach (var blueprint in timeTagBlueprints)
            {
                var timeTag = blueprint.Item;
                timeTag.Time = blueprint.PreviewTime + offset;
            }

            return true;
        }

        /// <summary>
        /// Commit time-tag time.
        /// </summary>
        protected override void DragOperationCompleted()
        {
            var processedTimeTags = SelectionBlueprints.Where(x => x.State == SelectionState.Selected).Select(x => x.Item);

            // todo : should change together.
            foreach (var timeTag in processedTimeTags)
            {
                if (timeTag.Time.HasValue)
                    lyricManager.SetTimeTagTime(timeTag, timeTag.Time.Value);
            }
        }

        protected override Container<SelectionBlueprint<TimeTag>> CreateSelectionBlueprintContainer()
            => new TimeTagEditorSelectionBlueprintContainer { RelativeSizeAxes = Axes.Both };

        protected override SelectionHandler<TimeTag> CreateSelectionHandler()
            => new TimeTagEditorSelectionHandler();

        protected override SelectionBlueprint<TimeTag> CreateBlueprintFor(TimeTag item)
            => new TimeTagEditorHitObjectBlueprint(item);

        protected override DragBox CreateDragBox(Action<RectangleF> performSelect) => new TimelineDragBox(performSelect);

        protected override bool OnClick(ClickEvent e)
        {
            base.OnClick(e);

            // skip if already have selected blueprint.
            if (ClickedBlueprint != null)
                return true;

            // navigation to target time.
            var navigationTime = timeline.SnapScreenSpacePositionToValidTime(e.ScreenSpaceMousePosition);
            if (navigationTime.Time == null)
                return false;

            editorClock.SeekSmoothlyTo(navigationTime.Time.Value);
            return true;
        }

        protected override void DeselectAll()
        {
            state.ClearSelectedTimeTags();
        }

        protected class TimeTagEditorSelectionHandler : ExtendSelectionHandler<TimeTag>
        {
            [Resolved]
            private ILyricEditorState state { get; set; }

            [Resolved]
            private LyricManager lyricManager { get; set; }

            [BackgroundDependencyLoader]
            private void load()
            {
                SelectedItems.BindTo(state.SelectedTimeTags);
            }

            // for now we always allow movement. snapping is provided by the Timeline's "distance" snap implementation
            public override bool HandleMovement(MoveSelectionEvent<TimeTag> moveEvent) => true;

            protected override void DeleteItems(IEnumerable<TimeTag> items)
            {
                // todo : delete time-line
                foreach (var item in items)
                {
                    lyricManager.RemoveTimeTag(item);
                }
            }

            protected override IEnumerable<MenuItem> GetContextMenuItemsForSelection(IEnumerable<SelectionBlueprint<TimeTag>> selection)
            {
                var timeTags = selection.Select(x => x.Item);

                if (timeTags.Any(x => x.Time != null))
                {
                    return new[]
                    {
                        new OsuMenuItem("Clear time", MenuItemType.Standard, () =>
                        {
                            timeTags.ForEach(x => x.Time = null);
                        })
                    };
                }

                return base.GetContextMenuItemsForSelection(selection);
            }
        }

        private class TimelineDragBox : DragBox
        {
            // the following values hold the start and end X positions of the drag box in the timeline's local space,
            // but with zoom unapplied in order to be able to compensate for positional changes
            // while the timeline is being zoomed in/out.
            private float? selectionStart;
            private float selectionEnd;

            [Resolved]
            private TimeTagEditor timeline { get; set; }

            public TimelineDragBox(Action<RectangleF> performSelect)
                : base(performSelect)
            {
            }

            protected override Drawable CreateBox() => new Box
            {
                RelativeSizeAxes = Axes.Y,
                Alpha = 0.3f
            };

            public override bool HandleDrag(MouseButtonEvent e)
            {
                selectionStart ??= e.MouseDownPosition.X / timeline.CurrentZoom;

                // only calculate end when a transition is not in progress to avoid bouncing.
                if (Precision.AlmostEquals(timeline.CurrentZoom, timeline.Zoom))
                    selectionEnd = e.MousePosition.X / timeline.CurrentZoom;

                updateDragBoxPosition();
                return true;
            }

            private void updateDragBoxPosition()
            {
                if (selectionStart == null)
                    return;

                float rescaledStart = selectionStart.Value * timeline.CurrentZoom;
                float rescaledEnd = selectionEnd * timeline.CurrentZoom;

                Box.X = Math.Min(rescaledStart, rescaledEnd);
                Box.Width = Math.Abs(rescaledStart - rescaledEnd);

                var boxScreenRect = Box.ScreenSpaceDrawQuad.AABBFloat;

                // we don't care about where the hitobjects are vertically. in cases like stacking display, they may be outside the box without this adjustment.
                boxScreenRect.Y -= boxScreenRect.Height;
                boxScreenRect.Height *= 2;

                PerformSelection?.Invoke(boxScreenRect);
            }

            public override void Hide()
            {
                base.Hide();
                selectionStart = null;
            }
        }

        protected class TimeTagEditorSelectionBlueprintContainer : Container<SelectionBlueprint<TimeTag>>
        {
            protected override Container<SelectionBlueprint<TimeTag>> Content { get; }

            public TimeTagEditorSelectionBlueprintContainer()
            {
                AddInternal(new TimelinePart<SelectionBlueprint<TimeTag>>(Content = new TimeTagOrderedSelectionContainer { RelativeSizeAxes = Axes.Both }) { RelativeSizeAxes = Axes.Both });
            }
        }
    }
}
