// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.UI;
using osu.Framework.Input.Bindings;
using System;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class KaraokePlayfield : Playfield, IKeyBindingHandler<KaraokeAction>
    {
        [Resolved]
        private IBindable<WorkingBeatmap> beatmap { get; set; }

        public WorkingBeatmap WorkingBeatmap => beatmap.Value;

        public Action<KaraokeAction> Pressed;

        public bool OnPressed(KaraokeAction action)
        {
            Pressed?.Invoke(action);
            return false;
        }

        public bool OnReleased(KaraokeAction action)
        {
            return false;
        }
    }
}
