// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.ContextMenu
{
    public class SingerContextMenu : OsuMenuItem
    {
        public SingerContextMenu(LyricManager manager, List<Lyric> lyrics, string name, Action postProcess = null)
            : base(name)
        {
            Items = manager.Singers?.Select(singer => new OsuMenuItem(singer.Name, anySingerInLyric(singer) ? MenuItemType.Highlighted : MenuItemType.Standard, () =>
            {
                // if only one lyric
                if (allSingerInLyric(singer))
                {
                    lyrics.ForEach(lyric => manager.RemoveSingerToLyric(singer, lyric));
                }
                else
                {
                    lyrics.ForEach(lyric => manager.AddSingerToLyric(singer, lyric));
                }

                postProcess?.Invoke();
            })).ToList();

            bool anySingerInLyric(Singer singer) => lyrics.Any(lyric => LyricUtils.ContainsSinger(lyric, singer));

            bool allSingerInLyric(Singer singer) => lyrics.All(lyric => LyricUtils.ContainsSinger(lyric, singer));
        }
    }
}
