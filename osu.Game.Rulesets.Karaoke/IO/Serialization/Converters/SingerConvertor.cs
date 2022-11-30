// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

public class SingerConvertor : GenericTypeConvertor<ISinger>
{
    protected override void PostProcessValue(ISinger existingValue, JObject jObject, JsonSerializer serializer)
    {
        int value = GetValueFromProperty<int>(jObject, "id");

        var propertyInfo = existingValue.GetType().GetProperty(nameof(ISinger.ID));
        if (propertyInfo == null)
            return;

        propertyInfo.SetValue(existingValue, value);
    }

    protected override void PostProcessJObject(JObject jObject, ISinger value, JsonSerializer serializer)
    {
        jObject.Add("id", value.ID);
    }

    protected override Type GetTypeByName(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetType($"osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.{name}");
    }
}
