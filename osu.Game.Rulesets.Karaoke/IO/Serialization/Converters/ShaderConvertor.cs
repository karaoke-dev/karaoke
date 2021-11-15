// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using osu.Framework.Graphics.Shaders;
using osu.Game.IO.Serialization;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class ShaderConvertor : JsonConverter<IShader>
    {
        // because we wants serializer that containers some common convertors except this one, so make a local one.
        private readonly JsonSerializer localSerializer;

        public ShaderConvertor()
        {
            var settings = JsonSerializableExtensions.CreateGlobalSettings();
            settings.ContractResolver = new WritablePropertiesOnlyResolver();
            settings.Converters = settings.Converters.Concat(new JsonConverter[]
            {
                new ColourConvertor(),
            }).ToArray();
            localSerializer = JsonSerializer.Create(settings);
        }

        public override IShader ReadJson(JsonReader reader, Type objectType, IShader existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // if not knows the type, then should get the type from type field and re-deserializer again.
            var jObject = JObject.Load(reader);
            var properties = jObject.Children().OfType<JProperty>().ToArray();

            // should process child shader first if have.
            var childShaders = getShadersFromProperties(properties, serializer);

            if (childShaders != null)
            {
                // remove value in this field, but not remove the property.
                jObject.Remove("StepShaders");
                jObject.Add("StepShaders", new JArray());
            }

            // should create new reader because old reader cannot reset read position.
            var newReader = jObject.CreateReader();
            var type = objectType != typeof(IShader) ? objectType : getTypeByProperties(properties);
            var shader = localSerializer.Deserialize(newReader, type) as IShader;

            if (shader is StepShader stepShader && childShaders != null)
            {
                stepShader.StepShaders = childShaders;
            }

            return shader;

            static Type getTypeByProperties(JProperty[] properties)
            {
                var typeString = properties.FirstOrDefault(x => x.Name == "$type")?.Value.ToObject<string>();
                return getTypeByName(typeString);
            }

            static IShader[] getShadersFromProperties(JProperty[] properties, JsonSerializer serializer)
            {
                // deserialize step shaders if process step shaders.
                var stepShaders = properties.FirstOrDefault(x => x.Name == "StepShaders");

                if (stepShaders == null)
                    return null;

                var shaderReader = stepShaders.Value.CreateReader();
                var shaders = serializer.Deserialize<IShader[]>(shaderReader);
                return shaders;
            }
        }

        public override void WriteJson(JsonWriter writer, IShader value, JsonSerializer serializer)
        {
            var jObject = JObject.FromObject(value, localSerializer);
            jObject.AddFirst(new JProperty("$type", getNameByType(value.GetType())));

            var childShader = getShadersFromParent(value, serializer);

            if (childShader != null)
            {
                jObject.Remove("StepShaders");
                jObject.Add("StepShaders", childShader);
            }

            jObject.WriteTo(writer);

            static JArray getShadersFromParent(IShader shader, JsonSerializer serializer)
            {
                if (shader is not StepShader stepShader)
                    return null;

                return JArray.FromObject(stepShader.StepShaders, serializer);
            }
        }

        private static Type getTypeByName(string name)
        {
            // only get name from font
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.Contains("osu.Framework.KaraokeFont"));
            return assembly?.GetType($"osu.Framework.Graphics.Shaders.{name}");
        }

        private static string getNameByType(Type type)
            => type.Name;

        private class WritablePropertiesOnlyResolver : DefaultContractResolver
        {
            // we only wants to save properties that only writable.
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);
                return props.Where(p => p.Writable).ToList();
            }
        }
    }
}
