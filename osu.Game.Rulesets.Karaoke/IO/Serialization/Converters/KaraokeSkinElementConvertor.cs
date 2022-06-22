// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.IO.Serialization;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class KaraokeSkinElementConvertor : JsonConverter<IKaraokeSkinElement>
    {
        // because we wants serializer that containers some common convertors except this one, so make a local one.
        private readonly JsonSerializer localSerializer;

        public KaraokeSkinElementConvertor()
        {
            var settings = JsonSerializableExtensions.CreateGlobalSettings();
            settings.ContractResolver = new KaraokeSkinContractResolver();
            settings.Converters.Add(new ColourConvertor());
            settings.Converters.Add(new Vector2Converter());
            settings.Converters.Add(new ShaderConvertor());
            settings.Converters.Add(new FontUsageConvertor());
            localSerializer = JsonSerializer.Create(settings);
        }

        public override IKaraokeSkinElement ReadJson(JsonReader reader, Type objectType, IKaraokeSkinElement existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var properties = jObject.Children().OfType<JProperty>().ToArray();

            var type = objectType != typeof(IKaraokeSkinElement) ? objectType : getTypeByProperties(properties);
            var newReader = jObject.CreateReader();
            return localSerializer.Deserialize(newReader, type) as IKaraokeSkinElement;

            static Type getTypeByProperties(IEnumerable<JProperty> properties)
            {
                var elementType = properties.FirstOrDefault(x => x.Name == "$type")?.Value.ToObject<ElementType>();
                if (elementType == null)
                    throw new ArgumentNullException(nameof(elementType));

                return GetObjectType(elementType.Value);
            }
        }

        public override void WriteJson(JsonWriter writer, IKaraokeSkinElement value, JsonSerializer serializer)
        {
            var jObject = JObject.FromObject(value, localSerializer);

            // should get type from enum instead of class type because change class name might cause resource not found.
            jObject.AddFirst(new JProperty("$type", GetElementType(value.GetType())));
            jObject.WriteTo(writer);
        }

        public static ElementType GetElementType(Type elementType) =>
            elementType switch
            {
                var type when type == typeof(LyricConfig) => ElementType.LyricConfig,
                var type when type == typeof(LyricLayout) => ElementType.LyricLayout,
                var type when type == typeof(LyricStyle) => ElementType.LyricStyle,
                var type when type == typeof(NoteStyle) => ElementType.NoteStyle,
                _ => throw new NotSupportedException()
            };

        public static Type GetObjectType(ElementType elementType) =>
            elementType switch
            {
                ElementType.LyricConfig => typeof(LyricConfig),
                ElementType.LyricLayout => typeof(LyricLayout),
                ElementType.LyricStyle => typeof(LyricStyle),
                ElementType.NoteStyle => typeof(NoteStyle),
                _ => throw new NotSupportedException()
            };
    }
}
