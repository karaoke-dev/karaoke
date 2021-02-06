// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.ContextMenu
{
    public class LyricLockContextMenu : OsuMenuItem
    {
        public LyricLockContextMenu(LyricManager manager, Lyric lyric, string name)
            : this(manager, new List<Lyric> { lyric }, name)
        {
        }

        public LyricLockContextMenu(LyricManager manager, List<Lyric> lyrics, string name)
            : base(name)
        {
            Items = EnumUtils.GetValues<LockState>().Select(l => new OsuMenuItem(l.ToString(), anyLyricInLockState(l) ? MenuItemType.Highlighted : MenuItemType.Standard, () =>
            {
                // change all selected lyric state.
                lyrics.ForEach(lyric => manager.LockLyrics(lyrics, l));
            })).ToList();

            bool anyLyricInLockState(LockState lockState) => lyrics.Any(lyric => lyric.Lock == lockState);
        }
    }
}
