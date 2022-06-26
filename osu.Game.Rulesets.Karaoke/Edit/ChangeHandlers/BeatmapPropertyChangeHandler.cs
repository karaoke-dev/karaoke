// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers
{
    public abstract class BeatmapPropertyChangeHandler<TItem> : Component
    {
        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        private KaraokeBeatmap karaokeBeatmap => beatmap.PlayableBeatmap as KaraokeBeatmap;

        protected IEnumerable<Lyric> Lyrics => karaokeBeatmap.HitObjects.OfType<Lyric>();

        // todo: should be interface.
        protected BindableList<TItem> Items = new();

        [BackgroundDependencyLoader]
        private void load()
        {
            Items.AddRange(GetItemsFromBeatmap(karaokeBeatmap));

            // todo: find a better way to handle only beatmap property changed.
            beatmap.TransactionEnded += syncItemsFromBeatmap;

            syncItemsFromBeatmap();

            void syncItemsFromBeatmap()
            {
                var items = GetItemsFromBeatmap(karaokeBeatmap);

                if (Items.SequenceEqual(items))
                    return;

                Items.AddRange(items.Except(Items));
                Items.RemoveAll(x => !items.Contains(x));
            }
        }

        protected void PerformObjectChanged(TItem item, Action<TItem> action)
        {
            // should call change from editor beatmap because there's only way to trigger transaction ended.
            beatmap?.BeginChange();
            action?.Invoke(item);
            beatmap?.EndChange();
        }

        protected abstract IList<TItem> GetItemsFromBeatmap(KaraokeBeatmap beatmap);

        public void Add(TItem item)
        {
            var items = GetItemsFromBeatmap(karaokeBeatmap);
            if (items.Contains(item))
                throw new InvalidOperationException(nameof(item));

            PerformObjectChanged(item, i =>
            {
                items.Add(i);
                OnItemAdded(i);
            });
        }

        public void Remove(TItem item)
        {
            var items = GetItemsFromBeatmap(karaokeBeatmap);
            if (!items.Contains(item))
                throw new InvalidOperationException($"{nameof(item)} is not in the list");

            PerformObjectChanged(item, i =>
            {
                items.Remove(i);
                OnItemRemoved(i);
            });
        }

        protected abstract void OnItemAdded(TItem item);

        protected abstract void OnItemRemoved(TItem item);
    }
}
