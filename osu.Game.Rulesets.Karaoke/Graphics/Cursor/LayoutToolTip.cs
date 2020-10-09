// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Graphics.Cursor
{
    public class LayoutToolTip : BackgroundToolTip
    {
        private readonly Box background;
        private readonly Box previewLyric;
        private readonly OsuSpriteText notSupportText;

        [Resolved(canBeNull: true)]
        private ISkinSource skinSource { get; set; }

        public LayoutToolTip()
        {
            Children = new Drawable[]
            {
                background = new Box
                {
                    Size = new Vector2(240, 135),
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

        public override bool SetContent(object content)
        {
            if (!(content is LyricLine lyric))
                return false;

            // Get layout
            var layoutIndex = lyric.LayoutIndex;
            var layout = skinSource?.GetConfig<KaraokeSkinLookup, KaraokeLayout>(new KaraokeSkinLookup(KaraokeSkinConfiguration.LyricLayout, layoutIndex)).Value;

            // Display in content
            if (layout == null)
            {
                // mark layout as not supported, or skin is not loaded
                notSupportText.Show();

                if(skinSource == null)
                    notSupportText.Text = "Sorry, skin is not exist.";
                else
                    notSupportText.Text = "Sorry, layout is not exist.";
            }
            else
            {
                // Display box preview position
                previewLyric.Show();
                
                // Set preview width
                previewLyric.Width = lyric.Text?.Length * 10 ?? 100;

                // Set relative position
                previewLyric.Anchor = layout.Alignment;
                previewLyric.Origin = layout.Alignment;

                // Set margin
                var horizontalMargin = layout.HorizontalMargin / 8;
                var verticalMargin = layout.VerticalMargin / 8;
                previewLyric.Margin = new MarginPadding
                {
                    Left = layout.Alignment.HasFlag(Anchor.x0) ? horizontalMargin : 0,
                    Right = layout.Alignment.HasFlag(Anchor.x2) ? horizontalMargin : 0,
                    Top = layout.Alignment.HasFlag(Anchor.y0) ? verticalMargin : 0,
                    Bottom = layout.Alignment.HasFlag(Anchor.y2) ? verticalMargin : 0
                };
            }

            return true;
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.Gray2;
            previewLyric.Colour = colours.Yellow;
        }
    }
}
