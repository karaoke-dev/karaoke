// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags
{
    public abstract class RubyTagGenerator<T> : ILyricPropertyGenerator<RubyTag[]> where T : RubyTagGeneratorConfig
    {
        protected T Config { get; }

        protected RubyTagGenerator(T config)
        {
            Config = config;
        }

        public bool CanGenerate(Lyric lyric)
            => !string.IsNullOrWhiteSpace(lyric.Text);

        public abstract RubyTag[] Generate(Lyric lyric);
    }
}
