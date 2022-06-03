// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Lists;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osu.Game.Rulesets.Timing;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.UI.Scrolling.Algorithms;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings.Previews.Gameplay
{
    public class NotePlayfieldPreview : SettingsSubsectionPreview
    {
        private const int columns = 9;

        [Cached(typeof(IScrollingInfo))]
        private readonly LocalScrollingInfo scrollingInfo = new();

        [Cached(typeof(INotePositionInfo))]
        private readonly PreviewNotePositionInfo notePositionInfo = new();

        private readonly NotePlayfield notePlayfield;

        public NotePlayfieldPreview()
        {
            Size = new Vector2(0.7f, 0.5f);

            Child = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(30),
                Child = notePlayfield = new NotePlayfield(columns)
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

            if (!(Time.Current > lastCreateSampleTime + 3000))
                return;

            lastCreateSampleTime = Time.Current;

            double startTime = Time.Current + 3000;
            notePlayfield.Add(new Note
            {
                StartTime = startTime,
                Duration = 1000,
                Text = "Note",
                ParentLyric = new Lyric(),
                HitWindows = new KaraokeNoteHitWindows(),
            });

            notePlayfield.Add(new BarLine
            {
                StartTime = startTime,
                Major = true
            });
        }

        private readonly Bindable<KaraokeScrollingDirection> configDirection = new();

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

        private class LocalScrollingInfo : IScrollingInfo
        {
            public IBindable<ScrollingDirection> Direction { get; } = new Bindable<ScrollingDirection>();

            public IBindable<double> TimeRange { get; } = new BindableDouble(1500);

            public IScrollAlgorithm Algorithm { get; } = new SequentialScrollAlgorithm(new SortedList<MultiplierControlPoint>(Comparer<MultiplierControlPoint>.Default));
        }

        private class PreviewNotePositionInfo : INotePositionInfo
        {
            public IBindable<NotePositionCalculator> Position { get; } =
                new Bindable<NotePositionCalculator>(new NotePositionCalculator(columns, DefaultColumnBackground.COLUMN_HEIGHT, ScrollingNotePlayfield.COLUMN_SPACING));

            public NotePositionCalculator Calculator => Position.Value;
        }
    }
}
