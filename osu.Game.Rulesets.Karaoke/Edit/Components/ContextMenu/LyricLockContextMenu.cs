// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using System.Linq;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.ContextMenu
{
    public class LyricLockContextMenu : OsuMenuItem
    {
        public LyricLockContextMenu(ILockChangeHandler lockChangeHandler, Lyric lyric, string name)
            : this(lockChangeHandler, new List<Lyric> { lyric }, name)
        {
        }

        public LyricLockContextMenu(ILockChangeHandler lockChangeHandler, List<Lyric> lyrics, string name)
            : base(name)
        {
            Items = EnumUtils.GetValues<LockState>().Select(l => new OsuMenuItem(l.ToString(), anyLyricInLockState(l) ? MenuItemType.Highlighted : MenuItemType.Standard, () =>
            {
                // todo: how to make lyric as selected?
                lockChangeHandler.Lock(l);
            })).ToList();

            bool anyLyricInLockState(LockState lockState) => lyrics.Any(lyric => lyric.Lock == lockState);
        }
    }
}
