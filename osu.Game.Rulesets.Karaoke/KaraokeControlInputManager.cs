// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;
using osu.Framework.Input.Bindings;
using osu.Game.Input.Bindings;

namespace osu.Game.Rulesets.Karaoke
{
    public class KaraokeControlInputManager : DatabasedKeyBindingContainer<KaraokeAction>
    {
        public KaraokeControlInputManager(RulesetInfo ruleset)
            : base(ruleset, 1, SimultaneousBindingMode.Unique)
        {
        }
    }

    public enum KaraokeAction
    {
        [Description("First Lyric")]
        FirstLyric,

        [Description("Previous BaseLyric")]
        PreviousLyric,

        [Description("Next BaseLyric")]
        NextLyric,

        [Description("Play and pause")]
        PlayAndPause,

        [Description("Open/Close adjustment")]
        OpenPanel,

        [Description("Increase Speed")]
        IncreaseTempo,

        [Description("Decrease Speed")]
        DecreaseTempo,

        [Description("Reset Speed")]
        ResetTempo,

        [Description("Increase pitch")]
        IncreasePitch,

        [Description("Decrease pitch")]
        DecreasePitch,

        [Description("Reset pitch")]
        ResetPitch,

        [Description("Increase vocal pitch")]
        IncreaseVocalPitch,

        [Description("Decrease vocal pitch")]
        DecreaseVocalPitch,

        [Description("Reset vocal pitch")]
        ResetVocalPitch,

        [Description("Increase scoring pitch")]
        IncreaseScoringPitch,

        [Description("Decrease scoring pitch")]
        DecreaseScoringPitch,

        [Description("Reset scoring pitch")]
        ResetScoringPitch,
    }
}
