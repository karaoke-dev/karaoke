// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Components.ContextMenu;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.FixedInfo
{
    public class LockInfo : SpriteIcon, IHasContextMenu
    {
        [Resolved]
        private LyricManager lyricManager { get; set; }

        [Resolved]
        private KaraokeRulesetEditConfigManager configManager { get; set; }

        public MenuItem[] ContextMenuItems => new LyricLockContextMenu(lyricManager, lyric, "Lock").Items.ToArray();

        private readonly Lyric lyric;

        public LockInfo(Lyric lyric)
        {
            this.lyric = lyric;

            Size = new Vector2(12);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            lyric.LockBindable.BindValueChanged(value =>
            {
                switch (value.NewValue)
                {
                    case LockState.None:
                        Icon = FontAwesome.Solid.Unlock;
                        Colour = colours.Green;
                        break;

                    case LockState.Partial:
                        Icon = FontAwesome.Solid.Lock;
                        Colour = colours.Yellow;
                        break;

                    case LockState.Full:
                        Icon = FontAwesome.Solid.Lock;
                        Colour = colours.Red;
                        return;
                }
            }, true);
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (lyric.Lock == LockState.None)
            {
                // change the state by config.
                var newLockState = configManager.Get<LockState>(KaraokeRulesetEditSetting.ClickToLockLyricState);
                lyricManager.LockLyric(lyric, newLockState);
            }
            else
            {
                lyricManager.UnlockLyric(lyric);
            }

            return base.OnClick(e);
        }
    }
}
