// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Logging;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics.Blueprints;

public partial class RubyBlueprintContainer : LyricPropertyBlueprintContainer<RubyTag>
{
    public RubyBlueprintContainer(Lyric lyric)
        : base(lyric)
    {
    }

    protected override BindableList<RubyTag> GetProperties(Lyric lyric)
        => lyric.RubyTagsBindable.GetBoundCopy();

    protected override SelectionHandler<RubyTag> CreateSelectionHandler()
        => new RubyTagSelectionHandler();

    protected override SelectionBlueprint<RubyTag> CreateBlueprintFor(RubyTag item)
        => new RubyTagSelectionBlueprint(item);

    protected override IEnumerable<SelectionBlueprint<RubyTag>> SortForMovement(IReadOnlyList<SelectionBlueprint<RubyTag>> blueprints)
        => blueprints.OrderBy(b => b.Item.StartIndex);

    protected partial class RubyTagSelectionHandler : LyricPropertySelectionHandler<IEditRubyModeState>
    {
        [Resolved]
        private ILyricRubyTagsChangeHandler rubyTagsChangeHandler { get; set; } = null!;

        [Resolved]
        private IPreviewLyricPositionProvider previewLyricPositionProvider { get; set; } = null!;

        // need to implement this because blueprint container support change the x scale.
        public override SelectionScaleHandler CreateScaleHandler()
            => new RubyTagSelectionScaleHandler();

        #region User Input Handling

        // for now we always allow movement. snapping is provided by the Timeline's "distance" snap implementation
        public override bool HandleMovement(MoveSelectionEvent<RubyTag> moveEvent)
        {
            if (!SelectedItems.Any())
                throw new InvalidOperationException("Should have at least one selected item.");

            float deltaXPosition = moveEvent.ScreenSpaceDelta.X;
            Logger.LogPrint($"position: {deltaXPosition}", LoggingTarget.Information);

            if (deltaXPosition < 0)
            {
                var firstTimeTag = SelectedItems.MinBy(x => x.StartIndex) ?? throw new InvalidOperationException();
                int? newStartIndex = calculateNewIndex(firstTimeTag, deltaXPosition, Anchor.CentreLeft);
                int? offset = newStartIndex - firstTimeTag.StartIndex;
                if (offset is null or 0)
                    return false;

                setRubyTagShifting(SelectedItems, -1);
            }
            else
            {
                var lastTimeTag = SelectedItems.MaxBy(x => x.EndIndex) ?? throw new InvalidOperationException();
                int? newEndIndex = calculateNewIndex(lastTimeTag, deltaXPosition, Anchor.CentreRight);
                int? offset = newEndIndex - lastTimeTag.EndIndex;
                if (offset is null or 0)
                    return false;

                setRubyTagShifting(SelectedItems, 1);
            }

            return true;

            void setRubyTagShifting(IEnumerable<RubyTag> rubyTags, int offset)
                => rubyTagsChangeHandler.ShiftingIndex(rubyTags, offset);
        }

        private int? calculateNewIndex(RubyTag rubyTag, float offset, Anchor anchor)
        {
            // get real left-side and right-side position
            var rect = previewLyricPositionProvider.GetRubyTagByPosition(rubyTag);

            // todo: need to think about how to handle the case if the text-tag already out of the range of the text.
            if (rect == null)
                throw new InvalidOperationException($"{nameof(rubyTag)} not in the range of the text.");

            switch (anchor)
            {
                case Anchor.CentreLeft:
                    var leftPosition = rect.Value.BottomLeft + new Vector2(offset, 0);
                    return previewLyricPositionProvider.GetCharIndexByPosition(leftPosition);

                case Anchor.CentreRight:
                    var rightPosition = rect.Value.BottomRight + new Vector2(offset, 0);
                    return previewLyricPositionProvider.GetCharIndexByPosition(rightPosition);

                default:
                    throw new ArgumentOutOfRangeException(nameof(anchor));
            }
        }

        #endregion

        protected override void DeleteItems(IEnumerable<RubyTag> items)
            => rubyTagsChangeHandler.RemoveRange(items);
    }

    private partial class RubyTagSelectionScaleHandler : SelectionScaleHandler
    {
        [Resolved]
        private IPreviewLyricPositionProvider previewLyricPositionProvider { get; set; } = null!;

        [Resolved]
        private ILyricRubyTagsChangeHandler rubyTagsChangeHandler { get; set; } = null!;

        private BindableList<RubyTag> selectedItems { get; } = new();

        [BackgroundDependencyLoader]
        private void load(IEditRubyModeState editRubyModeState)
        {
            selectedItems.BindTo(editRubyModeState.SelectedItems);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            selectedItems.CollectionChanged += (_, __) => updateState();
            updateState();
        }

        private void updateState()
        {
            // only select one ruby tag can let user drag to change start and end index.
            CanScaleX.Value = selectedItems.Count == 1;
        }

        public override void Begin()
        {
            base.Begin();

            var selectedItemsRect = selectedItems
                                    .Select(x => previewLyricPositionProvider.GetRubyTagByPosition(x))
                                    .Where(rect => rect != null)
                                    .OfType<RectangleF>();

            var rect = selectedItemsRect.Aggregate(new RectangleF(), RectangleF.Union);

            // Update the quad
            OriginalSurroundingQuad = Quad.FromRectangle(rect);
        }

        public override void Update(Vector2 scale, Vector2? origin = null, Axes adjustAxis = Axes.Both, float axisRotation = 0)
        {
            // this feature only works if only select one ruby tag.
            var selectedRubyTag = selectedItems.FirstOrDefault();
            if (selectedRubyTag == null)
                return;

            if (adjustAxis != Axes.X)
                throw new InvalidOperationException("Only can adjust x axes");

            if (origin == null || OriginalSurroundingQuad == null)
                return;

            float offset = OriginalSurroundingQuad.Value.Width * (scale.X - 1);

            if (origin.Value.X > OriginalSurroundingQuad.Value.Centre.X)
            {
                int? newStartIndex = calculateNewIndex(selectedRubyTag, -offset, Anchor.CentreLeft);
                if (newStartIndex == null || !RubyTagUtils.ValidNewStartIndex(selectedRubyTag, newStartIndex.Value))
                    return;

                setRubyTagIndex(selectedRubyTag, newStartIndex, null);
            }
            else
            {
                int? newEndIndex = calculateNewIndex(selectedRubyTag, offset, Anchor.CentreRight);
                if (newEndIndex == null || !RubyTagUtils.ValidNewEndIndex(selectedRubyTag, newEndIndex.Value))
                    return;

                setRubyTagIndex(selectedRubyTag, null, newEndIndex);
            }

            return;

            void setRubyTagIndex(RubyTag rubyTag, int? startPosition, int? endPosition)
                => rubyTagsChangeHandler.SetIndex(rubyTag, startPosition, endPosition);
        }

        public override void Commit()
        {
            base.Commit();

            OriginalSurroundingQuad = null;
        }

        private int? calculateNewIndex(RubyTag rubyTag, float offset, Anchor anchor)
        {
            // get real left-side and right-side position
            var rect = previewLyricPositionProvider.GetRubyTagByPosition(rubyTag);

            // todo: need to think about how to handle the case if the text-tag already out of the range of the text.
            if (rect == null)
                throw new InvalidOperationException($"{nameof(rubyTag)} not in the range of the text.");

            switch (anchor)
            {
                case Anchor.CentreLeft:
                    var leftPosition = rect.Value.BottomLeft + new Vector2(offset, 0);
                    return previewLyricPositionProvider.GetCharIndexByPosition(leftPosition);

                case Anchor.CentreRight:
                    var rightPosition = rect.Value.BottomRight + new Vector2(offset, 0);
                    return previewLyricPositionProvider.GetCharIndexByPosition(rightPosition);

                default:
                    throw new ArgumentOutOfRangeException(nameof(anchor));
            }
        }
    }
}
