// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

public class SingerConvertor : GenericTypeConvertor<ISinger>
{
    private const string singer_id_field = "id";
    private const string main_singer_id_field = "main_singer_id";

    protected override void PostProcessValue(ISinger existingValue, JObject jObject, JsonSerializer serializer)
    {
        int singerId = GetValueFromProperty<int>(jObject, singer_id_field);
        assignIdToSinger(existingValue, nameof(ISinger.ID), singerId);

        if (existingValue is not SubSinger subSinger)
            return;

        int mainSingerId = GetValueFromProperty<int>(jObject, main_singer_id_field);
        assignIdToSinger(subSinger, nameof(SubSinger.MainSingerId), mainSingerId);
    }

    private static void assignIdToSinger(ISinger singer, string propertyName, int value)
    {
        var propertyInfo = singer.GetType().GetProperty(propertyName);
        if (propertyInfo == null)
            return;

        propertyInfo.SetValue(singer, value);
    }

    protected override void PostProcessJObject(JObject jObject, ISinger value, JsonSerializer serializer)
    {
        jObject.Add(singer_id_field, value.ID);

        if (value is SubSinger subSinger)
            jObject.Add(main_singer_id_field, subSinger.MainSingerId);
    }

    protected override Type GetTypeByName(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetType($"osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.{name}");
    }
}
