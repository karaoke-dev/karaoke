// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers
{
    public class BeatmapChangeHandler : Component, IBeatmapChangeHandler
    {
        [Resolved, AllowNull]
        private EditorBeatmap beatmap { get; set; }

        private KaraokeBeatmap karaokeBeatmap => (KaraokeBeatmap)beatmap.PlayableBeatmap;

        public void Import(IBeatmap newBeatmap)
        {
            beatmap.BeginChange();

            beatmap.Clear();

            var lyrics = newBeatmap.HitObjects.OfType<Lyric>().ToArray();

            if (lyrics.Any())
            {
                for (int i = 0; i < lyrics.Length; i++)
                {
                    lyrics[i].ID = i;
                }

                beatmap.AddRange(lyrics);
            }

            beatmap.EndChange();
        }

        public void SetScorable(bool scorable)
        {
            beatmap.BeginChange();

            karaokeBeatmap.Scorable = scorable;

            beatmap.EndChange();
        }
    }
}
