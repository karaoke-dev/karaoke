﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menus
{
    /// <summary>
    /// If click the lock icon in <see cref="LyricEditor"/>, will apply <see cref="LockState.Partial"/> or <see cref="LockState.Full"/>
    /// </summary>
    public class LockStateMenu : EnumMenu<LockState>
    {
        public LockStateMenu(KaraokeRulesetLyricEditorConfigManager config, string text)
            : base(config.GetBindable<LockState>(KaraokeRulesetLyricEditorSetting.ClickToLockLyricState), text)
        {
        }

        protected override LockState[] ValidEnums => new[] { LockState.Partial, LockState.Full };

        protected override string GetName(LockState selection) =>
            selection switch
            {
                LockState.Partial => "Partial",
                LockState.Full => "Full",
                _ => throw new ArgumentOutOfRangeException(nameof(selection))
            };
    }
}
