// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.Components.ContextMenu;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Components.Timelines.Summary.Parts;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Components.SingerLyricEditor
{
    public class LyricBlueprintContainer : BlueprintContainer<Lyric>
    {
        private readonly Singer singer;

        public LyricBlueprintContainer(Singer singer)
        {
            this.singer = singer;
        }

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap beatmap)
        {
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();
            foreach (var lyric in lyrics)
                AddBlueprintFor(lyric);
        }

        protected override IEnumerable<SelectionBlueprint<Lyric>> SortForMovement(IReadOnlyList<SelectionBlueprint<Lyric>> blueprints)
            => blueprints.OrderBy(b => b.Item.LyricStartTime);

        protected override Container<SelectionBlueprint<Lyric>> CreateSelectionBlueprintContainer()
            => new SingerLyricSelectionBlueprintContainer { RelativeSizeAxes = Axes.Both };

        protected override SelectionHandler<Lyric> CreateSelectionHandler()
            => new SingerLyricSelectionHandler();

        protected override SelectionBlueprint<Lyric> CreateBlueprintFor(Lyric hitObject)
            => new LyricTimelineHitObjectBlueprint(hitObject);

        protected override DragBox CreateDragBox(Action<RectangleF> performSelect) => new SingerLyricDragBox(performSelect);

        protected class SingerLyricSelectionHandler : SelectionHandler<Lyric>
        {
            [Resolved]
            private LyricManager lyricManager { get; set; }

            public override bool HandleMovement(MoveSelectionEvent<Lyric> moveEvent) => false;

            protected override IEnumerable<MenuItem> GetContextMenuItemsForSelection(IEnumerable<SelectionBlueprint<Lyric>> selection)
            {
                var lyrics = selection.Select(x => x.Item).ToList();
                var contextMenu = new SingerContextMenu(lyricManager, lyrics, "");
                return contextMenu.Items;
            }

            protected override void DeleteItems(IEnumerable<Lyric> items)
            {
                // should not able to delete lyric in here.
            }
        }

        private class SingerLyricDragBox : DragBox
        {
            // the following values hold the start and end X positions of the drag box in the timeline's local space,
            // but with zoom unapplied in order to be able to compensate for positional changes
            // while the timeline is being zoomed in/out.
            private float? selectionStart;
            private float selectionEnd;

            [Resolved]
            private SingerLyricEditor editor { get; set; }

            public SingerLyricDragBox(Action<RectangleF> performSelect)
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
                selectionStart ??= e.MouseDownPosition.X / editor.CurrentZoom;

                // only calculate end when a transition is not in progress to avoid bouncing.
                if (Precision.AlmostEquals(editor.CurrentZoom, editor.Zoom))
                    selectionEnd = e.MousePosition.X / editor.CurrentZoom;

                updateDragBoxPosition();
                return true;
            }

            private void updateDragBoxPosition()
            {
                if (selectionStart == null)
                    return;

                float rescaledStart = selectionStart.Value * editor.CurrentZoom;
                float rescaledEnd = selectionEnd * editor.CurrentZoom;

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
