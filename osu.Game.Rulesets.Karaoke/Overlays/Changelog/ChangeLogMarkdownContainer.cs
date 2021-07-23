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
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Containers.Markdown;
using osu.Framework.Layout;
using osu.Game.Graphics.Containers.Markdown;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Overlays.Changelog
{
    public class ChangeLogMarkdownContainer : OsuMarkdownContainer
    {
        public ChangeLogMarkdownContainer(APIChangelogBuild build)
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
        /// Override <see cref="MarkdownTextFlowContainer"/> to limit image display size
        /// </summary>
        /// <returns></returns>
        public override MarkdownTextFlowContainer CreateTextFlow() => new ChangeLogMarkdownTextFlowContainer();

        /// <summary>
        /// Re-calculate image size by changelog width.
        /// </summary>
        public class ChangeLogMarkdownTextFlowContainer : OsuMarkdownTextFlowContainer
        {
            protected override void AddImage(LinkInline linkInline) => AddDrawable(new ChangeLogMarkdownImage(linkInline.Url));

            public ChangeLogMarkdownTextFlowContainer()
            {
                TextAnchor = Anchor.BottomLeft;
            }

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

                    if (widthSizeCache.IsValid)
                        return;

                    computeImageSize();
                    widthSizeCache.Validate();
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
                { "karaoke-microphone", "https://github.com/karaoke-dev/osu-framework-microphone" },
            };

            protected override void AddLinkText(string text, LinkInline linkInline)
            {
                if (linkInline.Url == null)
                    return;

                if (githubUrls.ContainsKey(text))
                {
                    var baseUri = new Uri(githubUrls[text]);

                    // Get hash tag with number
                    const string issue_regex = @"#(?<issue>[0-9]+)|@(?<username>[0-9A-z]+)";
                    var result = Regex.Matches(linkInline.Url, issue_regex, RegexOptions.IgnoreCase);

                    if (!result.Any())
                        return;

                    // add issue if has user
                    var issues = result.Select(x => x.Groups["issue"]?.Value).Where(x => !string.IsNullOrEmpty(x));

                    if (issues.Any())
                    {
                        AddText("(");

                        foreach (var issue in issues)
                        {
                            if (string.IsNullOrEmpty(issue))
                                continue;

                            AddDrawable(new MarkdownLinkText($"{text}{issue}", new LinkInline
                            {
                                Url = new Uri(baseUri, $"pull/{issue}").AbsoluteUri
                            }));

                            if (issue != issues.LastOrDefault())
                                AddText(", ");
                        }

                        AddText(")");
                    }

                    // add use name if has user
                    var usernames = result.Select(x => x.Groups["username"]?.Value).Where(x => !string.IsNullOrEmpty(x));

                    foreach (var user in usernames)
                    {
                        if (string.IsNullOrEmpty(user))
                            return;

                        var textScale = new Vector2(0.7f);
                        AddText(" by:", t =>
                        {
                            t.Scale = textScale;
                            t.Padding = new MarginPadding { Bottom = 2 };
                        });
                        AddDrawable(new UserLinkText(user, new LinkInline
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

        protected class UserLinkText : MarkdownLinkText
        {
            public UserLinkText(string text, LinkInline linkInline)
                : base(text, linkInline)
            {
                Padding = new MarginPadding { Bottom = 2 };
            }
        }
    }
}
