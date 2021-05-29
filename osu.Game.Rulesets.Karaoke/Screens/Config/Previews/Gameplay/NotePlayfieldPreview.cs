// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Lists;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Timing;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.UI.Scrolling.Algorithms;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Previews.Gameplay
{
    public class NotePlayfieldPreview : SettingsSubsectionPreview
    {
        private const int row_amount = 9;

        /// <summary>
        /// The default span of time visible by the length of the scrolling axes.
        /// This is clamped between <see cref="time_span_min"/> and <see cref="time_span_max"/>.
        /// </summary>
        private const double time_span_default = 1500;

        /// <summary>
        /// The minimum span of time that may be visible by the length of the scrolling axes.
        /// </summary>
        private const double time_span_min = 50;

        /// <summary>
        /// The maximum span of time that may be visible by the length of the scrolling axes.
        /// </summary>
        private const double time_span_max = 10000;

        [Cached(Type = typeof(IScrollingInfo))]
        private readonly LocalScrollingInfo scrollingInfo;

        [Cached(Type = typeof(IPositionCalculator))]
        private readonly PositionCalculator positionCalculator;

        /// <summary>
        /// The <see cref="MultiplierControlPoint"/>s that adjust the scrolling rate of <see cref="HitObject"/>s inside this <see cref="DrawableRuleset"/>.
        /// </summary>
        protected readonly SortedList<MultiplierControlPoint> ControlPoints = new SortedList<MultiplierControlPoint>(Comparer<MultiplierControlPoint>.Default);

        protected readonly Bindable<ScrollingDirection> Direction = new Bindable<ScrollingDirection>();

        private readonly Bindable<KaraokeScrollingDirection> configDirection = new Bindable<KaraokeScrollingDirection>();

        /// <summary>
        /// The span of time that is visible by the length of the scrolling axes.
        /// For example, only hit objects with start time less than or equal to 1000 will be visible with <see cref="TimeRange"/> = 1000.
        /// </summary>
        protected readonly BindableDouble TimeRange = new BindableDouble(time_span_default)
        {
            Default = time_span_default,
            MinValue = time_span_min,
            MaxValue = time_span_max
        };

        private readonly NotePlayfield notePlayfield;

        public NotePlayfieldPreview()
        {
            Size = new Vector2(0.7f, 0.5f);

            positionCalculator = new PositionCalculator(row_amount);
            scrollingInfo = new LocalScrollingInfo { Algorithm = new SequentialScrollAlgorithm(ControlPoints) };
            scrollingInfo.Direction.BindTo(Direction);
            scrollingInfo.TimeRange.BindTo(TimeRange);

            Child = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(30),
                Child = notePlayfield = new NotePlayfield(row_amount)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                }
            };

            var beatmap = createSampleBeatmap();
            var barLines = new BarLineGenerator<BarLine>(beatmap).BarLines;

            foreach (var hitObject in beatmap.HitObjects)
            {
                // todo : should support pooling.
                var drawableNote = new DrawableNote(hitObject as Note);
                notePlayfield.Add(drawableNote);
            }

            foreach (var barLine in barLines)
            {
                // notePlayfield.Add(barLine);
            }
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Config.BindWith(KaraokeRulesetSetting.ScrollDirection, configDirection);
            configDirection.BindValueChanged(direction => Direction.Value = (ScrollingDirection)direction.NewValue, true);

            Config.BindWith(KaraokeRulesetSetting.ScrollTime, TimeRange);
        }

        private IBeatmap createSampleBeatmap()
        {
            var hitObjects = new List<HitObject>(new HitObject[100]).Select((x, i) => new Note
            {
                StartTime = i * 2000,
                EndIndex = i * 2000 + 1000,
                Text = "Note",
                HitWindows = new KaraokeHitWindows(),
            }).OfType<HitObject>().ToList();

            var controlPointInfo = new ControlPointInfo();
            controlPointInfo.Add(0, new TimingControlPoint());

            return new Beatmap
            {
                HitObjects = hitObjects,
                ControlPointInfo = controlPointInfo,
            };
        }

        private class LocalScrollingInfo : IScrollingInfo
        {
            public IBindable<ScrollingDirection> Direction { get; } = new Bindable<ScrollingDirection>();

            public IBindable<double> TimeRange { get; } = new BindableDouble();

            public IScrollAlgorithm Algorithm { get; set; }
        }
    }
}
