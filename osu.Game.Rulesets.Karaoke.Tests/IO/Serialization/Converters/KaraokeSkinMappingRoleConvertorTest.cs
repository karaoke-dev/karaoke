// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Rulesets.Karaoke.Skinning.MappingRoles;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

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

            const string expected = "{\"$type\":\"DefaultMappingRole\",\"group_id\":2,\"name\":\"Singer 1 and 2\",\"element_type\":1,\"element_id\":1}";
            string actual = JsonConvert.SerializeObject(group, CreateSettings());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDefaultMappingRoleDeserializer()
        {
            const string json = "{\"$type\":\"DefaultMappingRole\",\"group_id\":2,\"name\":\"Singer 1 and 2\",\"element_type\":1,\"element_id\":1}";

            var expected = new DefaultMappingRole
            {
                Name = "Singer 1 and 2",
                ElementType = ElementType.LyricLayout,
                ElementId = 1,
                GroupId = 2,
            };
            var actual = (DefaultMappingRole)JsonConvert.DeserializeObject<IMappingRole>(json, CreateSettings())!;
            ObjectAssert.ArePropertyEqual(expected, actual);
        }
    }
}
