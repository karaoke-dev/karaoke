// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Video;
using osu.Game.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Screens.SaitenAdjustment.Beatmaps
{
    public class SaitenAdjustmentWorkingBeatmap : WorkingBeatmap
    {
        private readonly IBeatmap beatmap;

        public SaitenAdjustmentWorkingBeatmap(IBeatmap beatmap)
            : base(beatmap.BeatmapInfo, null)
        {
            this.beatmap = beatmap;
        }

        protected override Texture GetBackground() => null;

        protected override IBeatmap GetBeatmap() => beatmap;

        // TODO : get real track from resource
        protected override Track GetTrack() => AudioManager.Tracks.GetVirtual(10000);

        protected override VideoSprite GetVideo() => null;
    }
}
