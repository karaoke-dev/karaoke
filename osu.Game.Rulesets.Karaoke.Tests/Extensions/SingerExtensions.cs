// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Extensions;

public static class SingerExtensions
{
    public static TSinger ChangeId<TSinger>(this TSinger singer, ElementId id)
        where TSinger : ISinger
    {
        // get id from the singer and override the id.
        var propertyInfo = singer.GetType().GetProperty(nameof(ISinger.ID));
        if (propertyInfo == null)
            throw new InvalidOperationException();

        propertyInfo.SetValue(singer, id);

        return singer;
    }

    public static TSinger ChangeId<TSinger>(this TSinger singer, int id)
        where TSinger : ISinger
    {
        var elementId = TestCaseElementIdHelper.CreateElementIdByNumber(id);
        return singer.ChangeId(elementId);
    }
}
