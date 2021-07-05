// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
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
        private BlueprintSelectionState blueprintSelectionState { get; set; }

        [Resolved]
        private EditorLyricPiece editorLyricPiece { get; set; }

        [Resolved]
        private LyricCaretState lyricCaretState { get; set; }

        protected readonly Lyric Lyric;

        protected TextTagBlueprintContainer(Lyric lyric)
        {
            Lyric = lyric;
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            lyricCaretState.MoveCaretToTargetPosition(new NavigateCaretPosition(Lyric));
            return base.OnMouseDown(e);
        }

        protected override bool ApplySnapResult(SelectionBlueprint<T>[] blueprints, SnapResult result)
        {
            if (!base.ApplySnapResult(blueprints, result))
                return false;

            // handle lots of ruby / romaji drag position changed.
            var items = blueprints.Select(x => x.Item).ToArray();
            if (!items.Any())
                return false;

            var leftPosition = ToLocalSpace(result.ScreenSpacePosition).X;
            var startIndex = TextIndexUtils.ToStringIndex(editorLyricPiece.GetHoverIndex(leftPosition));
            var diff = startIndex - items.First().StartIndex;
            if (diff == 0)
                return false;

            foreach (var item in items)
            {
                var newStartIndex = item.StartIndex + diff;
                var newEndIndex = item.EndIndex + diff;
                if (!LyricUtils.AbleToInsertTextTagAtIndex(Lyric, newStartIndex) || !LyricUtils.AbleToInsertTextTagAtIndex(Lyric, newEndIndex))
                    continue;

                item.StartIndex = newStartIndex;
                item.EndIndex = newEndIndex;
            }

            return true;
        }

        protected override IEnumerable<SelectionBlueprint<T>> SortForMovement(IReadOnlyList<SelectionBlueprint<T>> blueprints)
            => blueprints.OrderBy(b => b.Item.StartIndex);

        protected override void DeselectAll()
        {
            blueprintSelectionState.ClearSelectedTextTags();
        }

        protected class TextTagSelectionHandler : ExtendSelectionHandler<T>
        {
            [Resolved]
            private LyricManager lyricManager { get; set; }

            [Resolved]
            private EditorLyricPiece editorLyricPiece { get; set; }

            // for now we always allow movement. snapping is provided by the Timeline's "distance" snap implementation
            public override bool HandleMovement(MoveSelectionEvent<T> moveEvent) => true;

            protected override void DeleteItems(IEnumerable<T> items)
            {
                // todo : delete ruby or romaji
            }

            private float deltaPosition;

            protected override void OnOperationBegan()
            {
                base.OnOperationBegan();
                deltaPosition = 0;
            }

            public override bool HandleScale(Vector2 scale, Anchor anchor)
            {
                deltaPosition += scale.X;

                // this feature only works if only select one ruby / romaji tag.
                var selectedTextTag = SelectedItems.FirstOrDefault();
                if (selectedTextTag == null)
                    return false;

                // get real left-side and right-side position
                var rect = editorLyricPiece.GetTextTagPosition(selectedTextTag);

                switch (anchor)
                {
                    case Anchor.CentreLeft:
                        var leftPosition = rect.Left + deltaPosition;
                        var startIndex = TextIndexUtils.ToStringIndex(editorLyricPiece.GetHoverIndex(leftPosition));
                        if (startIndex >= selectedTextTag.EndIndex)
                            return false;

                        selectedTextTag.StartIndex = startIndex;
                        return true;

                    case Anchor.CentreRight:
                        var rightPosition = rect.Right + deltaPosition;
                        var endIndex = TextIndexUtils.ToStringIndex(editorLyricPiece.GetHoverIndex(rightPosition));
                        if (endIndex <= selectedTextTag.StartIndex)
                            return false;

                        selectedTextTag.EndIndex = endIndex;
                        return true;

                    default:
                        return false;
                }
            }

            protected override void OnSelectionChanged()
            {
                base.OnSelectionChanged();

                // only select one ruby / romaji tag can let user drag to change start and end index.
                SelectionBox.CanScaleX = SelectedItems.Count == 1;
            }
        }
    }
}
