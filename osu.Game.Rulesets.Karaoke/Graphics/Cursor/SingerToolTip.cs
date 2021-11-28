// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Graphics.Cursor
{
    public class SingerToolTip : BackgroundToolTip<Singer>
    {
        private readonly DrawableSingerAvatar avatar;
        private readonly OsuSpriteText singerName;
        private readonly OsuSpriteText singerEnglishName;
        private readonly OsuSpriteText singerRomajiName;
        private readonly OsuSpriteText singerDescription;

        public SingerToolTip()
        {
            Child = new FillFlowContainer
            {
                AutoSizeAxes = Axes.Y,
                Width = 300,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(15),
                Children = new Drawable[]
                {
                    avatar = new DrawableSingerAvatar
                    {
                        Name = "Avatar",
                        Size = new Vector2(64)
                    },
                    new FillFlowContainer
                    {
                        Name = "Singer name",
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        Direction = FillDirection.Horizontal,
                        Spacing = new Vector2(5),
                        Children = new[]
                        {
                            singerName = new OsuSpriteText
                            {
                                Name = "Singer name",
                                Font = OsuFont.GetFont(weight: FontWeight.Bold, size: 20),
                            },
                            singerEnglishName = new OsuSpriteText
                            {
                                Name = "English name",
                                Anchor = Anchor.BottomLeft,
                                Origin = Anchor.BottomLeft,
                                Font = OsuFont.GetFont(weight: FontWeight.Bold, size: 13),
                                Margin = new MarginPadding { Bottom = 1 }
                            }
                        }
                    },
                    singerRomajiName = new OsuSpriteText
                    {
                        Name = "Romaji name"
                    },
                    singerDescription = new OsuSpriteText
                    {
                        RelativeSizeAxes = Axes.X,
                        AllowMultiline = true,
                        Colour = Color4.White.Opacity(0.75f),
                        Font = OsuFont.Default.With(size: 14),
                        Name = "Description",
                    }
                }
            };
        }

        public override void SetContent(Singer content)
        {
            avatar.Singer = content;
            singerName.Text = content.Name;
            singerEnglishName.Text = content.EnglishName != null ? $"({content.EnglishName})" : "";
            singerRomajiName.Text = content.RomajiName;
            singerDescription.Text = content.Description ?? "<No description>";
        }
    }
}
