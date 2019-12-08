// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Input.Bindings;
using osu.Framework.Input.StateChanges.Events;
using osu.Game.Input.Handlers;
using osu.Game.Rulesets.UI;
using System.ComponentModel;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke
{
    public class KaraokeInputManager : RulesetInputManager<KaraokeSoundAction>
    {
        public KaraokeInputManager(RulesetInfo ruleset)
            : base(ruleset, 1, SimultaneousBindingMode.All)
        {
        }

        public override void HandleInputStateChange(InputStateChangeEvent inputStateChange)
        {
            if (inputStateChange is ReplayInputHandler.ReplayStateChangeEvent<KaraokeSoundAction> replayStateChanged
                && replayStateChanged.Input is ReplayInputHandler.ReplayState<KaraokeSoundAction> replayState)
            {
                // Release event should be trigger first
                if (replayStateChanged.ReleasedActions.Any() && !replayState.PressedActions.Any())
                {
                    foreach (var action in replayStateChanged.ReleasedActions)
                        KeyBindingContainer.TriggerReleased(action);
                }

                // If any key pressed, the continuous send press event
                if (replayState.PressedActions.Any())
                {
                    foreach (var action in replayState.PressedActions)
                        KeyBindingContainer.TriggerPressed(action);
                }
            }
            else
            {
                base.HandleInputStateChange(inputStateChange);
            }
        }
    }

    public struct KaraokeSoundAction
    {
        public float Scale { get; set; }
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
