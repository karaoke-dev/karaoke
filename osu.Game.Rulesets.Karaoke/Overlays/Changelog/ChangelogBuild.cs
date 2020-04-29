// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Containers.Markdown;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Layout;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;
using osuTK.Graphics;
using System;
using System.Net.Http;

namespace osu.Game.Rulesets.Karaoke.Overlays.Changelog
{
    /// <summary>
    /// Display full content in <see cref="KaraokeChangelogBuild"/>
    /// </summary>
    public class ChangelogBuild : FillFlowContainer
    {
        public const float HORIZONTAL_PADDING = 70;

        public Action<KaraokeChangelogBuild> SelectBuild;

        protected readonly KaraokeChangelogBuild Build;

        public readonly ChangeLogMarkdownContainer ChangelogEntries;

        public ChangelogBuild(KaraokeChangelogBuild build)
        {
            Build = build;

            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Direction = FillDirection.Vertical;
            Padding = new MarginPadding { Horizontal = HORIZONTAL_PADDING };

            Children = new Drawable[]
            {
                CreateHeader(),
                ChangelogEntries = new ChangeLogMarkdownContainer(build)
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                },
            };
        }

        public class ChangeLogMarkdownContainer : MarkdownContainer
        {
            public ChangeLogMarkdownContainer(KaraokeChangelogBuild build)
            {
                DocumentUrl = build.DocumentUrl;
                RootUrl = build.RootUrl;

                using (var httpClient = new HttpClient())
                {
                    Text = httpClient.GetStringAsync(build.ReadmeDownloadUrl).Result;
                }
            }

            protected override MarkdownHeading CreateHeading(HeadingBlock headingBlock) => new ChangeLogMarkdownHeading(headingBlock);

            public override MarkdownTextFlowContainer CreateTextFlow() => new ChangeLogMarkdownTextFlowContainer();

            public class ChangeLogMarkdownHeading : MarkdownHeading
            {
                public ChangeLogMarkdownHeading(HeadingBlock heading)
                    :base(heading)
                {
                }

                protected override float GetFontSizeByLevel(int level)
                {
                    switch (level)
                    {
                        case 1:
                            return 1.7f;

                        case 2:
                            return 1.5f;

                        case 3:
                            return 1.3f;

                        case 4:
                            return 1.2f;

                        default:
                            return 1;
                    }
                }
            }

            public class ChangeLogMarkdownTextFlowContainer : MarkdownTextFlowContainer
            {
                protected override void AddImage(LinkInline linkInline) => AddDrawable(new ChangeLogMarkdownImage(linkInline.Url));

                public class ChangeLogMarkdownImage : MarkdownImage
                {
                    private readonly LayoutValue widthSizeCache = new LayoutValue(Invalidation.DrawSize);

                    public ChangeLogMarkdownImage(string url)
                        : base(url)
                    {
                        AutoSizeAxes = Axes.None;
                        RelativeSizeAxes = Axes.X;
                    }

                    protected override void Update()
                    {
                        base.Update();

                        if (!widthSizeCache.IsValid)
                        {
                            computeImageSize();
                            widthSizeCache.Validate();
                        }
                    }

                    private void computeImageSize()
                    {
                        // if image is larger then parent size, then change into max size instead
                        var imageWidth = InternalChild.Width;
                        if (imageWidth > DrawWidth)
                        {

                        }
                        else
                        {

                        }

                        Height = 100;
                    }
                }
            }
        }

        protected virtual FillFlowContainer CreateHeader() => new FillFlowContainer
        {
            Anchor = Anchor.TopCentre,
            Origin = Anchor.TopCentre,
            AutoSizeAxes = Axes.Both,
            Direction = FillDirection.Horizontal,
            Margin = new MarginPadding { Top = 20 },
            Children = new Drawable[]
            {
                new OsuHoverContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    AutoSizeAxes = Axes.Both,
                    Action = () => SelectBuild?.Invoke(Build),
                    Child = new FillFlowContainer<SpriteText>
                    {
                        AutoSizeAxes = Axes.Both,
                        Margin = new MarginPadding { Horizontal = 40 },
                        Children = new[]
                        {
                            new OsuSpriteText
                            {
                                Text = "Karaoke!",
                                Font = OsuFont.GetFont(weight: FontWeight.Medium, size: 19),
                            },
                            new OsuSpriteText
                            {
                                Text = " ",
                                Font = OsuFont.GetFont(weight: FontWeight.Medium, size: 19),
                            },
                            new OsuSpriteText
                            {
                                Text = Build.DisplayVersion,
                                Font = OsuFont.GetFont(weight: FontWeight.Light, size: 19),
                                Colour = Color4.Red,
                            },
                        }
                    }
                },
            }
        };
    }
}
