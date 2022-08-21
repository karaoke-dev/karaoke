// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Graphics.Shaders;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class ShaderConvertor : GenericTypeConvertor<ICustomizedShader>
    {
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
