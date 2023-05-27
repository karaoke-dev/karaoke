// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics.Blueprints;

public abstract partial class TextTagSelectionBlueprint<T> : SelectionBlueprint<T> where T : ITextTag
{
    private readonly Container previewTextArea;
    private readonly Container indexRangeBackground;

    [Resolved]
    private IPreviewLyricPositionProvider previewLyricPositionProvider { get; set; } = null!;

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
    private void load(OsuColour colours)
    {
        indexRangeBackground.Colour = colours.Pink;
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
            var textTagRect = previewLyricPositionProvider.GetTextTagByPosition(Item);

            if (textTagRect == null)
            {
                return;
            }

            var startRect = previewLyricPositionProvider.GetRectByCharIndex(Item.StartIndex);
            var endRect = previewLyricPositionProvider.GetRectByCharIndex(Item.EndIndex);

            // update select position
            updateDrawableRect(previewTextArea, textTagRect.Value);

            // update index range position.
            var indexRangePosition = new Vector2(startRect.Left, textTagRect.Value.Y);
            var indexRangeSize = new Vector2(endRect.Right - startRect.Left, textTagRect.Value.Height);
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
