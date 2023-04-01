// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Caching;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;

public partial class BeatmapPropertyChangeHandler : Component
{
    private readonly Cached changingCache = new();

    [Resolved, AllowNull]
    private EditorBeatmap beatmap { get; set; }

    protected KaraokeBeatmap KaraokeBeatmap => EditorBeatmapUtils.GetPlayableBeatmap(beatmap);

    protected IEnumerable<Lyric> Lyrics => KaraokeBeatmap.HitObjects.OfType<Lyric>();

    protected BeatmapPropertyChangeHandler()
    {
        changingCache.Validate();
    }

    protected void PerformBeatmapChanged(Action<KaraokeBeatmap> action)
    {
        try
        {
            beatmap.BeginChange();
            action.Invoke(KaraokeBeatmap);
            beatmap.EndChange();
        }
        catch
        {
            // We should make sure that editor beatmap will end the change if still changing.
            // will goes to here if have exception in the change handler.
            if (beatmap.TransactionActive)
                beatmap.EndChange();

            throw;
        }
    }

    protected void PerformOnSelection<T>(Action<T> action) where T : HitObject
    {
        if (!changingCache.IsValid)
            throw new NotSupportedException("Cannot trigger the change while applying another change.");

        if (beatmap.SelectedHitObjects.Count == 0)
            throw new NotSupportedException($"Should contain at least one selected {nameof(T)}");

        changingCache.Invalidate();

        try
        {
            // should trigger the UpdateState() in the editor beatmap only if there's no active state.
            beatmap.PerformOnSelection(h =>
            {
                if (h is T tHitObject)
                    action(tHitObject);
            });
        }
        catch
        {
            // We should make sure that editor beatmap will end the change if still changing.
            // will goes to here if have exception in the change handler.
            if (beatmap.TransactionActive)
                beatmap.EndChange();

            throw;
        }
        finally
        {
            changingCache.Validate();
        }
    }

    // todo: before having better solution to handle the undo/redo with better performance, we should use this to method to force invalidate all hit-object's working property.
    protected void InvalidateAllHitObjectWorkingProperty<TWorkingProperty>(TWorkingProperty property)
        where TWorkingProperty : struct, Enum
    {
        foreach (var hitObject in KaraokeBeatmap.HitObjects.OfType<IHasWorkingProperty<TWorkingProperty>>())
        {
            hitObject.InvalidateWorkingProperty(property);
        }

        beatmap.UpdateAllHitObjects();
    }
}
