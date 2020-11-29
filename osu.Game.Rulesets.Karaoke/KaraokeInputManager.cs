// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Input;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Handlers.Microphone;
using osu.Framework.Input.StateChanges.Events;
using osu.Framework.Input.States;
using osu.Framework.Logging;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke
{
    public class KaraokeInputManager : RulesetInputManager<KaraokeSaitenAction>
    {
        public KaraokeInputManager(RulesetInfo ruleset)
            : base(ruleset, 1, SimultaneousBindingMode.All)
        {
            UseParentInput = false;
        }

        private IBeatmap beatmap;

        [BackgroundDependencyLoader(true)]
        private void load(KaraokeRulesetConfigManager config, IBindable<IReadOnlyList<Mod>> mods, IBindable<WorkingBeatmap> beatmap, KaraokeSessionStatics session, EditorBeatmap editorBeatmap)
        {
            if (editorBeatmap != null)
            {
                session.Set(KaraokeRulesetSession.SaitenStatus, SaitenStatusMode.Edit);
                return;
            }

            this.beatmap = beatmap.Value.Beatmap;

            var disableMicrophoneDeviceByMod = mods.Value.OfType<IApplicableToMicrophone>().Any(x => !x.MicrophoneEnabled);

            if (disableMicrophoneDeviceByMod)
            {
                session.Set(KaraokeRulesetSession.SaitenStatus, SaitenStatusMode.AutoPlay);
                return;
            }

            var beatmapSaitenable = beatmap.Value.Beatmap.IsScorable();

            if (!beatmapSaitenable)
            {
                session.Set(KaraokeRulesetSession.SaitenStatus, SaitenStatusMode.NotSaitening);
                return;
            }

            try
            {
                var selectedDevice = config.GetBindable<string>(KaraokeRulesetSetting.MicrophoneDevice).Value;
                var microphoneList = new MicrophoneManager().MicrophoneDeviceNames.ToList();

                // Find index by selection id
                var deviceIndex = microphoneList.IndexOf(selectedDevice);
                AddHandler(new OsuTKMicrophoneHandler(deviceIndex));

                session.Set(KaraokeRulesetSession.SaitenStatus, SaitenStatusMode.Saitening);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Microphone initialize error.");
                // todo : set real error by exception
                session.Set(KaraokeRulesetSession.SaitenStatus, SaitenStatusMode.WindowsMicrophonePermissionDeclined);
            }
        }

        protected override InputState CreateInitialState()
            => new KaraokeRulesetInputManagerInputState<KaraokeSaitenAction>(base.CreateInitialState());

        public override void HandleInputStateChange(InputStateChangeEvent inputStateChange)
        {
            if (inputStateChange is ReplayInputHandler.ReplayStateChangeEvent<KaraokeSaitenAction> replayStateChanged
                && replayStateChanged.Input is ReplayInputHandler.ReplayState<KaraokeSaitenAction> replayState)
            {
                // Deal with replay event
                // Release event should be trigger first
                if (replayStateChanged.ReleasedActions.Any() && !replayState.PressedActions.Any())
                {
                    foreach (var action in replayStateChanged.ReleasedActions)
                        KeyBindingContainer.TriggerReleased(action);
                }

                // If any key pressed, then continuously send press event
                if (replayState.PressedActions.Any())
                {
                    foreach (var action in replayState.PressedActions)
                        KeyBindingContainer.TriggerPressed(action);
                }
            }
            else if (inputStateChange is MicrophoneSoundChangeEvent microphoneSoundChange)
            {
                // Deal with realtime microphone event
                var inputState = microphoneSoundChange.State as IMicrophoneInputState;
                var lastState = microphoneSoundChange.LastState;
                var state = inputState?.Microphone;

                if (state == null)
                    throw new ArgumentNullException($"{nameof(state)} cannot be null.");

                // Convert beatmap's pitch to scale setting.
                var scale = beatmap.PitchToScale(state.HasSound ? state.Pitch : lastState.Pitch);

                // TODO : adjust scale
                scale += 5;

                var action = new KaraokeSaitenAction
                {
                    Scale = scale
                };

                if (lastState.HasSound && !state.HasSound)
                    KeyBindingContainer.TriggerReleased(action);
                else
                    KeyBindingContainer.TriggerPressed(action);
            }
            else
            {
                // should never reach this
                base.HandleInputStateChange(inputStateChange);
            }
        }
    }

    public class KaraokeRulesetInputManagerInputState<T> : RulesetInputManagerInputState<T>, IMicrophoneInputState
        where T : struct
    {
        public MicrophoneState Microphone { get; }

        public KaraokeRulesetInputManagerInputState(InputState state)
            : base(state)
        {
            Microphone = new MicrophoneState();
        }
    }

    public struct KaraokeSaitenAction
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

        [Description("Increase saiten pitch")]
        IncreaseSaitenPitch,

        [Description("Decrease saiten pitch")]
        DecreaseSaitenPitch,

        [Description("Reset saiten pitch")]
        ResetSaitenPitch,
    }
}
