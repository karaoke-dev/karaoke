// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Layout
{
    internal class LayoutSection : Container
    {
        private readonly FillFlowContainer flow;

        [Resolved]
        protected OsuColour Colours { get; private set; }

        [Resolved]
        protected IBindable<WorkingBeatmap> Beatmap { get; private set; }

        protected override Container<Drawable> Content => flow;

        public LayoutSection()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            Padding = new MarginPadding(10);

            InternalChild = flow = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Spacing = new Vector2(10),
                Direction = FillDirection.Vertical,
            };
        }
    }
}
