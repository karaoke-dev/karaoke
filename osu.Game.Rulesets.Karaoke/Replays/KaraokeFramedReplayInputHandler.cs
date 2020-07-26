// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Input.StateChanges;
using osu.Game.Replays;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.Karaoke.Replays
{
    public class KaraokeFramedReplayInputHandler : FramedReplayInputHandler<KaraokeReplayFrame>
    {
        public KaraokeFramedReplayInputHandler(Replay replay)
            : base(replay)
        {
        }

        protected override bool IsImportant(KaraokeReplayFrame frame) => frame.Sound;

        public override void CollectPendingInputs(List<IInput> inputs)
        {
            inputs.Add(new ReplayState<KaraokeSaitenAction> {
                PressedActions = CurrentFrame?.Sound ?? false
                    ? new List<KaraokeSaitenAction>
                    {
                        new KaraokeSaitenAction
                        {
                            Scale = CurrentFrame.Scale
                        }
                    }
                    : new List<KaraokeSaitenAction>()
            });
        }
    }
}
