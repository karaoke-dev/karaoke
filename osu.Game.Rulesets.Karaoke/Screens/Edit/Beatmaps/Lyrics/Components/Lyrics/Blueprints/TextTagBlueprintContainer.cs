// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics.Blueprints;

public abstract partial class TextTagBlueprintContainer<T> : LyricPropertyBlueprintContainer<T> where T : class, ITextTag
{
    protected TextTagBlueprintContainer(Lyric lyric)
        : base(lyric)
    {
    }

    protected override IEnumerable<SelectionBlueprint<T>> SortForMovement(IReadOnlyList<SelectionBlueprint<T>> blueprints)
        => blueprints.OrderBy(b => b.Item.StartIndex);

    protected abstract partial class TextTagSelectionHandler<TModeState> : LyricPropertySelectionHandler<TModeState>
        where TModeState : IHasBlueprintSelection<T>
    {
        [Resolved]
        private IPreviewLyricPositionProvider previewLyricPositionProvider { get; set; } = null!;

        private float deltaScaleSize;

        protected override void OnSelectionChanged()
        {
            base.OnSelectionChanged();

            // only select one ruby / romaji tag can let user drag to change start and end index.
            SelectionBox.CanScaleX = SelectedItems.Count == 1;

            // should clear delta size before change start/end index.
            deltaScaleSize = 0;
        }

        #region User Input Handling

        // for now we always allow movement. snapping is provided by the Timeline's "distance" snap implementation
        public override bool HandleMovement(MoveSelectionEvent<T> moveEvent)
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

                SetTextTagShifting(SelectedItems, -1);
            }
            else
            {
                var lastTimeTag = SelectedItems.MaxBy(x => x.EndIndex) ?? throw new InvalidOperationException();
                int? newEndIndex = calculateNewIndex(lastTimeTag, deltaXPosition, Anchor.CentreRight);
                int? offset = newEndIndex - lastTimeTag.EndIndex;
                if (offset is null or 0)
                    return false;

                SetTextTagShifting(SelectedItems, 1);
            }

            return true;
        }

        public override bool HandleScale(Vector2 scale, Anchor anchor)
        {
            deltaScaleSize += scale.X;

            // this feature only works if only select one ruby / romaji tag.
            var selectedTextTag = SelectedItems.FirstOrDefault();
            if (selectedTextTag == null)
                return false;

            switch (anchor)
            {
                case Anchor.CentreLeft:
                    int? newStartIndex = calculateNewIndex(selectedTextTag, deltaScaleSize, anchor);
                    if (newStartIndex == null || !TextTagUtils.ValidNewStartIndex(selectedTextTag, newStartIndex.Value))
                        return false;

                    SetTextTagIndex(selectedTextTag, newStartIndex, null);
                    return true;

                case Anchor.CentreRight:
                    int? newEndIndex = calculateNewIndex(selectedTextTag, deltaScaleSize, anchor);
                    if (newEndIndex == null || !TextTagUtils.ValidNewEndIndex(selectedTextTag, newEndIndex.Value))
                        return false;

                    SetTextTagIndex(selectedTextTag, null, newEndIndex);
                    return true;

                default:
                    return false;
            }
        }

        private int? calculateNewIndex(T textTag, float offset, Anchor anchor)
        {
            // get real left-side and right-side position
            var rect = previewLyricPositionProvider.GetTextTagByPosition(textTag);

            // todo: need to think about how to handle the case if the text-tag already out of the range of the text.
            if (rect == null)
                throw new InvalidOperationException($"{nameof(textTag)} not in the range of the text.");

            switch (anchor)
            {
                case Anchor.CentreLeft:
                    float leftPosition = rect.Value.Left + offset;
                    return previewLyricPositionProvider.GetCharIndexByPosition(leftPosition);

                case Anchor.CentreRight:
                    float rightPosition = rect.Value.Right + offset;
                    return previewLyricPositionProvider.GetCharIndexByPosition(rightPosition);

                default:
                    throw new ArgumentOutOfRangeException(nameof(anchor));
            }
        }

        #endregion

        protected abstract void SetTextTagShifting(IEnumerable<T> textTags, int offset);

        protected abstract void SetTextTagIndex(T textTag, int? startPosition, int? endPosition);
    }
}
