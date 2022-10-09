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
            globalSetting.Converters.Add(new KaraokeSkinElementConvertor());
            globalSetting.Converters.Add(new ShaderConvertor());
            globalSetting.Converters.Add(new Vector2Converter());
            globalSetting.Converters.Add(new ColourConvertor());
            globalSetting.Converters.Add(new FontUsageConvertor());
            return globalSetting;
        }

        public static JsonSerializerSettings CreateSkinGroupGlobalSettings()
        {
            var globalSetting = JsonSerializableExtensions.CreateGlobalSettings();
            globalSetting.ContractResolver = new SnakeCaseKeyContractResolver();
            globalSetting.Converters.Add(new KaraokeSkinGroupConvertor());
            return globalSetting;
        }

        public static JsonSerializerSettings CreateSkinMappingGlobalSettings()
        {
            var globalSetting = JsonSerializableExtensions.CreateGlobalSettings();
            globalSetting.ContractResolver = new SnakeCaseKeyContractResolver();
            globalSetting.Converters.Add(new KaraokeSkinMappingRoleConvertor());
            return globalSetting;
        }
    }
}
