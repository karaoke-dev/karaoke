// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using ManagedBass;
using osu.Framework.Audio.Callbacks;
using osu.Framework.Audio.Track;
using osu.Game.Replays;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Replays;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace osu.Game.Rulesets.Karaoke.Replays
{
    public class KaraokeAutoGeneratorBySinger : AutoGenerator
    {
        /// <summary>
        /// Points are initially generated to a 1ms resolution to cover most use cases.
        /// </summary>
        private const float resolution = 0.001f;


        /// <summary>
        /// The data stream is iteratively decoded to provide this many points per iteration so as to not exceed BASS's internal buffer size.
        /// </summary>
        private const int points_per_iteration = 100000;

        private readonly CancellationTokenSource cancelSource = new CancellationTokenSource();
        private readonly Task readTask;

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
                // for the time being, this code cannot run if there is no bass device available.
                if (Bass.CurrentDevice <= 0)
                    return;

                int decodeStream = 0;
                using (var fileCallbacks = new FileCallbacks(new DataStreamFileProcedures(data)))
                {
                    decodeStream = Bass.CreateStream(StreamSystem.NoBuffer, BassFlags.Decode | BassFlags.Float, fileCallbacks.Callbacks, fileCallbacks.Handle);
                }

                Bass.ChannelGetInfo(decodeStream, out ChannelInfo info);

                long length = Bass.ChannelGetLength(decodeStream);

                // Each "point" is generated from a number of samples, each sample contains a number of channels
                int samplesPerPoint = (int)(info.Frequency * resolution * info.Channels);

                int bytesPerPoint = samplesPerPoint * TrackBass.BYTES_PER_SAMPLE;

                // Each iteration pulls in several samples
                int bytesPerIteration = bytesPerPoint * points_per_iteration;
                var sampleBuffer = new float[bytesPerIteration / TrackBass.BYTES_PER_SAMPLE];

                // Read sample data
                while (length > 0)
                {
                    length = Bass.ChannelGetData(decodeStream, sampleBuffer, bytesPerIteration);

                    // todo : convert pitch from here
                }
            }, cancelSource.Token);
        }

        public override Replay Generate()
        {
            return new Replay
            {
                Frames = new List<ReplayFrame>()
            };
        }
    }
}
