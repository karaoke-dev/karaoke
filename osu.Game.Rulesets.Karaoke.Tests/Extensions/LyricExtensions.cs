// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Extensions;

public static class LyricExtensions
{
    public static Lyric ChangeId(this Lyric lyric, ElementId id)
    {
        // get id from the singer and override the id.
        var propertyInfo = lyric.GetType().GetProperty(nameof(Lyric.ID));
        if (propertyInfo == null)
            throw new InvalidOperationException();

        propertyInfo.SetValue(lyric, id);

        return lyric;
    }

    public static Lyric ChangeId(this Lyric lyric, int id)
    {
        var elementId = TestCaseElementIdHelper.CreateElementIdByNumber(id);
        return lyric.ChangeId(elementId);
    }
}
