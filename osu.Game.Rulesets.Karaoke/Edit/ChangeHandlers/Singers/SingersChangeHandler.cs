// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Singers
{
    public class SingersChangeHandler : BeatmapPropertyChangeHandler<Singer>, ISingersChangeHandler
    {
        [Resolved(canBeNull: true)]
        private BeatmapManager beatmapManager { get; set; }

        [Resolved(canBeNull: true)]
        private IBindable<WorkingBeatmap> working { get; set; }

        public BindableList<Singer> Singers => Items;

        protected override List<Singer> GetItemsFromBeatmap(KaraokeBeatmap beatmap)
            => beatmap.Singers;

        public void ChangeOrder(Singer singer, int newIndex)
        {
            PerformObjectChanged(singer, s =>
            {
                int oldOrder = s.Order;
                int newOrder = newIndex + 1; // order is start from 1
                OrderUtils.ChangeOrder(Singers.ToArray(), oldOrder, newOrder, (switchSinger, oldOrder, newOrder) =>
                {
                    // todo : not really sure should call update?
                });
            });
        }

        public bool ChangeSingerAvatar(Singer singer, FileInfo fileInfo)
        {
            if (beatmapManager == null || working == null)
                return false;

            if (!fileInfo.Exists)
                throw new FileNotFoundException();

            // note: follow the same logic in the ResourcesSection.ChangeBackgroundImage
            var set = working.Value.BeatmapSetInfo;

            // todo: we might re-format the new file name, like give it a hash name for prevent duplicated file name with other singer.
            string newFileName = fileInfo.Name;

            using (var stream = fileInfo.OpenRead())
            {
                // in the future we probably want to check if this is being used elsewhere (other difficulties?)
                var oldFile = set.Files.FirstOrDefault(f => f.Filename == singer.AvatarFile);
                if (oldFile != null)
                    beatmapManager.DeleteFile(set, oldFile);

                beatmapManager.AddFile(set, stream, $"assets/singers/{newFileName}");
            }

            // Write-back the file name.
            singer.AvatarFile = newFileName;

            return true;
        }

        protected override void OnItemAdded(Singer item)
        {
            // should give it a id.
            item.Order = OrderUtils.GetMaxOrderNumber(Singers.ToArray()) + 1;
        }

        protected override void OnItemRemoved(Singer item)
        {
            // Should re-sort the order
            OrderUtils.ShiftingOrder(Singers.Where(x => x.Order > item.Order), -1);

            // should clear removed singer ids in singer editor.
            Lyrics.ForEach(x =>
            {
                x.Singers.Remove(item.ID);
            });
        }
    }
}
