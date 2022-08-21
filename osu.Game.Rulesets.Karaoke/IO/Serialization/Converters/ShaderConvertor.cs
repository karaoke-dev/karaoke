// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Graphics.Shaders;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class ShaderConvertor : GenericTypeConvertor<ICustomizedShader>
    {
        public override ICustomizedShader? ReadJson(JsonReader reader, Type objectType, ICustomizedShader? existingValue, bool hasExistingValue, JsonSerializer serializer)
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
            var type = objectType != typeof(ICustomizedShader) ? objectType : getTypeByProperties(properties);
            var newReader = jObject.CreateReader();
            var instance = (ICustomizedShader)Activator.CreateInstance(type);
            serializer.Populate(newReader, instance);

            if (instance is StepShader stepShader && childShaders != null)
            {
                stepShader.StepShaders = childShaders;
            }

            return instance;

            Type? getTypeByProperties(IEnumerable<JProperty> properties)
            {
                string? typeString = properties.FirstOrDefault(x => x.Name == "$type")?.Value.ToObject<string>();
                if (string.IsNullOrEmpty(typeString))
                    return default;

                return GetTypeByName(typeString);
            }

            static ICustomizedShader[]? getShadersFromProperties(IEnumerable<JProperty> properties, JsonSerializer serializer)
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

        protected override void InteractWithJObject(JObject jObject, JsonWriter writer, ICustomizedShader value, JsonSerializer serializer)
        {
            var childShader = getShadersFromParent(value, serializer);

            if (childShader != null)
            {
                jObject.Remove("step_shaders");
                jObject.Add("step_shaders", childShader);
            }

            static JArray? getShadersFromParent(ICustomizedShader shader, JsonSerializer serializer)
            {
                if (shader is not StepShader stepShader)
                    return null;

                return JArray.FromObject(stepShader.StepShaders, serializer);
            }
        }

        protected override Type GetTypeByName(string name)
        {
            // only get name from font
            var assembly = AssemblyUtils.GetAssemblyByName("osu.Framework.KaraokeFont");
            Debug.Assert(assembly != null);
            return assembly.GetType($"osu.Framework.Graphics.Shaders.{name}");
        }
    }
}
