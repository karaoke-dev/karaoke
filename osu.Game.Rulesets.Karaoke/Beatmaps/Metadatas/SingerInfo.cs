// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

public class SingerInfo
{
    public bool SupportSingerState { get; set; }

    // todo: should make the property as readonly.
    public BindableList<ISinger> Singers { get; set; } = new();

    public IEnumerable<Singer> GetAllSingers() =>
        Singers.OfType<Singer>().OrderBy(x => x.Order);

    public IEnumerable<SingerState> GetAllAvailableSingerStates(Singer singer) =>
        Singers.OfType<SingerState>().Where(x => x.MainSingerId == singer.ID).OrderBy(x => x.Order);

    public IDictionary<Singer, SingerState[]> GetSingerByIds(ElementId[] singerIds)
    {
        var matchedMainSingers = GetAllSingers().Where(x => singerIds.Contains(x.ID));
        return matchedMainSingers.ToDictionary(k => k, v =>
        {
            var matchedSingerStates = GetAllAvailableSingerStates(v);

            return matchedSingerStates.Where(x => singerIds.Contains(x.ID)).ToArray();
        });
    }

    public IDictionary<Singer, SingerState[]> GetSingerMap()
    {
        var matchedMainSingers = GetAllSingers();
        return matchedMainSingers.ToDictionary(k => k, v => GetAllAvailableSingerStates(v).ToArray());
    }

    public Singer AddSinger(Action<Singer>? action = null)
    {
        var id = getNewSingerId();
        var singer = new Singer(id);
        action?.Invoke(singer);

        Singers.Add(singer);

        return singer;
    }

    public SingerState AddSingerState(Singer singer, Action<SingerState>? action = null)
    {
        if (!Singers.Contains(singer))
            throw new InvalidOperationException("Main singer must in the singer info.");

        var id = getNewSingerId();
        var mainSingerId = singer.ID;
        var singerState = new SingerState(id, mainSingerId);
        action?.Invoke(singerState);

        Singers.Add(singerState);

        return singerState;
    }

    public bool RemoveSinger(ISinger singer)
    {
        switch (singer)
        {
            case Singer mainSinger:
            {
                var singerStates = GetAllAvailableSingerStates(mainSinger);

                foreach (var singerState in singerStates)
                {
                    RemoveSinger(singerState);
                }

                return Singers.Remove(singer);
            }

            case SingerState:
                return Singers.Remove(singer);

            default:
                throw new InvalidCastException();
        }
    }

    private ElementId getNewSingerId()
    {
        return ElementId.NewElementId();
    }
}
