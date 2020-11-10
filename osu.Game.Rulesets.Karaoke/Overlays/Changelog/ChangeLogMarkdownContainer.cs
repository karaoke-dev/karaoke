// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using Markdig;
using Markdig.Extensions.AutoIdentifiers;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Containers.Markdown;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Layout;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Overlays.Changelog
{
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

        protected override void AddMarkdownComponent(IMarkdownObject markdownObject, FillFlowContainer container, int level)
        {
            // hide hidden message in markdown document
            if (markdownObject is YamlFrontMatterBlock)
                return;

            base.AddMarkdownComponent(markdownObject, container, level);
        }

        /// <summary>
        /// Override <see cref="MarkdownHeading"/> to change default heading size.
        /// </summary>
        /// <param name="headingBlock"></param>
        /// <returns></returns>
        protected override MarkdownHeading CreateHeading(HeadingBlock headingBlock) => new ChangeLogMarkdownHeading(headingBlock);

        /// <summary>
        /// Override <see cref="MarkdownTextFlowContainer"/> to limit image display size
        /// </summary>
        /// <returns></returns>
        public override MarkdownTextFlowContainer CreateTextFlow() => new ChangeLogMarkdownTextFlowContainer();

        /// <summary>
        /// Override <see cref="MarkdownParagraph"/> to add dot before text.
        /// </summary>
        /// <param name="paragraphBlock"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        protected override MarkdownParagraph CreateParagraph(ParagraphBlock paragraphBlock, int level) => new ChangeLogMarkdownParagraph(paragraphBlock);

        /// <summary>
        /// Add dot before paragraph.
        /// </summary>
        public class ChangeLogMarkdownParagraph : MarkdownParagraph
        {
            private readonly bool displayDot;

            public ChangeLogMarkdownParagraph(ParagraphBlock paragraphBlock)
                : base(paragraphBlock)
            {
                displayDot = paragraphBlock == paragraphBlock.Parent[0];
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                if (displayDot)
                {
                    AddInternal(new SpriteIcon
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreRight,
                        Margin = new MarginPadding { Right = 10 },
                        Icon = FontAwesome.Solid.DotCircle,
                        Size = new Vector2(10)
                    });
                }
            }
        }

        /// <summary>
        /// Re-assize heading size.
        /// </summary>
        public class ChangeLogMarkdownHeading : MarkdownHeading
        {
            public ChangeLogMarkdownHeading(HeadingBlock heading)
                : base(heading)
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

        /// <summary>
        /// Re-calculate image size by changelog width.
        /// </summary>
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

                    AddLayout(widthSizeCache);
                }

                private bool imageLoaded;

                protected override void Update()
                {
                    base.Update();

                    // unable to get texture size on OnLoadComplete event, so use this way.
                    if (!imageLoaded && InternalChild.Width != 0)
                    {
                        computeImageSize();
                        imageLoaded = true;
                    }

                    if (!widthSizeCache.IsValid)
                    {
                        computeImageSize();
                        widthSizeCache.Validate();
                    }
                }

                private void computeImageSize()
                {
                    // if image is larger then parent size, then adjust image scale
                    var scale = Math.Min(1, DrawWidth / InternalChild.Width);

                    InternalChild.Scale = new Vector2(scale);
                    Height = InternalChild.Height * scale;
                }
            }

            private readonly IDictionary<string, string> githubUrls = new Dictionary<string, string>
            {
                { "karaoke", "https://github.com/karaoke-dev/karaoke/" },
                { "edge", "https://github.com/karaoke-dev/karaoke" },
                { "github.io", "https://github.com/karaoke-dev/karaoke-dev.github.io" },
                { "launcher", "https://github.com/karaoke-dev/launcher" },
                { "sample", "https://github.com/karaoke-dev/sample-beatmap" },
            };

            protected override void AddLinkText(string text, LinkInline linkInline)
            {
                if (githubUrls.ContainsKey(text))
                {
                    var baseUri = new Uri(githubUrls[text]);

                    // Get hash tag with number
                    const string pattern = @"(\#[0-9]+\b)(?!;)";
                    var issueOrRequests = Regex.Matches(linkInline.Url, pattern, RegexOptions.IgnoreCase);

                    if (!issueOrRequests.Any())
                        return;

                    AddText("(");

                    foreach (var issue in issueOrRequests.Select(x => x.Value))
                    {
                        AddDrawable(new MarkdownLinkText($"{text}{issue}", new LinkInline
                        {
                            Url = new Uri(baseUri, $"pull/{issue.Replace("#", "")}").AbsoluteUri
                        }));

                        if (issue != issueOrRequests.LastOrDefault()?.Value)
                            AddText(", ");
                    }

                    AddText(")");

                    // add use name if has user
                    var user = linkInline.Url.Split('@').LastOrDefault();

                    if (!string.IsNullOrEmpty(user))
                    {
                        var textScale = new Vector2(0.7f);
                        AddText(" by:", text => { text.Scale = textScale; });
                        AddDrawable(new MarkdownLinkText(user, new LinkInline
                        {
                            Url = $"https://github.com/{user}"
                        })
                        {
                            Scale = textScale,
                        });
                    }
                }
                else
                {
                    base.AddLinkText(text, linkInline);
                }
            }
        }

        protected override MarkdownPipeline CreateBuilder()
            => new MarkdownPipelineBuilder().UseAutoIdentifiers(AutoIdentifierOptions.GitHub)
                                            .UseYamlFrontMatter()
                                            .UseEmojiAndSmiley()
                                            .UseAdvancedExtensions().Build();
    }
}
