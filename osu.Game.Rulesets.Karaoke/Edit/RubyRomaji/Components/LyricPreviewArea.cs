// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.RubyRomaji.Components
{
    public class LyricPreviewArea : Container
    {
        private readonly Container container;

        public LyricPreviewArea()
        {
            Child = new OsuScrollContainer(Direction.Horizontal)
            {
                RelativeSizeAxes = Axes.Both,
                Child = container = new Container
                {
                    RelativeSizeAxes = Axes.Y,
                    AutoSizeAxes = Axes.X,
                    Padding = new MarginPadding(30),
                }
            };
        }

        private PreviewLyricSpriteText previewLyricLine;

        public Lyric LyricLine
        {
            get => previewLyricLine?.HitObject;
            set => container.Child = previewLyricLine = new PreviewLyricSpriteText(value)
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Font = new FontUsage(size: 100),
                RubyFont = new FontUsage(size: 42),
                RomajiFont = new FontUsage(size: 42)
            };
        }
    }
}
