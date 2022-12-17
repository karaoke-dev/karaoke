// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit.Components.Timelines.Summary.Parts;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Timeline;

public partial class LyricsBlueprintContainer : BlueprintContainer<Lyric>
{
    protected readonly IBindableList<Lyric> Lyrics = new BindableList<Lyric>();

    public LyricsBlueprintContainer()
    {
        Lyrics.BindCollectionChanged((_, b) =>
        {
            var removedLyrics = b.OldItems?.OfType<Lyric>().ToArray();
            var createdLyrics = b.NewItems?.OfType<Lyric>().ToArray();

            if (removedLyrics != null)
            {
                foreach (var lyric in removedLyrics)
                    RemoveBlueprintFor(lyric);
            }

            if (createdLyrics != null)
            {
                foreach (var lyric in createdLyrics)
                    AddBlueprintFor(lyric);
            }
        });
    }

    protected override void SelectAll()
    {
        SelectedItems.AddRange(Lyrics);
    }

    protected override IEnumerable<SelectionBlueprint<Lyric>> SortForMovement(IReadOnlyList<SelectionBlueprint<Lyric>> blueprints)
        => blueprints.OrderBy(b => b.Item.LyricStartTime);

    protected override Container<SelectionBlueprint<Lyric>> CreateSelectionBlueprintContainer()
        => new LyricSelectionBlueprintContainer { RelativeSizeAxes = Axes.Both };

    protected override SelectionHandler<Lyric> CreateSelectionHandler()
        => new LyricSelectionHandler();

    protected override SelectionBlueprint<Lyric> CreateBlueprintFor(Lyric item)
        => new EditableLyricTimelineSelectionBlueprint(item);

    protected override DragBox CreateDragBox() => new LyricDragBox();

    protected partial class LyricSelectionHandler : SelectionHandler<Lyric>
    {
        protected override void OnSelectionChanged()
        {
            base.OnSelectionChanged();

            // should hide selection box if not dragging at current row.
            bool dragging = Parent.IsDragged;
            SelectionBox.FadeTo(dragging ? 1f : 0.0f);
        }

        protected override void DeleteItems(IEnumerable<Lyric> items)
        {
            // implement in the child class.
        }
    }

    private partial class LyricDragBox : DragBox
    {
        public double MinTime { get; private set; }

        public double MaxTime { get; private set; }

        private double? startTime;

        [Resolved, AllowNull]
        private LyricsTimeline timeline { get; set; }

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

    protected partial class LyricSelectionBlueprintContainer : Container<SelectionBlueprint<Lyric>>
    {
        protected override Container<SelectionBlueprint<Lyric>> Content { get; }

        public LyricSelectionBlueprintContainer()
        {
            AddInternal(new TimelinePart<SelectionBlueprint<Lyric>>(Content = new Container<SelectionBlueprint<Lyric>> { RelativeSizeAxes = Axes.Both }) { RelativeSizeAxes = Axes.Both });
        }
    }
}
