// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas
{
    public class SingerMetadata
    {
        private List<ISinger> singers = new List<ISinger>();

        public IReadOnlyList<Singer> Singers => singers.OfType<Singer>().ToList();

        public IReadOnlyList<SubSinger> GetSubSingers(Singer singer) => singers.OfType<SubSinger>().Where(x=>x.ParentID == singer.ID).ToList();

        public void CreateSinger(Action<Singer> postProcess)
        {
            var id = singers.Count() + 1;
            var singer = new Singer(id);

            postProcess?.Invoke(singer);
            singers.Add(singer);
        }

        public void CreateSubSinger(Singer parent, Action<SubSinger> postProcess)
        {
            if (parent == null)
                throw new NullReferenceException("Singer cannot be null.");

            var id = singers.Count() + 1;
            var subSinger = new SubSinger(id, parent.ID);

            postProcess?.Invoke(subSinger);
            singers.Add(subSinger);
        }

        public int GetLayoutIndex(ISinger[] singers)
            => singers.Sum(x => (int)Math.Pow(2, x.ID - 1));
    }
}
