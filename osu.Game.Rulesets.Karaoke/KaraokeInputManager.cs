// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
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
    public class KaraokeInputManager : RulesetInputManager<KaraokeScoringAction>
    {
        public KaraokeInputManager(RulesetInfo ruleset)
            : base(ruleset, 0, SimultaneousBindingMode.All)
        {
            UseParentInput = false;
        }

        private IBeatmap beatmap;

        [BackgroundDependencyLoader(true)]
        private void load(KaraokeRulesetConfigManager config, IBindable<IReadOnlyList<Mod>> mods, IBindable<WorkingBeatmap> beatmap, KaraokeSessionStatics session, EditorBeatmap editorBeatmap)
        {
            if (editorBeatmap != null)
            {
                session.SetValue(KaraokeRulesetSession.ScoringStatus, ScoringStatusMode.Edit);
                return;
            }

            this.beatmap = beatmap.Value.Beatmap;

            bool disableMicrophoneDeviceByMod = mods.Value.OfType<IApplicableToMicrophone>().Any(x => !x.MicrophoneEnabled);

            if (disableMicrophoneDeviceByMod)
            {
                session.SetValue(KaraokeRulesetSession.ScoringStatus, ScoringStatusMode.AutoPlay);
                return;
            }

            bool beatmapSaitenable = beatmap.Value.Beatmap.IsScorable();

            if (!beatmapSaitenable)
            {
                session.SetValue(KaraokeRulesetSession.ScoringStatus, ScoringStatusMode.NotScoring);
                return;
            }

            try
            {
                string selectedDevice = config.Get<string>(KaraokeRulesetSetting.MicrophoneDevice);
                var microphoneList = new MicrophoneManager().MicrophoneDeviceNames.ToList();

                // Find index by selection id
                int deviceIndex = microphoneList.IndexOf(selectedDevice);
                AddHandler(new MicrophoneHandler(deviceIndex));

                session.SetValue(KaraokeRulesetSession.ScoringStatus, ScoringStatusMode.Scoring);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Microphone initialize error.");
                // todo : set real error by exception
                session.SetValue(KaraokeRulesetSession.ScoringStatus, ScoringStatusMode.WindowsMicrophonePermissionDeclined);
            }
        }

        protected override InputState CreateInitialState()
            => new KaraokeRulesetInputManagerInputState<KaraokeScoringAction>(base.CreateInitialState());

        public override void HandleInputStateChange(InputStateChangeEvent inputStateChange)
        {
            switch (inputStateChange)
            {
                case ReplayInputHandler.ReplayStateChangeEvent<KaraokeScoringAction> { Input: ReplayInputHandler.ReplayState<KaraokeScoringAction> replayState } replayStateChanged:
                {
                    // Deal with replay event
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

                    break;
                }

                case MicrophoneVoiceChangeEvent microphoneSoundChange:
                {
                    // Deal with realtime microphone event
                    if (microphoneSoundChange.State is not IMicrophoneInputState inputState)
                        throw new NotMicrophoneInputStateException();

                    var lastVoice = microphoneSoundChange.LastVoice;
                    var voice = inputState.Microphone.Voice;

                    // Convert beatmap's pitch to scale setting.
                    float scale = beatmap.PitchToScale(voice.HasVoice ? voice.Pitch : lastVoice.Pitch);

                    // TODO : adjust scale by
                    scale += 5;

                    var action = new KaraokeScoringAction
                    {
                        Scale = scale
                    };

                    if (lastVoice.HasVoice && !voice.HasVoice)
                        KeyBindingContainer.TriggerReleased(action);
                    else
                        KeyBindingContainer.TriggerPressed(action);
                    break;
                }

                default:
                    // Basically should not goes to here
                    base.HandleInputStateChange(inputStateChange);
                    break;
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

    public struct KaraokeScoringAction
    {
        public float Scale { get; set; }
    }
}
