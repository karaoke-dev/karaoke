// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers
{
    public abstract class BeatmapChangeHandler<TItem> : Component
    {
        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        protected KaraokeBeatmap Beatmap => beatmap.PlayableBeatmap as KaraokeBeatmap;

        [Resolved(CanBeNull = true)]
        private IEditorChangeHandler changeHandler { get; set; }

        protected void PerformObjectChanged(TItem item, Action<TItem> action)
        {
            changeHandler?.BeginChange();
            action?.Invoke(item);
            changeHandler?.EndChange();
        }

        public abstract void Add(TItem item);

        public abstract void Remove(TItem item);
    }
}
