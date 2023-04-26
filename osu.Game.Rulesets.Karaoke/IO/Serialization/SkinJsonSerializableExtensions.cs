// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Framework.IO.Serialization;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization
{
    public static class SkinJsonSerializableExtensions
    {
        public static JsonSerializerSettings CreateSkinElementGlobalSettings()
        {
            var globalSetting = JsonSerializableExtensions.CreateGlobalSettings();
            globalSetting.ContractResolver = new SnakeCaseKeyContractResolver();
            globalSetting.Converters.Add(new KaraokeSkinElementConverter());
            globalSetting.Converters.Add(new ShaderConverter());
            globalSetting.Converters.Add(new Vector2Converter());
            globalSetting.Converters.Add(new ColourConverter());
            globalSetting.Converters.Add(new FontUsageConverter());
            return globalSetting;
        }

        public static JsonSerializerSettings CreateSkinGroupGlobalSettings()
        {
            var globalSetting = JsonSerializableExtensions.CreateGlobalSettings();
            globalSetting.ContractResolver = new SnakeCaseKeyContractResolver();
            globalSetting.Converters.Add(new KaraokeSkinGroupConverter());
            return globalSetting;
        }
    }
}
