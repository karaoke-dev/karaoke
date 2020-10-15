// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas
{
    public class SingerMetadata
    {
        private readonly List<ISinger> singers = new List<ISinger>();

        public IReadOnlyList<Singer> Singers => singers.OfType<Singer>().ToList();

        public IReadOnlyList<SubSinger> GetSubSingers(Singer singer) => singers.OfType<SubSinger>().Where(x => x.ParentID == singer.ID).ToList();

        public void CreateSinger(Action<Singer> postProcess)
        {
            var id = singers.Count + 1;
            var singer = new Singer(id);

            postProcess?.Invoke(singer);
            singers.Add(singer);
        }

        public void RemoveSinger(Singer singer)
        {
            if (singer == null)
                throw new NullReferenceException("Singer cannot be null.");

            if (!Singers.Contains(singer))
                throw new ArgumentOutOfRangeException("Singer is not in the list.");

            // Remove sub singers.
            var subSingers = GetSubSingers(singer);

            foreach (var subSinger in subSingers)
            {
                RemoveSubSinger(subSinger);
            }

            // remove singer
            singers.Remove(singer);
        }

        public void CreateSubSinger(Singer parent, Action<SubSinger> postProcess)
        {
            if (parent == null)
                throw new NullReferenceException("Singer cannot be null.");

            var id = singers.Count + 1;
            var subSinger = new SubSinger(id, parent.ID);

            postProcess?.Invoke(subSinger);
            singers.Add(subSinger);
        }

        public void RemoveSubSinger(SubSinger subSinger)
        {
            if (subSinger == null)
                throw new NullReferenceException("Sub singer cannot be null.");

            if (!singers.Contains(subSinger))
                throw new ArgumentOutOfRangeException("Sub singer is not in the list");

            singers.Remove(subSinger);
        }

        public int GetLayoutIndex(ISinger[] singers)
            => singers.Sum(x => (int)Math.Pow(2, x.ID - 1));
    }
}
