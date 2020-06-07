// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Logging;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Database;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.Resources.Fonts;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Scoring;
using osu.Game.Screens.Play;
using osu.Game.Users;
using System;
using System.IO;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Mods
{
    public class KaraokeModAutoplayBySinger : KaraokeModAutoplay
    {
        public override string Name => "Autoplay by singer";
        public override string Acronym => "ABS";
        public override string Description => "Autoplay mode but replay's record is by singer's voice.";

        public override IconUsage? Icon => KaraokeIcon.ModAutoPlayBySinger;

        private Stream trackData;

        public override Score CreateReplayScore(IBeatmap beatmap) => new Score
        {
            ScoreInfo = new ScoreInfo { User = new User { Username = "karaoke!singer" } },
            Replay = Replay = new KaraokeAutoGeneratorBySinger((KaraokeBeatmap)beatmap, trackData).Generate(),
        };

        public override void ApplyToDrawableRuleset(DrawableRuleset<KaraokeHitObject> drawableRuleset)
        {
            if (!(drawableRuleset is DrawableKaraokeRuleset drawableKaraokeRuleset))
                return;

            if (!(drawableRuleset.Playfield is KaraokePlayfield karaokePlayfield))
                return;

            var accessResourceContainer = new AccessResourceContainer(karaokePlayfield.WorkingBeatmap.BeatmapInfo);
            drawableKaraokeRuleset.Overlays.Add(accessResourceContainer);
            trackData = accessResourceContainer.TrackData;

            base.ApplyToDrawableRuleset(drawableRuleset);
        }

        protected class AccessResourceContainer : Container
        {
            protected readonly BeatmapSetInfo BeatmapSetInfo;

            protected readonly BeatmapMetadata Metadata;

            private IResourceStore<byte[]> store;

            public Stream TrackData;

            public AccessResourceContainer(BeatmapInfo beatmapInfo)
            {
                BeatmapSetInfo = beatmapInfo.BeatmapSet;
                Metadata = beatmapInfo.Metadata;
            }

            [BackgroundDependencyLoader(true)]
            private void load(Storage storage, IDatabaseContextFactory contextFactory)
            {
                var files = new FileStore(contextFactory, storage);
                store = files.Store;
                TrackData = store.GetStream(getPathForFile(Metadata.AudioFile));
            }

            /// <summary>
            /// Copied from <see cref="WorkingBeatmap"/>
            /// </summary>
            /// <param name="filename"></param>
            /// <returns></returns>
            private string getPathForFile(string filename) => BeatmapSetInfo.Files.FirstOrDefault(f => string.Equals(f.Filename, filename, StringComparison.OrdinalIgnoreCase))?.FileInfo.StoragePath;
        }
    }
}
