// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Graphics.Cursor
{
    public class SingerToolTip : BackgroundToolTip<ISinger>
    {
        private const int avatar_size = 60;
        private const int main_text_size = 24;
        private const int sub_text_size = 12;

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
                    new GridContainer
                    {
                        Name = "Basic info",
                        RelativeSizeAxes = Axes.X,
                        Height = avatar_size,
                        ColumnDimensions = new[]
                        {
                            new Dimension(GridSizeMode.Absolute, avatar_size),
                            new Dimension(),
                        },
                        Content = new[]
                        {
                            new Drawable[]
                            {
                                avatar = new DrawableSingerAvatar
                                {
                                    Name = "Avatar",
                                    Size = new Vector2(avatar_size),
                                },
                                new Container
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Padding = new MarginPadding { Left = 5 },
                                    Children = new Drawable[]
                                    {
                                        new FillFlowContainer
                                        {
                                            Name = "Singer name",
                                            RelativeSizeAxes = Axes.X,
                                            AutoSizeAxes = Axes.Y,
                                            Direction = FillDirection.Vertical,
                                            Spacing = new Vector2(1),
                                            Children = new[]
                                            {
                                                singerName = new OsuSpriteText
                                                {
                                                    Name = "Singer name",
                                                    Font = OsuFont.GetFont(weight: FontWeight.Bold, size: main_text_size),
                                                    RelativeSizeAxes = Axes.X,
                                                    Truncate = true,
                                                },
                                                singerRomajiName = new OsuSpriteText
                                                {
                                                    Name = "Romaji name",
                                                    Font = OsuFont.GetFont(weight: FontWeight.Bold, size: sub_text_size),
                                                    RelativeSizeAxes = Axes.X,
                                                    Truncate = true,
                                                },
                                            }
                                        },
                                        singerEnglishName = new OsuSpriteText
                                        {
                                            Name = "English name",
                                            Anchor = Anchor.BottomLeft,
                                            Origin = Anchor.BottomLeft,
                                            Font = OsuFont.GetFont(weight: FontWeight.Bold, size: sub_text_size),
                                            RelativeSizeAxes = Axes.X,
                                            Truncate = true,
                                        }
                                    }
                                }
                            }
                        }
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

        private ISinger lastSinger;

        public override void SetContent(ISinger singer)
        {
            if (singer == lastSinger)
                return;

            lastSinger = singer;

            if (singer is not Singer s)
                return;

            // todo: other type of singer(e.g: sub-singer) might display different info.
            avatar.Singer = s;
            singerName.Text = s.Name;
            singerRomajiName.Text = s.RomajiName != null ? $"({s.RomajiName})" : "";
            singerEnglishName.Text = s.EnglishName;
            singerDescription.Text = s.Description ?? "<No description>";
        }
    }
}
