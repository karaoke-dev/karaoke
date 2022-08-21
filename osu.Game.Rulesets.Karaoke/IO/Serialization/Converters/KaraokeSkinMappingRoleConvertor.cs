// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Game.Rulesets.Karaoke.Skinning.MappingRoles;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class KaraokeSkinMappingRoleConvertor : GenericTypeConvertor<IMappingRole>
    {
        public override IMappingRole? ReadJson(JsonReader reader, Type objectType, IMappingRole? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var properties = jObject.Children().OfType<JProperty>().ToArray();

            var type = objectType != typeof(IMappingRole) ? objectType : getTypeByProperties(properties);
            var newReader = jObject.CreateReader();
            var instance = (IMappingRole)Activator.CreateInstance(type);
            serializer.Populate(newReader, instance);
            return instance;

            Type getTypeByProperties(IEnumerable<JProperty> properties)
            {
                string? elementType = properties.FirstOrDefault(x => x.Name == "$type")?.Value.ToObject<string>();
                if (elementType == null)
                    throw new ArgumentNullException(nameof(elementType));

                return GetTypeByName(elementType);
            }
        }

        protected override Type GetTypeByName(string name)
        {
            // only get name from font
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetType($"osu.Game.Rulesets.Karaoke.Skinning.MappingRoles.{name}");
        }
    }
}
