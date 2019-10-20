// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.UI;
using System.ComponentModel;

namespace osu.Game.Rulesets.Karaoke
{
    public class KaraokeInputManager : RulesetInputManager<KaraokeAction>
    {
        public KaraokeInputManager(RulesetInfo ruleset)
            : base(ruleset, 0, SimultaneousBindingMode.Unique)
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

        [Description("Open/Close panel")]
        OpenPanel,

        [Description("Increase Speed")]
        IncreaseTempo,

        [Description("Decrease Speed")]
        DecreaseTempo,

        [Description("Reset Speed")]
        ResetTempo,

        [Description("Increase Tone")]
        IncreasePitch,

        [Description("Decrease Tone")]
        DecreasePitch,

        [Description("Reset Tone")]
        ResetPitch,

        [Description("Increase appear time")]
        IncreaseLyricAppearTime,

        [Description("Decrease appear time")]
        DecreaseLyricAppearTime,

        [Description("Reset appear time")]
        ResetLyricAppearTime,
    }
}
