// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components
{
    public abstract class TextTagBlueprintContainer<T> : ExtendBlueprintContainer<T> where T : class, ITextTag
    {
        [Resolved]
        private ILyricCaretState lyricCaretState { get; set; }

        protected readonly Lyric Lyric;

        protected TextTagBlueprintContainer(Lyric lyric)
        {
            Lyric = lyric;
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            lyricCaretState.MoveCaretToTargetPosition(Lyric);
            return base.OnMouseDown(e);
        }

        protected override IEnumerable<SelectionBlueprint<T>> SortForMovement(IReadOnlyList<SelectionBlueprint<T>> blueprints)
            => blueprints.OrderBy(b => b.Item.StartIndex);

        protected abstract class TextTagSelectionHandler : ExtendSelectionHandler<T>
        {
            [Resolved]
            private EditorLyricPiece editorLyricPiece { get; set; }

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
                var selectedTextTags = SelectedItems;
                if (!selectedTextTags.Any())
                    throw new InvalidOperationException("Should have at least one selected item.");

                float deltaXPosition = moveEvent.ScreenSpaceDelta.X;
                Logger.LogPrint($"position: {deltaXPosition}", LoggingTarget.Information);

                if (deltaXPosition < 0)
                {
                    var firstTimeTag = selectedTextTags.OrderBy(x => x.StartIndex).FirstOrDefault();
                    int newStartIndex = calculateNewIndex(firstTimeTag, deltaXPosition, Anchor.CentreLeft);
                    int offset = newStartIndex - firstTimeTag!.StartIndex;
                    if (offset == 0)
                        return false;

                    SetTextTagShifting(selectedTextTags, -1);
                }
                else
                {
                    var lastTimeTag = selectedTextTags.OrderBy(x => x.EndIndex).LastOrDefault();
                    int newEndIndex = calculateNewIndex(lastTimeTag, deltaXPosition, Anchor.CentreRight);
                    int offset = newEndIndex - lastTimeTag!.EndIndex;
                    if (offset == 0)
                        return false;

                    SetTextTagShifting(selectedTextTags, 1);
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
                        int newStartIndex = calculateNewIndex(selectedTextTag, deltaScaleSize, anchor);
                        if (newStartIndex >= selectedTextTag.EndIndex)

                            return false;

                        SetTextTagIndex(selectedTextTag, newStartIndex, null);
                        return true;

                    case Anchor.CentreRight:
                        int newEndIndex = calculateNewIndex(selectedTextTag, deltaScaleSize, anchor);
                        if (newEndIndex <= selectedTextTag.StartIndex)
                            return false;

                        SetTextTagIndex(selectedTextTag, null, newEndIndex);
                        return true;

                    default:
                        return false;
                }
            }

            private int calculateNewIndex(T textTag, float offset, Anchor anchor)
            {
                // get real left-side and right-side position
                var rect = editorLyricPiece.GetTextTagPosition(textTag);

                switch (anchor)
                {
                    case Anchor.CentreLeft:
                        float leftPosition = rect.Left + offset;
                        return TextIndexUtils.ToStringIndex(editorLyricPiece.GetHoverIndex(leftPosition));

                    case Anchor.CentreRight:
                        float rightPosition = rect.Right + offset;
                        return TextIndexUtils.ToStringIndex(editorLyricPiece.GetHoverIndex(rightPosition));

                    default:
                        throw new ArgumentOutOfRangeException(nameof(anchor));
                }
            }

            #endregion

            protected abstract void SetTextTagShifting(IEnumerable<T> textTags, int offset);

            protected abstract void SetTextTagIndex(T textTag, int? startPosition, int? endPosition);
        }
    }
}
