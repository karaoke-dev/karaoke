// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers
{
    public interface IBeatmapChangeHandler
    {
        void Import(IBeatmap newBeatmap);

        void SetScorable(bool scorable);
    }
}
