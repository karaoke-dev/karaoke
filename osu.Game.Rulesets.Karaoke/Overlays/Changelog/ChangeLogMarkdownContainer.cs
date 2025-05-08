// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using Markdig.Syntax.Inlines;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Layout;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers.Markdown;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Overlays.Changelog;

public partial class ChangeLogMarkdownContainer : OsuMarkdownContainer
{
    public ChangeLogMarkdownContainer(APIChangelogBuild build)
    {
        DocumentUrl = build.DocumentUrl;
        RootUrl = build.RootUrl;
        Text = build.GetFormattedContent();
    }

    public override OsuMarkdownTextFlowContainer CreateTextFlow() => new ChangeLogMarkdownTextFlowContainer();

    public override SpriteText CreateSpriteText() => base.CreateSpriteText().With(s =>
    {
        s.Font = OsuFont.GetFont(size: 16, weight: FontWeight.Regular);
    });

    /// <summary>
    /// Re-calculate image size by changelog width.
    /// </summary>
    public partial class ChangeLogMarkdownTextFlowContainer : OsuMarkdownTextFlowContainer
    {
        protected override void AddImage(LinkInline linkInline) => AddDrawable(new ChangeLogMarkdownImage(linkInline));

        protected override void AddLinkText(string text, LinkInline linkInline)
        {
            if (linkInline.Url == null)
                return;

            var pullRequestInfo = ChangelogPullRequestInfo.GetPullRequestInfoFromLink(text, linkInline.Url);

            if (pullRequestInfo != null)
            {
                addPullRequestInfo(pullRequestInfo);
                return;
            }

            var badgeInfo = ChangelogBadgeInfo.GetBadgeInfoFromLink(text);

            if (badgeInfo != null)
            {
                addBadgeInfo(badgeInfo);
                return;
            }

            base.AddLinkText(text, linkInline);
        }

        private void addPullRequestInfo(ChangelogPullRequestInfo pullRequestInfo)
        {
            var pullRequests = pullRequestInfo.PullRequests;
            var users = pullRequestInfo.Users;

            if (pullRequests.Any())
            {
                AddText("(");

                for (int index = 0; index < pullRequests.Length; index++)
                {
                    var pullRequest = pullRequests[index];
                    AddDrawable(new OsuMarkdownLinkText($"{pullRequestInfo.RepoName}#{pullRequest.Number}", new LinkInline
                    {
                        Url = pullRequest.Url,
                    }));

                    if (index != pullRequests.Length - 1)
                        AddText(", ");
                }

                AddText(")");
            }

            foreach (var user in users)
            {
                var textScale = new Vector2(0.7f);
                AddText("    by ", t =>
                {
                    t.Scale = textScale;
                    t.Padding = new MarginPadding { Top = 6 };
                });
                AddDrawable(new UserLinkText(user.UserName, new LinkInline
                {
                    Url = user.Url,
                })
                {
                    Scale = textScale,
                });
            }
        }

        private void addBadgeInfo(ChangelogBadgeInfo badgeInfo)
        {
            AddDrawable(new Badge
            {
                BadgeText = badgeInfo.Text,
                BadgeColour = badgeInfo.Color,
            });
        }

        /// <summary>
        /// Override <see cref="OsuMarkdownImage"/> to limit image display size
        /// </summary>
        /// <returns></returns>
        private partial class ChangeLogMarkdownImage : OsuMarkdownImage
        {
            private readonly LayoutValue widthSizeCache = new(Invalidation.DrawSize);

            public ChangeLogMarkdownImage(LinkInline linkInline)
                : base(linkInline)
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
                float scale = Math.Min(1, DrawWidth / InternalChild.Width);

                InternalChild.Scale = new Vector2(scale);
                Height = InternalChild.Height * scale;
            }
        }

        private partial class UserLinkText : OsuMarkdownLinkText
        {
            public UserLinkText(string text, LinkInline linkInline)
                : base(text, linkInline)
            {
                Padding = new MarginPadding { Top = 6 };
            }
        }

        private partial class Badge : CompositeDrawable
        {
            private readonly Box background;
            private readonly OsuSpriteText text;

            public Badge()
            {
                AutoSizeAxes = Axes.Both;
                Masking = true;
                CornerRadius = 5;

                InternalChildren = new Drawable[]
                {
                    background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.White,
                    },
                    text = new OsuSpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Font = OsuFont.GetFont(size: 12, weight: FontWeight.Bold),
                        Margin = new MarginPadding { Horizontal = 5, Vertical = 3 },
                    },
                };
            }

            public ColourInfo BadgeColour
            {
                get => background.Colour;
                set => background.Colour = value;
            }

            public string BadgeText
            {
                get => text.Text.ToString();
                set => text.Text = value;
            }
        }
    }
}
