// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public abstract partial class LyricPropertyChangeHandler : HitObjectPropertyChangeHandler<Lyric>, ILyricPropertyChangeHandler
    {
        #region Auto-Generate

        [Resolved]
        private KaraokeRulesetEditGeneratorConfigManager? generatorConfigManager { get; set; }

        protected LyricPropertyDetector<TProperty, TConfig> GetDetector<TProperty, TConfig>()
            where TConfig : GeneratorConfig, new()
        {
            // using reflection to get the detector.
            var config = getGeneratorConfig<TConfig>();
            var detectorType = getChildType(typeof(LyricPropertyDetector<,>).MakeGenericType(typeof(TProperty), typeof(TConfig)));
            var detector = Activator.CreateInstance(detectorType, config) as LyricPropertyDetector<TProperty, TConfig>;
            return detector ?? throw new InvalidOperationException();
        }

        protected LyricPropertyDetector<TProperty, TConfig> GetDetector<TProperty, TConfig>(IEnumerable<Lyric> lyrics)
            where TConfig : GeneratorConfig, new()
        {
            // using reflection to get the detector.
            var config = getGeneratorConfig<TConfig>();
            var detectorType = getChildType(typeof(LyricPropertyDetector<,>).MakeGenericType(typeof(TProperty), typeof(TConfig)));
            var detector = Activator.CreateInstance(detectorType, lyrics, config) as LyricPropertyDetector<TProperty, TConfig>;
            return detector ?? throw new InvalidOperationException();
        }

        protected LyricPropertyGenerator<TProperty, TConfig> GetGenerator<TProperty, TConfig>()
            where TConfig : GeneratorConfig, new()
        {
            // using reflection to get the generator.
            var config = getGeneratorConfig<TConfig>();
            var generatorType = getChildType(typeof(LyricPropertyGenerator<,>).MakeGenericType(typeof(TProperty), typeof(TConfig)));
            var generator = Activator.CreateInstance(generatorType, config) as LyricPropertyGenerator<TProperty, TConfig>;
            return generator ?? throw new InvalidOperationException();
        }

        protected LyricGeneratorSelector<TProperty, TBaseConfig> GetSelector<TProperty, TBaseConfig>()
            where TBaseConfig : GeneratorConfig
        {
            // using reflection to get the selector.
            var selectorType = getChildType(typeof(LyricGeneratorSelector<,>).MakeGenericType(typeof(TProperty), typeof(TBaseConfig)));
            var selector = Activator.CreateInstance(selectorType, generatorConfigManager) as LyricGeneratorSelector<TProperty, TBaseConfig>;
            return selector ?? throw new InvalidOperationException();
        }

        private Type getChildType(Type type)
        {
            // should get the assembly that the has the class GeneratorConfig.
            var assembly = typeof(GeneratorConfig).Assembly;
            return assembly.GetTypes()
                           .Single(x => type.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);
        }

        private TConfig getGeneratorConfig<TConfig>()
            where TConfig : GeneratorConfig, new()
        {
            if (generatorConfigManager == null)
                throw new InvalidOperationException();

            return generatorConfigManager.Get<TConfig>();
        }

        protected bool CanDetect<T>(PropertyDetector<Lyric, T> detector)
            => HitObjects.Where(x => !IsWritePropertyLocked(x)).Any(detector.CanDetect);

        protected bool CanGenerate<T>(PropertyGenerator<Lyric, T> generator)
            => HitObjects.Where(x => !IsWritePropertyLocked(x)).Any(generator.CanGenerate);

        protected IDictionary<Lyric, LocalisableString> GetInvalidMessageFromDetector<T>(PropertyDetector<Lyric, T> detector)
            => HitObjects.Select(x => new KeyValuePair<Lyric, LocalisableString?>(x, detector.GetInvalidMessage(x) ?? getReferenceLyricInvalidMessage(x)))
                         .Where(x => x.Value != null)
                         .ToDictionary(k => k.Key, v => v.Value!.Value);

        protected IDictionary<Lyric, LocalisableString> GetInvalidMessageFromGenerator<T>(PropertyGenerator<Lyric, T> generator)
            => HitObjects.Select(x => new KeyValuePair<Lyric, LocalisableString?>(x, generator.GetInvalidMessage(x) ?? getReferenceLyricInvalidMessage(x)))
                         .Where(x => x.Value != null)
                         .ToDictionary(k => k.Key, v => v.Value!.Value);

        private LocalisableString? getReferenceLyricInvalidMessage(Lyric lyric)
        {
            bool locked = IsWritePropertyLocked(lyric);
            return locked ? "Cannot modify property because has reference lyric." : default(LocalisableString?);
        }

        #endregion
    }
}
