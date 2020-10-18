// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Layout.Components
{
    public class LayoutPreview : Container
    {
        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider)
        {
            Masking = true;
            CornerRadius = 15;
            FillMode = FillMode.Fit;
            FillAspectRatio = 1.25f;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = colourProvider.Background1,
                },
                new LayoutPreviewArea
                {
                    Name = "Lyric preview area",
                }
            };
        }

        internal class LayoutPreviewArea : Container
        {
            private SkinProvidingContainer layoutArea;
            private DrawableLyricLine drawableLyricLine;

            [BackgroundDependencyLoader]
            private void load(OverlayColourProvider colourProvider)
            {
                Children = new Drawable[]
                {
                    new Box
                    {
                        Name = "Setting background",
                        RelativeSizeAxes = Axes.Both,
                        Colour = colourProvider.Light4
                    },
                    layoutArea = new SkinProvidingContainer(new LayoutEditorSkin())
                    {
                        RelativeSizeAxes = Axes.Both,
                    }
                };
            }

            public void InitialLyricLine(LyricLine lyricLine)
                => layoutArea.Child = drawableLyricLine = new DrawableLyricLine(lyricLine);
        }
    }
}
