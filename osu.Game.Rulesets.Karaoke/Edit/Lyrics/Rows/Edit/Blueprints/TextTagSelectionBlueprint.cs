// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Edit.Blueprints
{
    public abstract class TextTagSelectionBlueprint<T> : SelectionBlueprint<T> where T : ITextTag
    {
        private readonly Container previewTextArea;
        private readonly Container indexRangeBackground;

        [Resolved]
        private EditorKaraokeSpriteText karaokeSpriteText { get; set; }

        protected TextTagSelectionBlueprint(T item)
            : base(item)
        {
            InternalChildren = new[]
            {
                previewTextArea = new Container
                {
                    Alpha = 0,
                },
                indexRangeBackground = new Container
                {
                    Masking = true,
                    BorderThickness = 3,
                    Alpha = 0,
                    BorderColour = Color4.White,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Alpha = 0f,
                            AlwaysPresent = true,
                        },
                    }
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colour)
        {
            indexRangeBackground.Colour = colour.Pink;
        }

        protected override void OnSelected()
        {
            indexRangeBackground.FadeIn(500);
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
                var textTagRect = karaokeSpriteText.GetTextTagPosition(Item);

                var startIndexPosition = karaokeSpriteText.GetTextIndexPosition(TextIndexUtils.FromStringIndex(Item.StartIndex, false));
                var endIndexPosition = karaokeSpriteText.GetTextIndexPosition(TextIndexUtils.FromStringIndex(Item.EndIndex, true));

                // update select position
                updateDrawableRect(previewTextArea, textTagRect);

                // update index range position.
                var indexRangePosition = new Vector2(startIndexPosition.X, textTagRect.Y);
                var indexRangeSize = new Vector2(endIndexPosition.X - startIndexPosition.X, textTagRect.Height);
                updateDrawableRect(indexRangeBackground, new RectangleF(indexRangePosition, indexRangeSize));
            });

            static void updateDrawableRect(Drawable target, RectangleF rect)
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
