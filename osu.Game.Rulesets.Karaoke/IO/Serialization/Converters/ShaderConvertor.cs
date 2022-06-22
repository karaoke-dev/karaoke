// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Graphics.Shaders;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class ShaderConvertor : JsonConverter<ICustomizedShader>
    {
        // because we wants serializer that containers some common convertors except this one, so make a local one.
        private readonly JsonSerializer localSerializer;

        public ShaderConvertor()
        {
            var settings = JsonSerializableExtensions.CreateGlobalSettings();
            settings.ContractResolver = new KaraokeSkinContractResolver();
            settings.Converters.Add(new ColourConvertor());
            localSerializer = JsonSerializer.Create(settings);
        }

        public override ICustomizedShader ReadJson(JsonReader reader, Type objectType, ICustomizedShader existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // if not knows the type, then should get the type from type field and re-deserializer again.
            var jObject = JObject.Load(reader);
            var properties = jObject.Children().OfType<JProperty>().ToArray();

            // should process child shader first if have.
            var childShaders = getShadersFromProperties(properties, serializer);

            if (childShaders != null)
            {
                // remove value in this field, but not remove the property.
                jObject.Remove("step_shaders");
                jObject.Add("step_shaders", new JArray());
            }

            // should create new reader because old reader cannot reset read position.
            var newReader = jObject.CreateReader();
            var type = objectType != typeof(ICustomizedShader) ? objectType : getTypeByProperties(properties);
            var shader = localSerializer.Deserialize(newReader, type) as ICustomizedShader;

            if (shader is StepShader stepShader && childShaders != null)
            {
                stepShader.StepShaders = childShaders;
            }

            return shader;

            static Type getTypeByProperties(IEnumerable<JProperty> properties)
            {
                string typeString = properties.FirstOrDefault(x => x.Name == "$type")?.Value.ToObject<string>();
                return getTypeByName(typeString);
            }

            static ICustomizedShader[] getShadersFromProperties(IEnumerable<JProperty> properties, JsonSerializer serializer)
            {
                // deserialize step shaders if process step shaders.
                var stepShaders = properties.FirstOrDefault(x => x.Name == "step_shaders");

                if (stepShaders == null)
                    return null;

                var shaderReader = stepShaders.Value.CreateReader();
                var shaders = serializer.Deserialize<ICustomizedShader[]>(shaderReader);
                return shaders;
            }
        }

        public override void WriteJson(JsonWriter writer, ICustomizedShader value, JsonSerializer serializer)
        {
            var jObject = JObject.FromObject(value, localSerializer);
            jObject.AddFirst(new JProperty("$type", getNameByType(value.GetType())));

            var childShader = getShadersFromParent(value, serializer);

            if (childShader != null)
            {
                jObject.Remove("step_shaders");
                jObject.Add("step_shaders", childShader);
            }

            jObject.WriteTo(writer);

            static JArray getShadersFromParent(ICustomizedShader shader, JsonSerializer serializer)
            {
                if (shader is not StepShader stepShader)
                    return null;

                return JArray.FromObject(stepShader.StepShaders, serializer);
            }
        }

        private static Type getTypeByName(string name)
        {
            // only get name from font
            var assembly = AssemblyUtils.GetAssemblyByName("osu.Framework.KaraokeFont");
            return assembly?.GetType($"osu.Framework.Graphics.Shaders.{name}");
        }

        private static string getNameByType(MemberInfo type)
            => type.Name;
    }
}
