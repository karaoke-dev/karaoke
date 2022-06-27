// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.Skinning.MappingRoles;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class KaraokeSkinMappingRoleConvertor : JsonConverter<IMappingRole>
    {
        // because we wants serializer that containers some common convertors except this one, so make a local one.
        private readonly JsonSerializer localSerializer;

        public KaraokeSkinMappingRoleConvertor()
        {
            var settings = JsonSerializableExtensions.CreateGlobalSettings();
            settings.ContractResolver = new KaraokeSkinContractResolver();
            localSerializer = JsonSerializer.Create(settings);
        }

        public override IMappingRole? ReadJson(JsonReader reader, Type objectType, IMappingRole? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var properties = jObject.Children().OfType<JProperty>().ToArray();

            var type = objectType != typeof(IMappingRole) ? objectType : getTypeByProperties(properties);
            var newReader = jObject.CreateReader();
            return localSerializer.Deserialize(newReader, type) as IMappingRole;

            static Type getTypeByProperties(IEnumerable<JProperty> properties)
            {
                string? elementType = properties.FirstOrDefault(x => x.Name == "$type")?.Value.ToObject<string>();
                if (elementType == null)
                    throw new ArgumentNullException(nameof(elementType));

                return getTypeByName(elementType);
            }
        }

        public override void WriteJson(JsonWriter writer, IMappingRole? value, JsonSerializer serializer)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var jObject = JObject.FromObject(value, localSerializer);

            // should get type from enum instead of class type because change class name might cause resource not found.
            jObject.AddFirst(new JProperty("$type", getNameByType(value.GetType())));
            jObject.WriteTo(writer);
        }

        private static Type getTypeByName(string name)
        {
            // only get name from font
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetType($"osu.Game.Rulesets.Karaoke.Skinning.MappingRoles.{name}");
        }

        private static string getNameByType(MemberInfo type)
            => type.Name;
    }
}
