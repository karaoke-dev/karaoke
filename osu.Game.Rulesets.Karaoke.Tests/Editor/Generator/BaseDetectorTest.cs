// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator
{
    public abstract class BaseDetectorTest<TDetector, TObject, TConfig>
        where TDetector : class, ILyricPropertyDetector<TObject> where TConfig : new()
    {
        protected static TConfig GeneratorConfig(params string[] properties)
        {
            var config = new TConfig();
            if (properties == null)
                return config;

            foreach (string propertyName in properties)
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

        protected static void CheckCanDetect(string text, bool canDetect, TConfig config)
        {
            var lyric = new Lyric { Text = text };
            CheckCanDetect(lyric, canDetect, config);
        }

        protected static void CheckCanDetect(Lyric lyric, bool canDetect, TConfig config)
        {
            if (Activator.CreateInstance(typeof(TDetector), config) is not TDetector detector)
                throw new ArgumentNullException(nameof(detector));

            bool actual = detector.CanDetect(lyric);
            Assert.AreEqual(canDetect, actual);
        }

        protected void CheckDetectResult(string text, TObject expected, TConfig config)
        {
            var lyric = new Lyric { Text = text };
            CheckDetectResult(lyric, expected, config);
        }

        protected void CheckDetectResult(Lyric lyric, TObject expected, TConfig config)
        {
            if (Activator.CreateInstance(typeof(TDetector), config) is not TDetector detector)
                throw new ArgumentNullException(nameof(detector));

            // create time tag and actually time tag.
            var actual = detector.Detect(lyric);
            AssertEqual(expected, actual);
        }

        protected abstract void AssertEqual(TObject expected, TObject actual);
    }
}
