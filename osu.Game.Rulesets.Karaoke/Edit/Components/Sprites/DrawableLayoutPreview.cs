// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions.EnumExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Sprites
{
    public class DrawableLayoutPreview : CompositeDrawable
    {
        private const float scale = 0.4f;

        private readonly Box background;
        private readonly Box previewLyric;
        private readonly OsuSpriteText notSupportText;

        [Resolved(canBeNull: true)]
        private ISkinSource skinSource { get; set; }

        public DrawableLayoutPreview()
        {
            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                previewLyric = new Box
                {
                    Height = 15
                },
                notSupportText = new OsuSpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                }
            };

            previewLyric.Hide();
            notSupportText.Hide();
        }

        private LyricLayout layout;

        public LyricLayout Layout
        {
            get => layout;
            set
            {
                if (layout == value)
                    return;

                layout = value;
                updateLayout();
            }
        }

        private Lyric lyric;

        public Lyric Lyric
        {
            get => lyric;
            set
            {
                if (lyric == value)
                    return;

                lyric = value;
                updateLayout();
            }
        }

        private void updateLayout()
        {
            // Display in content
            if (Layout == null)
            {
                // mark layout as not supported, or skin is not loaded
                notSupportText.Show();

                if (skinSource == null)
                    notSupportText.Text = "Sorry, skin is not exist.";
                else
                    notSupportText.Text = "Sorry, layout is not exist.";
            }
            else
            {
                // Display box preview position
                previewLyric.Show();

                // Set preview width
                const float text_size = 20;
                previewLyric.Width = (Lyric?.Text?.Length ?? 10) * text_size * scale;
                previewLyric.Height = text_size * 1.5f * scale;

                // Set relative position
                previewLyric.Anchor = layout.Alignment;
                previewLyric.Origin = layout.Alignment;

                // Set margin
                const float padding = 30 * scale;
                float horizontalMargin = layout.HorizontalMargin * scale + padding;
                float verticalMargin = layout.VerticalMargin * scale + padding;
                previewLyric.Margin = new MarginPadding
                {
                    Left = layout.Alignment.HasFlagFast(Anchor.x0) ? horizontalMargin : 0,
                    Right = layout.Alignment.HasFlagFast(Anchor.x2) ? horizontalMargin : 0,
                    Top = layout.Alignment.HasFlagFast(Anchor.y0) ? verticalMargin : 0,
                    Bottom = layout.Alignment.HasFlagFast(Anchor.y2) ? verticalMargin : 0
                };
            }
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.Gray2;
            previewLyric.Colour = colours.Yellow;
        }
    }
}
