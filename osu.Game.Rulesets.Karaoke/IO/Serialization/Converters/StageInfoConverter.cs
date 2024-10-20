// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Reflection;
using osu.Game.Rulesets.Karaoke.Stages;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Preview;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

public class StageInfoConverter : GenericTypeConverter<StageInfo>
{
    private const string classic_stage = "classic";
    private const string preview_stage = "preview";

    protected override string GetNameByType(MemberInfo type) =>
        type switch
        {
            Type t when t == typeof(ClassicStageInfo) => classic_stage,
            Type t when t == typeof(PreviewStageInfo) => preview_stage,
            _ => throw new InvalidOperationException(),
        };

    protected override Type GetTypeByName(string name) =>
        name switch
        {
            classic_stage => typeof(ClassicStageInfo),
            preview_stage => typeof(PreviewStageInfo),
            _ => throw new InvalidOperationException(),
        };
}
