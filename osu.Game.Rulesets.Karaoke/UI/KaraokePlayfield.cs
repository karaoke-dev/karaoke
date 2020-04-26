// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI.Scrolling;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI
{
    [Cached]
    public class KaraokePlayfield : ScrollingPlayfield
    {
        [Resolved]
        private IBindable<WorkingBeatmap> beatmap { get; set; }

        public WorkingBeatmap WorkingBeatmap => beatmap.Value;

        public LyricPlayfield LyricPlayfield { get; }

        public NotePlayfield NotePlayfield { get; }

        public BindableBool DisplayCursor { get; set; } = new BindableBool();
        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => !DisplayCursor.Value && base.ReceivePositionalInputAt(screenSpacePos);

        private readonly BindableInt bindablePitch = new BindableInt();
        private readonly BindableInt bindableVocalPitch = new BindableInt();
        private readonly BindableInt bindablePlayback = new BindableInt();

        public KaraokePlayfield()
        {
            AddInternal(LyricPlayfield = new LyricPlayfield
            {
                RelativeSizeAxes = Axes.Both,
            });

            AddInternal(new Container
            {
                Padding = new MarginPadding(50),
                RelativeSizeAxes = Axes.Both,
                Child = NotePlayfield = new NotePlayfield(9)
                {
                    Alpha = 0,
                    RelativeSizeAxes = Axes.X
                }
            });

            AddNested(LyricPlayfield);
            AddNested(NotePlayfield);

            bindablePitch.BindValueChanged(value =>
            {
                // Convert between -10 and 10 into 0.5 and 1.5
                var newValue = 1.0f + (float)value.NewValue / 40;
                WorkingBeatmap.Track.Frequency.Value = newValue;
            });

            bindableVocalPitch.BindValueChanged(value =>
            {
                // TODO : implement until has vocal track
            });

            bindablePlayback.BindValueChanged(value =>
            {
                // Convert between -10 and 10 into 0.5 and 1.5
                var newValue = 1.0f + (float)value.NewValue / 40;
                WorkingBeatmap.Track.Tempo.Value = newValue;
            });
        }

        public override void Add(DrawableHitObject h)
        {
            switch (h)
            {
                case DrawableLyricLine _:
                    LyricPlayfield.Add(h);
                    break;

                case DrawableNote _:
                    // hidden the note playfield until receive any note.
                    NotePlayfield.Alpha = 1;
                    NotePlayfield.Add(h);
                    break;

                default:
                    base.Add(h);
                    break;
            }
        }

        public override bool Remove(DrawableHitObject h)
        {
            switch (h)
            {
                case DrawableLyricLine _:
                    return LyricPlayfield.Remove(h);

                case DrawableNote _:
                    return NotePlayfield.Remove(h);

                default:
                    return base.Remove(h);
            }
        }

        public void Add(BarLine barLine)
        {
            NotePlayfield.Add(barLine);
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetConfigManager rulesetConfig, KaraokeSessionStatics session)
        {
            rulesetConfig?.BindWith(KaraokeRulesetSetting.ShowCursor, DisplayCursor);

            // Pitch
            session.BindWith(KaraokeRulesetSession.Pitch, bindablePitch);
            session.BindWith(KaraokeRulesetSession.VocalPitch, bindableVocalPitch);
            session.BindWith(KaraokeRulesetSession.PlaybackSpeed, bindablePlayback);
        }
    }
}
