﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using osu.Game.IO.Serialization;

namespace osu.Game.Rulesets.Karaoke.Tests.Asserts
{
    public class ObjectAssert : Assert
    {
        public static void ArePropertyEqual<T>(T expect, T actual) where T : class
        {
            var settings = JsonSerializableExtensions.CreateGlobalSettings();
            settings.ContractResolver = new WritablePropertiesOnlyResolver();

            var expectJsonString = JsonConvert.SerializeObject(expect, settings);
            var actualJsonString = JsonConvert.SerializeObject(actual, settings);

            AreEqual(expectJsonString, actualJsonString);
        }

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
