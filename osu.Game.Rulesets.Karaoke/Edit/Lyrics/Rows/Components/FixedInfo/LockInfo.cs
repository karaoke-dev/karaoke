// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Edit.Components.ContextMenu;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.FixedInfo
{
    public class LockInfo : SpriteIcon, IHasContextMenu
    {
        [Resolved]
        private ILockChangeHandler lockChangeHandler { get; set; }

        [Resolved]
        private ILyricCaretState lyricCaretState { get; set; }

        [Resolved]
        private KaraokeRulesetLyricEditorConfigManager configManager { get; set; }

        public MenuItem[] ContextMenuItems => new LyricLockContextMenu(lockChangeHandler, lyric, "Lock").Items.ToArray();

        private readonly Lyric lyric;

        private readonly IBindable<LockState> bindableLockState;

        public LockInfo(Lyric lyric)
        {
            this.lyric = lyric;
            bindableLockState = lyric.LockBindable.GetBoundCopy();

            Size = new Vector2(12);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            bindableLockState.BindValueChanged(value =>
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

                    default:
                        throw new ArgumentOutOfRangeException(nameof(value.NewValue));
                }
            }, true);
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (bindableLockState.Value == LockState.None)
            {
                // should mark lyric as selected for able to apply the lock state.
                lyricCaretState.MoveCaretToTargetPosition(lyric);

                // change the state by config.
                var newLockState = configManager.Get<LockState>(KaraokeRulesetLyricEditorSetting.ClickToLockLyricState);
                lockChangeHandler.Lock(newLockState);
            }
            else
            {
                lockChangeHandler.Unlock();
            }

            return base.OnClick(e);
        }
    }
}
