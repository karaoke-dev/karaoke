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
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Timing;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.UI.Scrolling.Algorithms;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Previews.Gameplay
{
    public class NotePlayfieldPreview : SettingsSubsectionPreview
    {
        private const int row_amount = 9;

        [Cached(Type = typeof(IScrollingInfo))]
        private readonly LocalScrollingInfo scrollingInfo;

        [Cached(Type = typeof(IPositionCalculator))]
        private readonly PositionCalculator positionCalculator;

        private readonly NotePlayfield notePlayfield;

        public NotePlayfieldPreview()
        {
            Size = new Vector2(0.7f, 0.5f);

            positionCalculator = new PositionCalculator(row_amount);
            scrollingInfo = new LocalScrollingInfo();

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
        }

        private double lastCreateSampleTime;

        protected override void Update()
        {
            base.Update();

            if (Time.Current > lastCreateSampleTime + 3000)
            {
                lastCreateSampleTime = Time.Current;

                var startTime = Time.Current + 3000;
                notePlayfield.Add(new Note
                {
                    StartTime = startTime,
                    Duration = 1000,
                    Text = "Note",
                    HitWindows = new KaraokeHitWindows(),
                });

                notePlayfield.Add(new BarLine
                {
                    StartTime = startTime,
                    Major = true
                });
            }
        }

        private readonly Bindable<KaraokeScrollingDirection> configDirection = new Bindable<KaraokeScrollingDirection>();

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetConfigManager config)
        {
            config.BindWith(KaraokeRulesetSetting.ScrollDirection, configDirection);
            configDirection.BindValueChanged(direction =>
            {
                if (scrollingInfo.Direction is Bindable<ScrollingDirection> bindableScrollingDirection)
                    bindableScrollingDirection.Value = (ScrollingDirection)direction.NewValue;
            }, true);

            config.BindWith(KaraokeRulesetSetting.ScrollTime, scrollingInfo.TimeRange as BindableDouble);
        }

        private IBeatmap createSampleBeatmap()
        {
            var hitObjects = new List<HitObject>(new HitObject[100]).Select((x, i) => new Note
            {
                StartTime = i * 2000,
                Duration = 1000,
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

            public IBindable<double> TimeRange { get; } = new BindableDouble(1500);

            public IScrollAlgorithm Algorithm { get; } = new SequentialScrollAlgorithm(new SortedList<MultiplierControlPoint>(Comparer<MultiplierControlPoint>.Default));
        }
    }
}
