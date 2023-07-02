// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

public abstract class GenericTypeConverter<TType> : GenericTypeConverter<TType, string>
{
    protected override string GetNameByType(MemberInfo type)
        => type.Name;
}

public abstract class GenericTypeConverter<TType, TTypeName> : JsonConverter<TType> where TTypeName : notnull
{
    public sealed override TType ReadJson(JsonReader reader, Type objectType, TType? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);
        var type = objectType != typeof(TType) ? objectType : getTypeByProperties(jObject);

        var newReader = jObject.CreateReader();

        var instance = (TType)Activator.CreateInstance(type)!;
        serializer.Populate(newReader, instance);
        PostProcessValue(instance, jObject, serializer);
        return instance;

        Type getTypeByProperties(JObject jObj)
        {
            var elementType = GetValueFromProperty<TTypeName>(serializer, jObj, "$type");
            return GetTypeByName(elementType);
        }
    }

    protected static TPropertyType GetValueFromProperty<TPropertyType>(JsonSerializer serializer, JObject jObject, string propertyName)
    {
        var jProperties = jObject.Children().OfType<JProperty>().ToArray();
        var value = jProperties.FirstOrDefault(x => x.Name == propertyName)?.Value;
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        var elementType = value.ToObject<TPropertyType>(serializer);
        if (elementType == null)
            throw new InvalidCastException(nameof(elementType));

        return elementType;
    }

    protected virtual void PostProcessValue(TType existingValue, JObject jObject, JsonSerializer serializer) { }

    public sealed override void WriteJson(JsonWriter writer, TType? value, JsonSerializer serializer)
    {
        ArgumentNullException.ThrowIfNull(value);

        var resolver = serializer.ContractResolver;

        // follow: https://stackoverflow.com/a/59329703
        // not a good way but seems there's no better choice.
        serializer.Converters.Remove(this);
        serializer.ContractResolver = new WritablePropertiesOnlyResolver();

        var jObject = JObject.FromObject(value, serializer);

        serializer.Converters.Add(this);
        serializer.ContractResolver = resolver;

        jObject.AddFirst(new JProperty("$type", GetNameByType(value.GetType())));
        PostProcessJObject(jObject, value, serializer);
        jObject.WriteTo(writer);
    }

    protected virtual void PostProcessJObject(JObject jObject, TType value, JsonSerializer serializer) { }

    protected abstract Type GetTypeByName(TTypeName name);

    protected abstract TTypeName GetNameByType(MemberInfo type);
}
