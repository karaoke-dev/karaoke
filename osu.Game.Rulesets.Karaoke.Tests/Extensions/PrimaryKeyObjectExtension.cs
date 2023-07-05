// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Extensions;

public static class PrimaryKeyObjectExtension
{
    public static TObject ChangeId<TObject>(this TObject obj, int id)
        where TObject : IHasPrimaryKey
    {
        var elementId = TestCaseElementIdHelper.CreateElementIdByNumber(id);
        return obj.ChangeId(elementId);
    }
}
