﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Components.Menus;

/// <summary>
/// If click the lock icon in <see cref="LyricEditor"/>, will apply <see cref="LockState.Partial"/> or <see cref="LockState.Full"/>
/// </summary>
public class LockStateMenuItem : BindableEnumMenuItem<LockState>
{
    public LockStateMenuItem(string text, KaraokeRulesetLyricEditorConfigManager config)
        : base(text, config.GetBindable<LockState>(KaraokeRulesetLyricEditorSetting.ClickToLockLyricState))
    {
    }

    protected override IEnumerable<LockState> ValidEnums => new[] { LockState.Partial, LockState.Full };
}
