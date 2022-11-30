// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

public class SingerInfo
{
    public IList<ISinger> Singers = new List<ISinger>();

    public IEnumerable<Singer> GetAllSingers() =>
        Singers.OfType<Singer>().OrderBy(x => x.Order);

    public IEnumerable<SubSinger> GetAllAvailableSubSinger(Singer singer) =>
        Singers.OfType<SubSinger>().Where(x => x.MainSingerId == singer.ID).OrderBy(x => x.Order);

    public IDictionary<Singer, SubSinger[]> GetSingerByIds(int[] singerIds)
    {
        var matchedMainSingers = GetAllSingers().Where(x => singerIds.Contains(x.ID));
        return matchedMainSingers.ToDictionary(k => k, v =>
        {
            var matchedSubSingers = GetAllAvailableSubSinger(v);

            return matchedSubSingers.Where(x => singerIds.Contains(x.ID)).ToArray();
        });
    }

    public IDictionary<Singer, SubSinger[]> GetSingerMap()
    {
        var matchedMainSingers = GetAllSingers();
        return matchedMainSingers.ToDictionary(k => k, v => GetAllAvailableSubSinger(v).ToArray());
    }

    public Singer AddSinger(Action<Singer>? action = null)
    {
        int id = getNewSingerId();
        var singer = new Singer(id);
        action?.Invoke(singer);

        Singers.Add(singer);

        return singer;
    }

    public SubSinger AddSubSinger(Singer singer, Action<SubSinger>? action = null)
    {
        if (!Singers.Contains(singer))
            throw new InvalidOperationException("Main singer must in the singer info.");

        int id = getNewSingerId();
        int mainSingerId = singer.ID;
        var subSinger = new SubSinger(id, mainSingerId);
        action?.Invoke(subSinger);

        Singers.Add(subSinger);

        return subSinger;
    }

    public bool RemoveSinger(ISinger singer)
    {
        switch (singer)
        {
            case Singer mainSinger:
            {
                var subSingers = GetAllAvailableSubSinger(mainSinger);

                foreach (var subSinger in subSingers)
                {
                    RemoveSinger(subSinger);
                }

                return Singers.Remove(singer);
            }

            case SubSinger:
                return Singers.Remove(singer);

            default:
                throw new InvalidCastException();
        }
    }

    private int getNewSingerId()
    {
        if (Singers.Count == 0)
            return 1;

        return Singers.Max(x => x.ID) + 1;
    }
}
