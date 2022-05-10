// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.UI
{
    [TestFixture]
    public class TestSceneNotePlayfield : OsuTestScene
    {
        public const int COLUMNS = 9;

        [Cached(typeof(IReadOnlyList<Mod>))]
        private IReadOnlyList<Mod> mods { get; set; } = Array.Empty<Mod>();

        [Cached(typeof(INotePositionInfo))]
        private readonly PreviewNotePositionInfo notePositionInfo = new();

        private readonly List<NotePlayfield> notePlayfields = new();

        protected override Ruleset CreateRuleset() => new KaraokeRuleset();

        [BackgroundDependencyLoader]
        private void load()
        {
            var config = Dependencies.Get<KaraokeRulesetConfigManager>();
            Dependencies.Cache(new KaraokeSessionStatics(config, null));

            Child = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Content = new[]
                {
                    new[]
                    {
                        createColumn(ScrollingDirection.Left, COLUMNS),
                    },
                    new[]
                    {
                        createColumn(ScrollingDirection.Right, COLUMNS)
                    }
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AddStep("note", () =>
            {
                createBar(true);
                createNote();
                createBar(true, 3000);
            });
            AddStep("multi note", () =>
            {
                createBar(true);
                createNote(2000, 100, -4);
                createNote(2100, 100, -3);
                createNote(2200, 100, -2);
                createNote(2300, 100, -1);
                createNote(2400, 100);
                createNote(2500, 100, 1);
                createNote(2600, 100, 2);
                createNote(2700, 100, 3);
                createNote(2800, 100, 4);
                createBar(true, 2900);
            });
            AddStep("saiten", () =>
            {
                createBar(true);
                createNote(2000, 100, 4, true);
                createNote(2100, 100, 3, true);
                createNote(2200, 100, 2, true);
                createNote(2300, 100, 1, true);
                createNote(2400, 100, 0, true);
                createNote(2500, 100, -1, true);
                createNote(2600, 100, -2, true);
                createNote(2700, 100, -3, true);
                createNote(2800, 100, -4, true);
                createBar(true, 2900);
            });
            AddStep("bar", () => createBar(false));
            AddStep("major bar", () => createBar(true));
        }

        private void createNote(double increaseTime = 2000, double duration = 1000, int tone = 0, bool saiten = false)
        {
            notePlayfields.ForEach(x =>
            {
                var note = new Note
                {
                    StartTime = Time.Current + increaseTime,
                    Duration = duration,
                    Tone = new Tone { Scale = tone },
                    Text = "Here",
                    ParentLyric = new Lyric(),
                    Display = true
                };
                note.ApplyDefaults(new ControlPointInfo(), new BeatmapDifficulty());

                x.Add(new DrawableNote(note));
            });

            if (saiten)
                createSaitenPath(increaseTime, duration, tone);
        }

        private void createSaitenPath(double increaseTime = 2000, double duration = 1000, int scale = 0)
        {
            notePlayfields.ForEach(x =>
            {
                // Start frame
                x.AddReplay(new KaraokeReplayFrame(Time.Current + increaseTime, scale));

                // End frame
                x.AddReplay(new KaraokeReplayFrame(Time.Current + increaseTime + duration - 2, scale));

                // Stop point
                x.AddReplay(new KaraokeReplayFrame(Time.Current + increaseTime + duration - 1));
            });
        }

        private void createBar(bool isMajor, double increaseTime = 2000)
        {
            notePlayfields.ForEach(x =>
            {
                var bar = new BarLine { StartTime = Time.Current + increaseTime, Major = isMajor };
                bar.ApplyDefaults(new ControlPointInfo(), new BeatmapDifficulty());

                x.Add(bar);
            });
        }

        private Drawable createColumn(ScrollingDirection direction, int column)
        {
            var playfield = new NotePlayfield(column)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            };

            notePlayfields.Add(playfield);

            return new ScrollingTestContainer(direction)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Padding = new MarginPadding(20),
                RelativeSizeAxes = Axes.Both,
                TimeRange = 2000,
                Child = playfield
            };
        }

        private class PreviewNotePositionInfo : INotePositionInfo
        {
            public IBindable<NotePositionCalculator> Position { get; } =
                new Bindable<NotePositionCalculator>(new NotePositionCalculator(COLUMNS, DefaultColumnBackground.COLUMN_HEIGHT, ScrollingNotePlayfield.COLUMN_SPACING));

            public NotePositionCalculator Calculator => Position.Value;
        }
    }
}
