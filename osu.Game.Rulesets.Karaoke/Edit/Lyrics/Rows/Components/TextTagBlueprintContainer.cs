// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
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

            // for now we always allow movement. snapping is provided by the Timeline's "distance" snap implementation
            public override bool HandleMovement(MoveSelectionEvent<T> moveEvent) => true;

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
                        float leftPosition = rect.Left + deltaPosition;
                        int startIndex = TextIndexUtils.ToStringIndex(editorLyricPiece.GetHoverIndex(leftPosition));
                        if (startIndex >= selectedTextTag.EndIndex)
                            return false;

                        SetTextTagIndex(selectedTextTag, startIndex, null);
                        return true;

                    case Anchor.CentreRight:
                        float rightPosition = rect.Right + deltaPosition;
                        int endIndex = TextIndexUtils.ToStringIndex(editorLyricPiece.GetHoverIndex(rightPosition));
                        if (endIndex <= selectedTextTag.StartIndex)
                            return false;

                        SetTextTagIndex(selectedTextTag, null, endIndex);
                        return true;

                    default:
                        return false;
                }
            }

            protected abstract void SetTextTagIndex(T textTag, int? startPosition, int? endPosition);

            protected override void OnSelectionChanged()
            {
                base.OnSelectionChanged();

                // only select one ruby / romaji tag can let user drag to change start and end index.
                SelectionBox.CanScaleX = SelectedItems.Count == 1;
            }
        }
    }
}
