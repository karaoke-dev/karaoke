// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Edit;
using osu.Game.Screens.Edit.Components.Timelines.Summary.Parts;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Timeline;

public partial class EditableTimelineBlueprintContainer<TItem> : BlueprintContainer<TItem> where TItem : class
{
    [Resolved]
    private EditableTimeline timeline { get; set; } = null!;

    protected readonly IBindableList<TItem> Items = new BindableList<TItem>();

    public EditableTimelineBlueprintContainer()
    {
        Items.BindCollectionChanged((_, b) =>
        {
            var removedItems = b.OldItems?.OfType<TItem>().ToArray();
            var createdItems = b.NewItems?.OfType<TItem>().ToArray();

            if (removedItems != null)
            {
                foreach (var item in removedItems)
                    RemoveBlueprintFor(item);
            }

            if (createdItems != null)
            {
                foreach (var item in createdItems)
                    AddBlueprintFor(item);
            }
        });
    }

    protected override void SelectAll()
    {
        SelectedItems.AddRange(Items);
    }

    protected override bool TryMoveBlueprints(DragEvent e, IList<(SelectionBlueprint<TItem> blueprint, Vector2[] originalSnapPositions)> blueprints)
    {
        var result = timeline.FindSnappedPositionAndTime(e.ScreenSpaceMousePosition);

        if (result.Time == null)
            return false;

        var items = blueprints.Select(x => x.blueprint.Item).ToArray();
        double time = result.Time.Value;
        return ApplyOffsetResult(items, time);
    }

    protected virtual bool ApplyOffsetResult(TItem[] items, double time) => false;

    protected override SelectionBlueprintContainer CreateSelectionBlueprintContainer()
        => new EditableTimelineSelectionBlueprintContainer { RelativeSizeAxes = Axes.Both };

    protected override SelectionHandler<TItem> CreateSelectionHandler()
        => new EditableTimelineSelectionHandler();

    protected override DragBox CreateDragBox() => new EditableTimelineDragBox();

    protected partial class EditableTimelineSelectionHandler : SelectionHandler<TItem>
    {
        protected override void OnSelectionChanged()
        {
            base.OnSelectionChanged();

            // should hide selection box if not dragging at current row.
            bool dragging = Parent.IsDragged;
            SelectionBox.FadeTo(dragging ? 1f : 0.0f);
        }

        protected override void DeleteItems(IEnumerable<TItem> items)
        {
            // implement in the child class.
        }
    }

    private partial class EditableTimelineDragBox : DragBox
    {
        public double MinTime { get; private set; }

        public double MaxTime { get; private set; }

        private double? startTime;

        [Resolved]
        private EditableTimeline timeline { get; set; } = null!;

        protected override Drawable CreateBox() => new Box
        {
            RelativeSizeAxes = Axes.Y,
            Alpha = 0.3f,
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

    protected partial class EditableTimelineSelectionBlueprintContainer : SelectionBlueprintContainer
    {
        protected override Container<SelectionBlueprint<TItem>> Content { get; }

        public EditableTimelineSelectionBlueprintContainer()
        {
            AddInternal(new TimelinePart<SelectionBlueprint<TItem>>(Content = new Container<SelectionBlueprint<TItem>> { RelativeSizeAxes = Axes.Both }) { RelativeSizeAxes = Axes.Both });
        }
    }
}
