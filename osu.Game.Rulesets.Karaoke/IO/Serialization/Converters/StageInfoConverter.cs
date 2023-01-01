// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Reflection;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

public class StageInfoConverter : GenericTypeConverter<StageInfo>
{
    private const string classic_stage = "classic";

    protected override string GetNameByType(MemberInfo type) =>
        type switch
        {
            Type t when t == typeof(ClassicStageInfo) => classic_stage,
            _ => throw new InvalidOperationException()
        };

    protected override Type GetTypeByName(string name) =>
        name switch
        {
            classic_stage => typeof(ClassicStageInfo),
            _ => throw new InvalidOperationException()
        };
}
