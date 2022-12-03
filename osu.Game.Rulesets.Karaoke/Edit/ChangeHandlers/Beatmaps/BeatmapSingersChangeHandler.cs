// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps
{
    public class BeatmapSingersChangeHandler : BeatmapPropertyChangeHandler, IBeatmapSingersChangeHandler
    {
        [Resolved]
        private BeatmapManager? beatmapManager { get; set; }

        [Resolved]
        private IBindable<WorkingBeatmap>? working { get; set; }

        private SingerInfo singerInfo => KaraokeBeatmap.SingerInfo;

        public BindableList<ISinger> Singers => singerInfo.Singers;

        public void ChangeOrder(ISinger singer, int newIndex)
        {
            performSingerChanged(singer, s =>
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

            performSingerChanged(singer, s =>
            {
                // Write-back the file name.
                s.AvatarFile = newFileName;
            });

            return true;
        }

        public Singer Add()
        {
            var newSinger = singerInfo.AddSinger(s =>
            {
                s.Order = getMaxSingerOrder() + 1;
                s.Name = "New singer";
            });
            return newSinger;

            int getMaxSingerOrder()
                => OrderUtils.GetMaxOrderNumber(singerInfo.GetAllSingers());
        }

        public void Remove(Singer singer)
        {
            singerInfo.RemoveSinger(singer);

            // Should re-sort the order
            OrderUtils.ShiftingOrder(singerInfo.GetAllSingers().Where(x => x.Order > singer.Order), -1);

            // should clear removed singer ids in singer editor.
            Lyrics.ForEach(x =>
            {
                x.Singers.Remove(singer.ID);
            });
        }

        private void performSingerInfoChanged(Action<SingerInfo> action)
        {
            PerformBeatmapChanged(beatmap =>
            {
                action(beatmap.SingerInfo);
            });
        }

        private void performSingerChanged<TSinger>(TSinger singer, Action<TSinger> action) where TSinger : ISinger
        {
            performSingerInfoChanged(singerInfo =>
            {
                if (!singerInfo.Singers.Contains(singer))
                    throw new InvalidOperationException("Singer should be in the beatmap");

                action(singer);
            });
        }
    }
}
