// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class KaraokePlayfield : ScrollingPlayfield
    {
        [Resolved]
        private IBindable<WorkingBeatmap> beatmap { get; set; }

        public WorkingBeatmap WorkingBeatmap => beatmap.Value;

        public Playfield LyricPlayfield { get; }

        public ScrollingNotePlayfield NotePlayfield { get; }

        public BindableBool DisplayCursor { get; set; } = new();
        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => !DisplayCursor.Value && base.ReceivePositionalInputAt(screenSpacePos);

        private readonly BindableInt bindablePitch = new();
        private readonly BindableInt bindableVocalPitch = new();
        private readonly BindableInt bindablePlayback = new();
        private readonly BindableDouble notePlayfieldAlpha = new();
        private readonly BindableDouble lyricPlayfieldAlpha = new();

        public KaraokePlayfield()
        {
            AddInternal(LyricPlayfield = CreateLyricPlayfield().With(x =>
            {
                x.RelativeSizeAxes = Axes.Both;
            }));

            AddInternal(new Container
            {
                Padding = new MarginPadding(50),
                RelativeSizeAxes = Axes.Both,
                Child = NotePlayfield = CreateNotePlayfield(9).With(x =>
                {
                    x.Alpha = 0;
                    x.RelativeSizeAxes = Axes.X;
                })
            });

            AddNested(LyricPlayfield);
            AddNested(NotePlayfield);

            bindablePitch.BindValueChanged(value =>
            {
                // Convert between -10 and 10 into 0.5 and 1.5
                float newValue = 1.0f + (float)value.NewValue / 40;
                WorkingBeatmap.Track.Frequency.Value = newValue;
            });

            bindableVocalPitch.BindValueChanged(value =>
            {
                // TODO : implement until has vocal track
            });

            bindablePlayback.BindValueChanged(value =>
            {
                // Convert between -10 and 10 into 0.5 and 1.5
                float newValue = 1.0f + (float)value.NewValue / 40;
                WorkingBeatmap.Track.Tempo.Value = newValue;
            });

            // Alpha
            notePlayfieldAlpha.BindValueChanged(x =>
            {
                // todo : how to check is there any notes in this playfield?
                double alpha = WorkingBeatmap.Beatmap.IsScorable() ? x.NewValue : 0;
                NotePlayfield.Alpha = (float)alpha;
            });
            lyricPlayfieldAlpha.BindValueChanged(x => LyricPlayfield.Alpha = (float)x.NewValue);
        }

        protected virtual Playfield CreateLyricPlayfield()
            => new LyricPlayfield();

        protected virtual ScrollingNotePlayfield CreateNotePlayfield(int columns)
            => new NotePlayfield(columns);

        #region Pooling support

        public override void Add(HitObject hitObject)
        {
            switch (hitObject)
            {
                case Lyric:
                    LyricPlayfield.Add(hitObject);
                    break;

                case Note:
                case BarLine:
                    NotePlayfield.Add(hitObject);

                    break;

                default:
                    throw new ArgumentException($"Unsupported {nameof(HitObject)} type: {hitObject.GetType()}");
            }
        }

        public override bool Remove(HitObject hitObject)
        {
            switch (hitObject)
            {
                case Lyric:
                    return LyricPlayfield.Remove(hitObject);

                case Note:
                case BarLine:
                    return NotePlayfield.Remove(hitObject);

                default:
                    throw new ArgumentException($"Unsupported {nameof(HitObject)} type: {hitObject.GetType()}");
            }
        }

        #endregion

        #region Non-pooling support

        public override void Add(DrawableHitObject h)
        {
            switch (h)
            {
                case DrawableLyric:
                    LyricPlayfield.Add(h);
                    break;

                case DrawableNote:
                    NotePlayfield.Add(h);

                    break;

                default:
                    base.Add(h);
                    break;
            }
        }

        public override bool Remove(DrawableHitObject h) =>
            h switch
            {
                DrawableLyric => LyricPlayfield.Remove(h),
                DrawableNote => NotePlayfield.Remove(h),
                _ => base.Remove(h)
            };

        #endregion

        public override void PostProcess()
        {
            base.PostProcess();

            // trigger again to update note playfield alpha.
            notePlayfieldAlpha.TriggerChange();
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetConfigManager rulesetConfig, KaraokeSessionStatics session)
        {
            // Cursor
            rulesetConfig?.BindWith(KaraokeRulesetSetting.ShowCursor, DisplayCursor);

            // Alpha
            rulesetConfig?.BindWith(KaraokeRulesetSetting.NoteAlpha, notePlayfieldAlpha);
            rulesetConfig?.BindWith(KaraokeRulesetSetting.LyricAlpha, lyricPlayfieldAlpha);

            // Pitch
            session.BindWith(KaraokeRulesetSession.Pitch, bindablePitch);
            session.BindWith(KaraokeRulesetSession.VocalPitch, bindableVocalPitch);
            session.BindWith(KaraokeRulesetSession.PlaybackSpeed, bindablePlayback);
        }
    }
}
