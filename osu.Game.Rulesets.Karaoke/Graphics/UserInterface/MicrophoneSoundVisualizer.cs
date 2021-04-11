// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterface
{
    public class MicrophoneSoundVisualizer : CompositeDrawable
    {
        protected override bool Handle(UIEvent e)
        {
            switch (e)
            {
                case MicrophoneStartPitchingEvent microphoneStartPitching:
                    return OnMicrophoneStartSinging(microphoneStartPitching);

                case MicrophoneEndPitchingEvent microphoneEndPitching:
                    return OnMicrophoneEndSinging(microphoneEndPitching);

                case MicrophonePitchingEvent microphonePitching:
                    return OnMicrophoneSinging(microphonePitching);

                default:
                    return base.Handle(e);
            }
        }

        protected virtual bool OnMicrophoneStartSinging(MicrophoneStartPitchingEvent e)
        {
            return false;
        }

        protected virtual bool OnMicrophoneEndSinging(MicrophoneEndPitchingEvent e)
        {
            return false;
        }

        protected virtual bool OnMicrophoneSinging(MicrophonePitchingEvent e)
        {
            var loudness = e.CurrentState.Microphone.Loudness;
            var pitch = e.CurrentState.Microphone.Pitch;

            // todo : draw the pitch position and loudness into this composite drawable.

            return false;
        }

        internal class LoudnessVisualizer : CompositeDrawable
        {
            public LoudnessVisualizer()
            {
                // todo : see how osu-logo edge do
            }
        }

        internal class PitchVisualier : CompositeDrawable
        {
            public PitchVisualier()
            {
                // todo : draw that stupid shapes with progressive background color.
            }
        }
    }
}
