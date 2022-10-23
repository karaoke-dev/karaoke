// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Components.Timelines.Summary.Parts;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.BottomEditor.AdjustTimeTags
{
    public class AdjustTimeTagBlueprintContainer : BindableBlueprintContainer<TimeTag>
    {
        [Resolved(CanBeNull = true)]
        private AdjustTimeTagScrollContainer timeline { get; set; }

        [Resolved]
        private EditorClock editorClock { get; set; }

        [Resolved]
        private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; }

        [BackgroundDependencyLoader]
        private void load(BindableList<TimeTag> timeTags)
        {
            // Add time-tag into blueprint container
            RegisterBindable(timeTags);
        }

        protected override IEnumerable<SelectionBlueprint<TimeTag>> SortForMovement(IReadOnlyList<SelectionBlueprint<TimeTag>> blueprints)
            => blueprints.OrderBy(b => b.Item.Index);

        protected override bool ApplySnapResult(SelectionBlueprint<TimeTag>[] blueprints, SnapResult result)
        {
            if (!base.ApplySnapResult(blueprints, result))
                return false;

            if (result.Time == null)
                return false;

            var timeTags = blueprints.OfType<AdjustTimeTagHitObjectBlueprint>().Select(x => x.Item).ToArray();
            var firstTimeTag = timeTags.FirstOrDefault();
            if (firstTimeTag == null)
                return false;

            double offset = result.Time.Value - timeline.GetPreviewTime(firstTimeTag);
            if (offset == 0)
                return false;

            lyricTimeTagsChangeHandler.ShiftingTimeTagTime(timeTags, offset);

            return true;
        }

        protected override Container<SelectionBlueprint<TimeTag>> CreateSelectionBlueprintContainer()
            => new TimeTagEditorSelectionBlueprintContainer { RelativeSizeAxes = Axes.Both };

        protected override SelectionHandler<TimeTag> CreateSelectionHandler()
            => new TimeTagEditorSelectionHandler();

        protected override SelectionBlueprint<TimeTag> CreateBlueprintFor(TimeTag item)
            => new AdjustTimeTagHitObjectBlueprint(item);

        protected override DragBox CreateDragBox() => new TimelineDragBox();

        protected override bool OnClick(ClickEvent e)
        {
            base.OnClick(e);

            // skip if already have selected blueprint.
            if (ClickedBlueprint != null)
                return true;

            // navigation to target time.
            var navigationTime = timeline.FindSnappedPositionAndTime(e.ScreenSpaceMousePosition);
            if (navigationTime.Time == null)
                return false;

            editorClock.SeekSmoothlyTo(navigationTime.Time.Value);
            return true;
        }

        protected class TimeTagEditorSelectionHandler : BindableSelectionHandler
        {
            [Resolved]
            private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; }

            [BackgroundDependencyLoader]
            private void load(ITimeTagModeState timeTagModeState)
            {
                SelectedItems.BindTo(timeTagModeState.SelectedItems);
            }

            // for now we always allow movement. snapping is provided by the Timeline's "distance" snap implementation
            public override bool HandleMovement(MoveSelectionEvent<TimeTag> moveEvent) => true;

            protected override void DeleteItems(IEnumerable<TimeTag> items)
            {
                lyricTimeTagsChangeHandler.RemoveRange(items);
            }

            protected override IEnumerable<MenuItem> GetContextMenuItemsForSelection(IEnumerable<SelectionBlueprint<TimeTag>> selection)
            {
                var timeTags = selection.Select(x => x.Item).ToArray();

                if (timeTags.Any(x => x.Time != null))
                {
                    return new[]
                    {
                        new OsuMenuItem("Clear time", MenuItemType.Standard, () =>
                        {
                            timeTags.ForEach(x => x.Time = null);

                            // todo : should re-calculate all preview position because some time-tag without position might be affected.
                        })
                    };
                }

                return base.GetContextMenuItemsForSelection(selection);
            }
        }

        private class TimelineDragBox : DragBox
        {
            public double MinTime { get; private set; }

            public double MaxTime { get; private set; }

            private double? startTime;

            [Resolved]
            private AdjustTimeTagScrollContainer timeline { get; set; }

            protected override Drawable CreateBox() => new Box
            {
                RelativeSizeAxes = Axes.Y,
                Alpha = 0.3f
            };

            public override void HandleDrag(MouseButtonEvent e)
            {
                startTime ??= timeline.TimeAtPosition(e.MouseDownPosition.X);
                double endTime = timeline.TimeAtPosition(e.MousePosition.X);

                MinTime = Math.Min(startTime.Value, endTime);
                MaxTime = Math.Max(startTime.Value, endTime);

                Box.X = timeline.PositionAtTime(MinTime);
                Box.Width = timeline.PositionAtTime(MaxTime) - Box.X;
            }

            public override void Hide()
            {
                base.Hide();
                startTime = null;
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
