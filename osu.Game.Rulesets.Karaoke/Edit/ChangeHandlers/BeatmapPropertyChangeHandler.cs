// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;

public partial class BeatmapPropertyChangeHandler : Component
{
    [Resolved, AllowNull]
    private EditorBeatmap beatmap { get; set; }

    protected KaraokeBeatmap KaraokeBeatmap => (KaraokeBeatmap)beatmap.PlayableBeatmap;

    protected IEnumerable<Lyric> Lyrics => KaraokeBeatmap.HitObjects.OfType<Lyric>();

    protected void PerformBeatmapChanged(Action<KaraokeBeatmap> action)
    {
        beatmap.BeginChange();
        action.Invoke(KaraokeBeatmap);
        beatmap.EndChange();
    }
}
