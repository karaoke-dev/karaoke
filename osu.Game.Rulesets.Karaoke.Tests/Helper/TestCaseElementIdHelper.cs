// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests.Helper;

public static class TestCaseElementIdHelper
{
    /// <summary>
    /// Because it's hard to create characters as id in the test case.
    /// So create a tool to convert the number to ElementId.
    /// </summary>
    /// <example>
    /// 1       -> 0000001<br/>
    /// 2       -> 0000002<br/>
    /// -1      -> fffffff<br/>
    /// -2      -> ffffffe<br/>
    /// </example>
    /// <param name="number"></param>
    /// <returns></returns>
    public static ElementId CreateElementIdByNumber(int number)
    {
        const int length = 7;
        string id = string.Concat(number.ToString("x").PadLeft(length, '0').TakeLast(length));
        return new ElementId(id);
    }

    public static ElementId[] CreateElementIdsByNumbers(IEnumerable<int> ids)
        => ids.Select(CreateElementIdByNumber).ToArray();
}
