// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public abstract class GenericTypeConvertor<TType> : GenericTypeConvertor<TType, string>
    {
        protected override string GetNameByType(MemberInfo type)
            => type.Name;
    }

    public abstract class GenericTypeConvertor<TType, TTypeName> : JsonConverter<TType> where TTypeName : notnull
    {
        public sealed override TType ReadJson(JsonReader reader, Type objectType, TType? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var jProperties = jObject.Children().OfType<JProperty>().ToArray();
            var type = objectType != typeof(TType) ? objectType : getTypeByProperties(jProperties);

            var newReader = jObject.CreateReader();

            var instance = (TType)Activator.CreateInstance(type);
            InteractWithJObject(jObject, objectType, existingValue, hasExistingValue, serializer);
            serializer.Populate(newReader, instance);
            return instance;

            Type getTypeByProperties(IEnumerable<JProperty> properties)
            {
                var value = properties.FirstOrDefault(x => x.Name == "$type")?.Value;
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                TTypeName? elementType = value.ToObject<TTypeName>();
                if (elementType == null)
                    throw new InvalidCastException(nameof(elementType));

                return GetTypeByName(elementType);
            }
        }

        protected virtual void InteractWithJObject(JObject jObject, Type objectType, TType? existingValue, bool hasExistingValue, JsonSerializer serializer) { }

        public sealed override void WriteJson(JsonWriter writer, TType? value, JsonSerializer serializer)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var resolver = serializer.ContractResolver;

            // follow: https://stackoverflow.com/a/59329703
            // not a good way but seems there's no better choice.
            serializer.Converters.Remove(this);
            serializer.ContractResolver = new KaraokeSkinContractResolver();

            var jObject = JObject.FromObject(value, serializer);

            serializer.Converters.Add(this);
            serializer.ContractResolver = resolver;

            InteractWithJObject(jObject, writer, value, serializer);
            jObject.AddFirst(new JProperty("$type", GetNameByType(value.GetType())));
            jObject.WriteTo(writer);
        }

        protected virtual void InteractWithJObject(JObject jObject, JsonWriter writer, TType value, JsonSerializer serializer) { }

        protected abstract Type GetTypeByName(TTypeName name);

        protected abstract TTypeName GetNameByType(MemberInfo type);
    }
}
