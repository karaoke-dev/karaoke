// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.ContextMenu
{
    public class SingerContextMenu : OsuMenuItem
    {
        public SingerContextMenu(EditorBeatmap beatmap, ILyricSingerChangeHandler lyricSingerChangeHandler, string name, Action postProcess = null)
            : base(name)
        {
            var lyrics = beatmap.SelectedHitObjects.OfType<Lyric>().ToArray();
            var singers = (beatmap.PlayableBeatmap as KaraokeBeatmap)?.Singers;

            Items = singers?.Select(singer => new OsuMenuItem(singer.Name, anySingerInLyric(singer) ? MenuItemType.Highlighted : MenuItemType.Standard, () =>
            {
                // if only one lyric
                if (allSingerInLyric(singer))
                {
                    lyricSingerChangeHandler.Remove(singer);
                }
                else
                {
                    lyricSingerChangeHandler.Add(singer);
                }

                postProcess?.Invoke();
            })).ToList();

            bool anySingerInLyric(Singer singer) => lyrics.Any(lyric => LyricUtils.ContainsSinger(lyric, singer));

            bool allSingerInLyric(Singer singer) => lyrics.All(lyric => LyricUtils.ContainsSinger(lyric, singer));
        }
    }
}
