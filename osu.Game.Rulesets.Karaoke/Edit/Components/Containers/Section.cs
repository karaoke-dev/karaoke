// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Containers
{
    public abstract class Section : Container
    {
        protected const float SECTION_PADDING = 10;

        protected const float SECTION_SPACING = 10;

        private readonly FillFlowContainer flow;

        [Resolved]
        protected OsuColour Colours { get; private set; }

        protected override Container<Drawable> Content => flow;

        protected abstract string Title { get; }

        protected Section()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            Padding = new MarginPadding(SECTION_PADDING);

            InternalChildren = new Drawable[]
            {
                new OsuSpriteText
                {
                    Font = OsuFont.GetFont(weight: FontWeight.Bold, size: 18),
                    Text = Title,
                },
                flow = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Spacing = new Vector2(SECTION_SPACING),
                    Direction = FillDirection.Vertical,
                    Margin = new MarginPadding { Top = 30 }
                }
            };
        }
    }
}
