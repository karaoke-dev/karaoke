// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Extensions;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Karaoke.Mods
{
    public class KaraokeModAutoplayBySinger : KaraokeModAutoplay
    {
        public override string Name => "Autoplay by singer";
        public override string Acronym => "ABS";
        public override string Description => "Autoplay mode but replay's record is by singer's voice.";

        public override IconUsage? Icon => KaraokeIcon.ModAutoPlayBySinger;

        private Stream? trackData;

        public override ModReplayData CreateReplayData(IBeatmap beatmap, IReadOnlyList<Mod> mods)
            => new(new KaraokeAutoGeneratorBySinger(beatmap, trackData).Generate(), new ModCreatedUser { Username = "karaoke!singer" });

        public override void ApplyToDrawableRuleset(DrawableRuleset<KaraokeHitObject> drawableRuleset)
        {
            if (drawableRuleset.Playfield is not KaraokePlayfield karaokePlayfield)
                return;

            var workingBeatmap = karaokePlayfield.WorkingBeatmap;
            string? path = getPathForFile(workingBeatmap.BeatmapInfo);
            trackData = workingBeatmap.GetStream(path);

            base.ApplyToDrawableRuleset(drawableRuleset);
        }

        private string? getPathForFile(BeatmapInfo beatmapInfo)
        {
            var beatmapSetInfo = beatmapInfo.BeatmapSet;
            string audioFile = beatmapInfo.Metadata.AudioFile;

            return beatmapSetInfo?.Files.SingleOrDefault(f => string.Equals(f.Filename, audioFile, StringComparison.OrdinalIgnoreCase))?.File.GetStoragePath();
        }
    }
}
