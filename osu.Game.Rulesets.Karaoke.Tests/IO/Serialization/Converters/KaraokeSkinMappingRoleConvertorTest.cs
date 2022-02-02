// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Rulesets.Karaoke.Skinning.MappingRoles;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters
{
    public class KaraokeSkinMappingRoleConvertorTest : BaseSingleConverterTest<KaraokeSkinMappingRoleConvertor>
    {
        [Test]
        public void TestDefaultMappingRoleSerializer()
        {
            var group = new DefaultMappingRole
            {
                Name = "Singer 1 and 2",
                ElementType = ElementType.LyricLayout,
                ElementId = 1,
                GroupId = 2,
            };
            string result = JsonConvert.SerializeObject(group, CreateSettings());
            Assert.AreEqual(result, "{\"$type\":\"DefaultMappingRole\",\"group_id\":2,\"name\":\"Singer 1 and 2\",\"element_type\":1,\"element_id\":1}");
        }

        [Test]
        public void TestDefaultMappingRoleDeserializer()
        {
            const string json = "{\"$type\":\"DefaultMappingRole\",\"group_id\":2,\"name\":\"Singer 1 and 2\",\"element_type\":1,\"element_id\":1}";
            var result = JsonConvert.DeserializeObject<IMappingRole>(json, CreateSettings()) as DefaultMappingRole;
            var actual = new DefaultMappingRole
            {
                Name = "Singer 1 and 2",
                ElementType = ElementType.LyricLayout,
                ElementId = 1,
                GroupId = 2,
            };

            Assert.NotNull(result);
            Assert.AreEqual(result.Name, actual.Name);
            Assert.AreEqual(result.ElementType, actual.ElementType);
            Assert.AreEqual(result.ElementId, actual.ElementId);
            Assert.AreEqual(result.GroupId, actual.GroupId);
        }
    }
}
