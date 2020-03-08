// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Replays;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.Karaoke.Replays
{
    public class KaraokeAutoGenerator : AutoGenerator
    {
        public KaraokeAutoGenerator(KaraokeBeatmap beatmap)
            : base(beatmap)
        {
        }

        public override Replay Generate()
        {
            var notes = Beatmap.HitObjects.OfType<Note>().Where(x => x.Display);
            return new Replay
            {
                Frames = notes.SelectMany((element, index) => getReplayFrames(element, notes.ElementAtOrDefault(index + 1))).ToList()
            };
        }

        private IEnumerable<ReplayFrame> getReplayFrames(Note note, Note next)
        {
            var startTime = note.StartTime;
            var endTime = note.EndTime;

            // Generate frame each 100ms
            for (var i = startTime; i < endTime; i += 100)
            {
                var scale = note.Tone.Scale + (note.Tone.Half ? 0.5f : 0);
                yield return new KaraokeReplayFrame(i, scale);
            }

            if ((next?.StartTime ?? int.MaxValue) - note.EndTime > 500)
            {
                yield return new KaraokeReplayFrame
                {
                    Time = endTime + 1
                };
            }
        }
    }
}
