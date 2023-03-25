// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Generator;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator;

public class GeneratorConfigExtensionTest
{
    [Test]
    public void TestGetOrderedConfigsSourceDictionary()
    {
        var config = new TestGeneratorConfig();
        var defaultCategory = new ConfigCategoryAttribute("Category 0");
        var result = config.GetOrderedConfigsSourceDictionary(defaultCategory);

        Assert.AreEqual(result[defaultCategory].Length, 1);
        Assert.AreEqual(result[new ConfigCategoryAttribute("Category 1")].Length, 2);
    }

    [Test]
    public void TestGetOrderedConfigsSourceProperties()
    {
        var config = new TestGeneratorConfig();
        var result = config.GetOrderedConfigsSourceProperties().ToArray();

        checkSingleProperty(result[0], "Culture infos");
        checkSingleProperty(result[1], "Boolean");
        checkSingleProperty(result[2], "Double"); // todo: this shit should be the first one.

        static void checkSingleProperty((ConfigSourceAttribute, ConfigCategoryAttribute?, PropertyInfo) property, string title) =>
            Assert.AreEqual(property.Item1.Label.ToString(), title);
    }

    [Test]
    public void TestGetConfigSourceProperties()
    {
        var config = new TestGeneratorConfig();
        var result = config.GetConfigSourceProperties().ToArray();

        checkSingleProperty(result[0], "Double", "Edit the double value");
        checkSingleProperty(result[1], "Boolean", "Edit the boolean value", "Category 1");
        checkSingleProperty(result[2], "Culture infos", "Edit the culture infos", "Category 1");

        static void checkSingleProperty((ConfigSourceAttribute, ConfigCategoryAttribute?, PropertyInfo) property, string title, string description, string? category = null)
        {
            Assert.AreEqual(property.Item1.Label.ToString(), title);
            Assert.AreEqual(property.Item1.Description.ToString(), description);
            Assert.AreEqual(property.Item2?.Category.ToString(), category);
        }
    }

    private class TestGeneratorConfig : GeneratorConfig
    {
        [ConfigSource("Double", "Edit the double value")]
        public Bindable<double> Double { get; } = new BindableDouble();

        [ConfigCategory("Category 1")]
        [ConfigSource("Boolean", "Edit the boolean value", 1)]
        public Bindable<bool> Boolean { get; } = new BindableBool();

        [ConfigCategory("Category 1")]
        [ConfigSource("Culture infos", "Edit the culture infos", 0)]
        public Bindable<CultureInfo[]> CultureInfos { get; } = new(Array.Empty<CultureInfo>());
    }
}
