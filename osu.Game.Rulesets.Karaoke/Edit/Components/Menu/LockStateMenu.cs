﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menu
{
    /// <summary>
    /// If click the lock icon in <see cref="LyricEditor"/>, will apply <see cref="LockState.Partial"/> or <see cref="LockState.Full"/>
    /// </summary>
    public class LockStateMenu : EnumMenu<LockState>
    {
        protected override KaraokeRulesetEditSetting Setting => KaraokeRulesetEditSetting.ClickToLockLyricState;

        public LockStateMenu(KaraokeRulesetEditConfigManager config, string text)
           : base(config, text)
        {
        }

        protected override LockState[] ValidEnums => new[] { LockState.Partial, LockState.Full};

        protected override string GetName(LockState selection)
        {
            switch (selection)
            {
                case LockState.Partial:
                    return "Partial";

                case LockState.Full:
                    return "Full";

                default:
                    throw new ArgumentOutOfRangeException(nameof(selection));
            }
        }
    }
}
