﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags
{
    public abstract class RubyTagGenerator<T> : RubyTagGenerator where T : RubyTagGeneratorConfig
    {
        protected T Config { get; }

        protected RubyTagGenerator(T config)
        {
            Config = config;
        }
    }

    public abstract class RubyTagGenerator
    {
        public abstract RubyTag[] CreateRubyTags(Lyric lyric);
    }
}
