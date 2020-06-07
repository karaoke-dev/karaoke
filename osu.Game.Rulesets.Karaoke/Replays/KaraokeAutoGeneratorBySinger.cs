// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using ManagedBass;
using NWaves.Features;
using osu.Framework.Audio.Callbacks;
using osu.Framework.Audio.Track;
using osu.Game.Replays;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Replays;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace osu.Game.Rulesets.Karaoke.Replays
{
    public class KaraokeAutoGeneratorBySinger : AutoGenerator
    {
        private readonly CancellationTokenSource cancelSource = new CancellationTokenSource();
        private readonly Task<Dictionary<double,float?>> readTask;

        /// <summary>
        /// Using audio's vioce to generate replay frames
        /// Logic is copird from <see cref="Waveform"/>
        /// </summary>
        /// <param name="beatmap"></param>
        /// <param name="data"></param>
        public KaraokeAutoGeneratorBySinger(KaraokeBeatmap beatmap, Stream data)
           : base(beatmap)
        {
            if (data == null)
                return;

            readTask = Task.Run(() =>
            {
                int decodeStream = 0;
                using (var fileCallbacks = new FileCallbacks(new DataStreamFileProcedures(data)))
                {
                    decodeStream = Bass.CreateStream(StreamSystem.NoBuffer, BassFlags.Decode | BassFlags.Float, fileCallbacks.Callbacks, fileCallbacks.Handle);
                }

                Bass.ChannelGetInfo(decodeStream, out ChannelInfo info);

                var totalLength = Bass.ChannelGetLength(decodeStream);
                double trackLength = Bass.ChannelBytes2Seconds(decodeStream, totalLength) * 1000;
                var length = totalLength;
                long lengthSum = 0;

                // Microphone at period 10
                var bytesPerIteration = 3276 * info.Channels * TrackBass.BYTES_PER_SAMPLE;

                var pitches = new Dictionary<double, float?>();
                var sampleBuffer = new float[bytesPerIteration / TrackBass.BYTES_PER_SAMPLE];

                // Read sample data
                while (length > 0)
                {
                    length = Bass.ChannelGetData(decodeStream, sampleBuffer, bytesPerIteration);
                    lengthSum += length;

                    // usually sample 1 is vocal
                    var channel0Sample = sampleBuffer.Where((x, i) => i % 2 == 0).ToArray();
                    //var channel1Sample = sampleBuffer.Where((x, i) => i % 2 != 0).ToArray();

                    // Convert buffer to pitch data
                    var time = lengthSum * trackLength / totalLength;
                    var pitch = Pitch.FromYin(channel0Sample, info.Frequency, low: 40, high: 1000);
                    pitches.Add(time, pitch == 0 ? default(float?) : pitch);
                }

                return pitches;
            }, cancelSource.Token);
        }

        public override Replay Generate()
        {
            var result = readTask.Result;
            return new Replay
            {
                Frames = getReplayFrames(result).ToList()
            };
        }

        private IEnumerable<ReplayFrame> getReplayFrames(IDictionary<double, float?> pitches)
        {
            var lastPitch = pitches.FirstOrDefault();
            foreach (var pitch in pitches)
            {
                if (pitch.Value != null)
                {
                    var scale = ((pitch.Value ?? 0) - 80) / 7;
                    yield return new KaraokeReplayFrame(pitch.Key, scale);
                }
                else if(lastPitch.Value != null)
                    yield return new KaraokeReplayFrame(pitch.Key);

                lastPitch = pitch;
            }
        }
    }
}
