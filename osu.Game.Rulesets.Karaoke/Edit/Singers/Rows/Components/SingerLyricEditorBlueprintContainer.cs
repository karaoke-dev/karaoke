// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Components.ContextMenu;
using osu.Game.Rulesets.Karaoke.Edit.Singers.Rows.Components.Blueprints;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Components.Timelines.Summary.Parts;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Rows.Components
{
    public class LyricBlueprintContainer : BlueprintContainer<Lyric>
    {
        private readonly IBindableList<Lyric> bindableLyrics = new BindableList<Lyric>();

        public LyricBlueprintContainer()
        {
            bindableLyrics.BindCollectionChanged((_, b) =>
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

        [BackgroundDependencyLoader]
        private void load(ILyricsProvider lyricsProvider)
        {
            bindableLyrics.BindTo(lyricsProvider.BindableLyrics);
        }

        protected override IEnumerable<SelectionBlueprint<Lyric>> SortForMovement(IReadOnlyList<SelectionBlueprint<Lyric>> blueprints)
            => blueprints.OrderBy(b => b.Item.LyricStartTime);

        protected override Container<SelectionBlueprint<Lyric>> CreateSelectionBlueprintContainer()
            => new SingerLyricSelectionBlueprintContainer { RelativeSizeAxes = Axes.Both };

        protected override SelectionHandler<Lyric> CreateSelectionHandler()
            => new SingerLyricSelectionHandler();

        protected override SelectionBlueprint<Lyric> CreateBlueprintFor(Lyric item)
            => new LyricTimelineHitObjectBlueprint(item);

        protected override DragBox CreateDragBox() => new SingerLyricDragBox();

        protected class SingerLyricSelectionHandler : SelectionHandler<Lyric>
        {
            [Resolved]
            private EditorBeatmap beatmap { get; set; }

            [Resolved]
            private ILyricSingerChangeHandler lyricSingerChangeHandler { get; set; }

            [Resolved]
            private BindableList<Lyric> selectedLyrics { get; set; }

            [BackgroundDependencyLoader]
            private void load()
            {
                SelectedItems.BindTo(selectedLyrics);
            }

            protected override void OnSelectionChanged()
            {
                base.OnSelectionChanged();

                // should hide selection box if not dragging at current row.
                bool dragging = Parent.IsDragged;
                SelectionBox.FadeTo(dragging ? 1f : 0.0f);
            }

            protected override IEnumerable<MenuItem> GetContextMenuItemsForSelection(IEnumerable<SelectionBlueprint<Lyric>> selection)
            {
                var contextMenu = new SingerContextMenu(beatmap, lyricSingerChangeHandler, string.Empty, () =>
                {
                    selectedLyrics.Clear();
                });
                return contextMenu.Items;
            }

            protected override void DeleteItems(IEnumerable<Lyric> items)
            {
                // todo : remove all in the same time.
                foreach (var item in items)
                {
                    lyricSingerChangeHandler.Clear();
                }

                selectedLyrics.Clear();
            }
        }

        private class SingerLyricDragBox : DragBox
        {
            public double MinTime { get; private set; }

            public double MaxTime { get; private set; }

            private double? startTime;

            [Resolved]
            private SingerLyricTimeline timeline { get; set; }

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

        protected class SingerLyricSelectionBlueprintContainer : Container<SelectionBlueprint<Lyric>>
        {
            protected override Container<SelectionBlueprint<Lyric>> Content { get; }

            public SingerLyricSelectionBlueprintContainer()
            {
                AddInternal(new TimelinePart<SelectionBlueprint<Lyric>>(Content = new Container<SelectionBlueprint<Lyric>> { RelativeSizeAxes = Axes.Both }) { RelativeSizeAxes = Axes.Both });
            }
        }
    }
}
