// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class LyricsProvider : Component, ILyricsProvider
    {
        /// <summary>
        /// Get the bindable lyrics with sorted order.
        /// </summary>
        public BindableList<Lyric> BindableLyrics { get; } = new();

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap beatmap)
        {
            // Load the lyric into bindable list.
            // And notice that order change in the bindable will not affect the order in the editor beatmap.
            // The hit object in the editor beatmap will auto sort by the time.
            var lyrics = OrderUtils.Sorted(beatmap.HitObjects.OfType<Lyric>());
            BindableLyrics.AddRange(lyrics);

            // need to check is there any lyric added or removed.
            beatmap.HitObjectAdded += e =>
            {
                if (e is not Lyric lyric)
                    return;

                var previousLyric = BindableLyrics.LastOrDefault(x => x.Order < lyric.Order);

                if (previousLyric != null)
                {
                    int insertIndex = BindableLyrics.IndexOf(previousLyric) + 1;
                    BindableLyrics.Insert(insertIndex, lyric);
                }
                else
                {
                    // insert to first.
                    BindableLyrics.Insert(0, lyric);
                }
            };
            beatmap.HitObjectRemoved += e =>
            {
                if (e is not Lyric lyric)
                    return;

                BindableLyrics.Remove(lyric);
            };
        }
    }
}
