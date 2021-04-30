// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Replays;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Scoring;
using osu.Game.Users;

namespace osu.Game.Rulesets.Karaoke.Mods
{
    public class KaraokeModAutoplay : ModAutoplay<KaraokeHitObject>, IApplicableToMicrophone
    {
        protected Replay Replay;

        public bool MicrophoneEnabled => false;

        public override Score CreateReplayScore(IBeatmap beatmap, IReadOnlyList<Mod> mods) => new Score
        {
            ScoreInfo = new ScoreInfo { User = new User { Username = "osu!7pupu" } },
            Replay = Replay = new KaraokeAutoGenerator((KaraokeBeatmap)beatmap).Generate(),
        };

        public override void ApplyToDrawableRuleset(DrawableRuleset<KaraokeHitObject> drawableRuleset)
        {
            // Got no idea why edit ruleset call this shit.
            if (drawableRuleset is DrawableKaraokeEditorRuleset)
                return;

            base.ApplyToDrawableRuleset(drawableRuleset);

            if (!(drawableRuleset.Playfield is KaraokePlayfield karaokePlayfield))
                return;

            var notePlayfield = karaokePlayfield.NotePlayfield;
            var frames = Replay.Frames.OfType<KaraokeReplayFrame>();

            // for safety purpose should clear reply to make sure not cause crash if apply to ruleset runs more then one times.
            notePlayfield.ClearReplay();

            foreach (var frame in frames)
            {
                notePlayfield.AddReplay(frame);
            }
        }
    }
}
