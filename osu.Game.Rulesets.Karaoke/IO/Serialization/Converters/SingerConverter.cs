// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

public class SingerConverter : GenericTypeConverter<ISinger>
{
    private const string singer_id_field = "id";
    private const string main_singer_id_field = "main_singer_id";

    protected override void PostProcessValue(ISinger existingValue, JObject jObject, JsonSerializer serializer)
    {
        var singerId = GetValueFromProperty<ElementId>(serializer, jObject, singer_id_field);
        assignIdToSinger(existingValue, nameof(ISinger.ID), singerId);

        if (existingValue is not SingerState singerState)
            return;

        var mainSingerId = GetValueFromProperty<ElementId>(serializer, jObject, main_singer_id_field);
        assignIdToSinger(singerState, nameof(SingerState.MainSingerId), mainSingerId);
    }

    private static void assignIdToSinger(ISinger singer, string propertyName, ElementId value)
    {
        var propertyInfo = singer.GetType().GetProperty(propertyName);
        if (propertyInfo == null)
            return;

        propertyInfo.SetValue(singer, value);
    }

    protected override void PostProcessJObject(JObject jObject, ISinger value, JsonSerializer serializer)
    {
        jObject.Add(singer_id_field, JToken.FromObject(value.ID, serializer));

        if (value is SingerState singerState)
            jObject.Add(main_singer_id_field, JToken.FromObject(singerState.MainSingerId, serializer));
    }

    protected override Type GetTypeByName(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var type = assembly.GetType($"osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.{name}");
        Debug.Assert(type != null);
        return type;
    }
}
