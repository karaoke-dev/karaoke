// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Game.Rulesets.Karaoke.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

public class ElementIdConverter : JsonConverter<ElementId?>
{
    public override ElementId? ReadJson(JsonReader reader, Type objectType, ElementId? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var obj = JToken.Load(reader);
        string? value = obj.Value<string?>();

        return createElementId(value);
    }

    private static ElementId? createElementId(string? str) =>
        str switch
        {
            null => null,
            "" => ElementId.Empty,
            _ => new ElementId(str)
        };

    public override void WriteJson(JsonWriter writer, ElementId? value, JsonSerializer serializer)
    {
        string? id = value.ToString();
        writer.WriteValue(id);
    }
}
