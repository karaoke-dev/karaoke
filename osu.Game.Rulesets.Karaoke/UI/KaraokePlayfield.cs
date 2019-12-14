// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Framework.Graphics.Containers;
using osuTK;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class KaraokePlayfield : ScrollingPlayfield
    {
        [Resolved]
        private IBindable<WorkingBeatmap> beatmap { get; set; }

        public WorkingBeatmap WorkingBeatmap => beatmap.Value;

        public LyricPlayfield LyricPlayfield => lyricPlayfield;
        private readonly LyricPlayfield lyricPlayfield;

        public NotePlayfield NotePlayfield => notePlayfield;
        private readonly NotePlayfield notePlayfield;

        public BindableBool DisplayCursor { get; set; } = new BindableBool();
        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => DisplayCursor.Value ? false : base.ReceivePositionalInputAt(screenSpacePos);

        public KaraokePlayfield()
        {
            AddInternal(lyricPlayfield = new LyricPlayfield
            {
                RelativeSizeAxes = Axes.Both,
            });

            AddInternal(new Container
            {
                Padding = new MarginPadding(50),
                RelativeSizeAxes = Axes.Both,
                Child = notePlayfield = new NotePlayfield(9)
                {
                    Alpha = 0,
                    RelativeSizeAxes = Axes.X
                }
            });

            AddNested(lyricPlayfield);
            AddNested(notePlayfield);
        }

        public override void Add(DrawableHitObject h)
        {
            if (h is DrawableLyricLine)
                lyricPlayfield.Add(h);
            else if (h is DrawableNote)
            {
                // hidden the note playfield until receive any note.
                notePlayfield.Alpha = 1;
                notePlayfield.Add(h);
            }
            else
                base.Add(h);
        }

        public override bool Remove(DrawableHitObject h)
        {
            if (h is DrawableLyricLine)
                return lyricPlayfield.Remove(h);
            if (h is DrawableNote)
                return notePlayfield.Remove(h);

            return base.Remove(h);
        }

        public void Add(BarLine barline)
        {
            notePlayfield.Add(barline);
        }

        [BackgroundDependencyLoader]
        private void Load(KaraokeRulesetConfigManager rulesetConfig)
        {
            rulesetConfig?.BindWith(KaraokeRulesetSetting.ShowCursor, DisplayCursor);
        }
    }
}
