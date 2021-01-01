// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Infos.MainInfo;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Infos.SubInfo;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Infos
{
    public class InfoControl : Container
    {
        private const int max_height = 120;

        private readonly Box headerBackground;
        private readonly Container subInfoContainer;

        public Lyric Lyric { get; }

        public InfoControl(Lyric lyric)
        {
            Lyric = lyric;

            Children = new Drawable[]
            {
                headerBackground = new Box
                {
                    RelativeSizeAxes = Axes.X,
                    Height = max_height,
                    Alpha = 0.7f
                },
                new FillFlowContainer
                {
                    Direction = FillDirection.Vertical,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Spacing = new Vector2(5),
                    Children = new Drawable[]
                    {
                        new TimeInfoContainer(Lyric)
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 36,
                        },
                        subInfoContainer = new Container
                        {
                            RelativeSizeAxes = Axes.X
                        },
                    }
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, LyricEditorStateManager stateManager)
        {
            headerBackground.Colour = colours.Gray2;
            stateManager.BindableFastEditMode.BindValueChanged(e =>
            {
                CreateBadge(e.NewValue);
            }, true);
        }

        protected void CreateBadge(LyricFastEditMode mode)
        {
            subInfoContainer.Clear();
            var subInfo = createSubInfo();
            if (subInfo == null)
                return;

            subInfo.Margin = new MarginPadding { Right = 5 };
            subInfo.Anchor = Anchor.TopRight;
            subInfo.Origin = Anchor.TopRight;
            subInfoContainer.Add(subInfo);

            Drawable createSubInfo()
            {
                switch (mode)
                {
                    case LyricFastEditMode.None:
                        return null;

                    case LyricFastEditMode.Layout:
                        return new LayoutInfo(Lyric);

                    case LyricFastEditMode.Singer:
                        return new SingerInfo(Lyric);

                    case LyricFastEditMode.Language:
                        return new LanguageInfo(Lyric);

                    case LyricFastEditMode.TimeTag:
                        return new TimeTagInfo(Lyric);

                    default:
                        throw new IndexOutOfRangeException(nameof(mode));
                }
            }
        }
    }
}
