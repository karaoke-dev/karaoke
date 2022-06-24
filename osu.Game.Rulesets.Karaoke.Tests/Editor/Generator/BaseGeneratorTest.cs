// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator
{
    public abstract class BaseGeneratorTest<TGenerator, TObject, TConfig>
        where TGenerator : class, ILyricPropertyGenerator<TObject> where TConfig : new()
    {
        protected static TConfig GeneratorConfig(params string?[] properties)
        {
            var config = new TConfig();

            foreach (string? propertyName in properties)
            {
                if (propertyName == null)
                    continue;

                var theMethod = config.GetType().GetProperty(propertyName);
                if (theMethod == null)
                    throw new MissingMethodException("Config is not exist.");

                theMethod.SetValue(config, true);
            }

            return config;
        }

        protected static void CheckCanGenerate(string text, bool canGenerate, TConfig config)
        {
            var lyric = new Lyric { Text = text };
            CheckCanGenerate(lyric, canGenerate, config);
        }

        protected static void CheckCanGenerate(Lyric lyric, bool canGenerate, TConfig config)
        {
            if (Activator.CreateInstance(typeof(TGenerator), config) is not TGenerator generator)
                throw new ArgumentNullException(nameof(generator));

            bool actual = generator.CanGenerate(lyric);
            Assert.AreEqual(canGenerate, actual);
        }

        protected void CheckGenerateResult(string text, TObject expected, TConfig config)
        {
            var lyric = new Lyric { Text = text };
            CheckGenerateResult(lyric, expected, config);
        }

        protected void CheckGenerateResult(Lyric lyric, TObject expected, TConfig config)
        {
            if (Activator.CreateInstance(typeof(TGenerator), config) is not TGenerator generator)
                throw new ArgumentNullException(nameof(generator));

            // create time tag and actually time tag.
            var actual = generator.Generate(lyric);
            AssertEqual(expected, actual);
        }

        protected abstract void AssertEqual(TObject expected, TObject actual);
    }
}
