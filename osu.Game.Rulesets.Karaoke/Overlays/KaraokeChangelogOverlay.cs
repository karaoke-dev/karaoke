// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Overlays.Changelog;

namespace osu.Game.Rulesets.Karaoke.Overlays
{
    public class KaraokeChangelogOverlay : FullscreenOverlay
    {
        protected ChangelogHeader Header;

        private Container<ChangelogContent> content;

        private SampleChannel sampleBack;

        public KaraokeChangelogOverlay()
            : base(OverlayColourScheme.Purple)
        {
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = ColourProvider.Background4,
                },
                new OverlayScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    ScrollbarVisible = false,
                    Child = new ReverseChildIDFillFlowContainer<Drawable>
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Vertical,
                        Children = new Drawable[]
                        {
                            Header = new ChangelogHeader
                            {
                                //ListingSelected = ShowListing,
                            },
                            content = new Container<ChangelogContent>
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                            }
                        },
                    },
                },
            };

            sampleBack = audio.Samples.Get(@"UI/generic-select-soft");
        }
    }
}
