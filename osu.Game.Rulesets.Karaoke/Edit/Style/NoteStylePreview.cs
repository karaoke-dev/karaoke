// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.UI.Scrolling.Algorithms;

namespace osu.Game.Rulesets.Karaoke.Edit.Style
{
    public class NoteStylePreview : Container
    {
        private const int preview_column = 9;

        [Cached(Type = typeof(IScrollingInfo))]
        private readonly PreviewScrollingInfo scrollingInfo = new PreviewScrollingInfo();

        [Cached(Type = typeof(INotePositionInfo))]
        private readonly PreviewNotePositionInfo positionCalculator = new PreviewNotePositionInfo();

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            var configCache = parent.Get<RulesetConfigCache>();
            var config = (KaraokeRulesetConfigManager)configCache.GetConfigFor(new KaraokeRuleset());
            var dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
            dependencies.Cache(new KaraokeSessionStatics(config, null));

            return dependencies;
        }

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider, StyleManager manager)
        {
            Masking = true;
            CornerRadius = 15;
            FillMode = FillMode.Fit;
            FillAspectRatio = 2f;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = colourProvider.Background1,
                },
                new PreviewDrawableNoteArea
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                }
            };
        }

        public class PreviewDrawableNoteArea : NotePlayfield
        {
            public PreviewDrawableNoteArea()
                : base(preview_column)
            {
            }
        }

        public class PreviewScrollingInfo : IScrollingInfo
        {
            public IBindable<ScrollingDirection> Direction { get; } = new Bindable<ScrollingDirection>();

            public IBindable<double> TimeRange { get; } = new BindableDouble();

            public IScrollAlgorithm Algorithm { get; } = new ZeroScrollAlgorithm();

            private class ZeroScrollAlgorithm : IScrollAlgorithm
            {
                protected const double START_TIME = 1000000000;

                public double GetDisplayStartTime(double originTime, float offset, double timeRange, float scrollLength)
                    => double.MinValue;

                public float GetLength(double startTime, double endTime, double timeRange, float scrollLength)
                    => scrollLength;

                public float PositionAt(double time, double currentTime, double timeRange, float scrollLength)
                    => (float)((time - START_TIME) / timeRange) * scrollLength;

                public double TimeAt(float position, double currentTime, double timeRange, float scrollLength)
                    => 0;

                public void Reset()
                {
                }
            }
        }

        private class PreviewNotePositionInfo : INotePositionInfo
        {
            public IBindable<NotePositionCalculator> Position { get; } = new Bindable<NotePositionCalculator>(new NotePositionCalculator(preview_column, DefaultColumnBackground.COLUMN_HEIGHT, ScrollingNotePlayfield.COLUMN_SPACING));
        }
    }
}
