﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Localisation.Edit.Generator;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags
{
    public abstract class RomajiTagGenerator<T> : ILyricPropertyGenerator<RomajiTag[]> where T : RomajiTagGeneratorConfig
    {
        protected T Config { get; }

        protected RomajiTagGenerator(T config)
        {
            Config = config;
        }

        public LocalisableString? GetInvalidMessage(Lyric lyric)
        {
            if (string.IsNullOrWhiteSpace(lyric.Text))
                return RomajiTagGeneratorStrings.LyricShouldNotBeEmpty;

            return null;
        }

        public abstract RomajiTag[] Generate(Lyric lyric);
    }
}
