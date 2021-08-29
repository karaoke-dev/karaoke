// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Platform;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Overlays.Changelog.Sidebar
{
    public class ChangelogSection : CompositeDrawable
    {
        private const int animation_duration = 250;

        public readonly BindableBool Expanded = new(true);

        public ChangelogSection(int year, IEnumerable<APIChangelogBuild> posts)
        {
            Debug.Assert(posts.All(p => p.PublishedAt.Year == year));

            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Masking = true;

            InternalChild = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    new PostsContainer
                    {
                        Expanded = { BindTarget = Expanded },
                        Children = posts.Select(p => new PostButton(p)).ToArray()
                    }
                }
            };
        }

        private class PostButton : OsuHoverContainer
        {
            protected override IEnumerable<Drawable> EffectTargets => new[] { text };

            private readonly TextFlowContainer text;
            private readonly APIChangelogBuild post;

            public PostButton(APIChangelogBuild post)
            {
                this.post = post;

                RelativeSizeAxes = Axes.X;
                AutoSizeAxes = Axes.Y;
                Child = text = new TextFlowContainer(t => t.Font = OsuFont.GetFont(size: 12))
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Text = post.DisplayVersion
                };
            }

            [BackgroundDependencyLoader]
            private void load(OverlayColourProvider overlayColours, GameHost host, Bindable<APIChangelogBuild> current)
            {
                current.BindValueChanged(e =>
                {
                    var isCurrent = post == e.NewValue;

                    // update hover color.
                    Colour = isCurrent ? Color4.White : overlayColours.Light2;
                    HoverColour = isCurrent ? Color4.White : overlayColours.Light1;

                    // update font.
                    text.OfType<SpriteText>().ForEach(f =>
                    {
                        f.Font = OsuFont.GetFont(size: 12, weight: isCurrent ? FontWeight.SemiBold : FontWeight.Medium);
                    });
                }, true);

                TooltipText = "view current changelog";

                Action = () => current.Value = post;
            }
        }

        private class PostsContainer : Container
        {
            public readonly BindableBool Expanded = new();

            protected override Container<Drawable> Content { get; }

            public PostsContainer()
            {
                RelativeSizeAxes = Axes.X;
                AutoSizeAxes = Axes.Y;
                AutoSizeDuration = animation_duration;
                AutoSizeEasing = Easing.Out;
                InternalChild = Content = new FillFlowContainer
                {
                    Margin = new MarginPadding { Top = 5 },
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 5),
                    Alpha = 0
                };
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();
                Expanded.BindValueChanged(updateState, true);
            }

            private void updateState(ValueChangedEvent<bool> expanded)
            {
                ClearTransforms(true);

                if (expanded.NewValue)
                {
                    AutoSizeAxes = Axes.Y;
                    Content.FadeIn(animation_duration, Easing.OutQuint);
                }
                else
                {
                    AutoSizeAxes = Axes.None;
                    this.ResizeHeightTo(0, animation_duration, Easing.OutQuint);

                    Content.FadeOut(animation_duration, Easing.OutQuint);
                }
            }
        }
    }
}
