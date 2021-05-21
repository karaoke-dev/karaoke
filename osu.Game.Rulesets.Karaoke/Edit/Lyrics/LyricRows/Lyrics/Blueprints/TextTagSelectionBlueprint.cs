// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics.Blueprints
{
    public abstract class TextTagSelectionBlueprint<T> : SelectionBlueprint<T> where T : ITextTag
    {
        private readonly Box previewTextArea;
        private readonly Box indexRangeBackground;

        [Resolved]
        private EditorLyricPiece editorLyricPiece { get; set; }

        protected TextTagSelectionBlueprint(T item)
            : base(item)
        {
            InternalChildren = new Drawable[]
            {
                previewTextArea = new Box
                {
                    Alpha = 0,
                    AlwaysPresent = true, // test can be removed?
                },
                indexRangeBackground = new Box
                {
                    Alpha = 0,
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colour)
        {
            indexRangeBackground.Colour = colour.Gray1;
        }

        protected override void OnSelected()
        {
            indexRangeBackground.FadeTo(0.3f, 500);
        }

        protected override void OnDeselected()
        {
            indexRangeBackground.FadeOut(500);
        }

        protected void UpdatePositionAndSize()
        {
            // wait until lyric update ruby position.
            ScheduleAfterChildren(() =>
            {
                var textTagRect = editorLyricPiece.GetTextTagPosition(Item);

                var startIndexPosition = editorLyricPiece.GetTextIndexPosition(TextIndexUtils.FromStringIndex(Item.StartIndex, false));
                var endIndexPosition = editorLyricPiece.GetTextIndexPosition(TextIndexUtils.FromStringIndex(Item.EndIndex, true));

                // update select position
                updateDrawableRect(previewTextArea, textTagRect);

                // update index range position.
                var indexRangePosition = new Vector2(startIndexPosition.X, textTagRect.Y);
                var indexRangeSize = new Vector2(endIndexPosition.X - startIndexPosition.X, textTagRect.Height);
                updateDrawableRect(indexRangeBackground, new RectangleF(indexRangePosition, indexRangeSize));
            });

            void updateDrawableRect(Drawable target, RectangleF rect)
            {
                target.X = rect.X;
                target.Y = rect.Y;
                target.Width = rect.Width;
                target.Height = rect.Height;
            }
        }

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos)
            => previewTextArea.ReceivePositionalInputAt(screenSpacePos);

        public override Vector2 ScreenSpaceSelectionPoint => previewTextArea.ScreenSpaceDrawQuad.Centre;

        public override Quad SelectionQuad => previewTextArea.ScreenSpaceDrawQuad;
    }
}
