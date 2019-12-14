// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public readonly struct KaraokeSkinLookup
    {
        public KaraokeSkinLookup(KaraokeSkinConfiguration config, int lookup)
        {
            Config = config;
            Lookup = lookup;
            Editor = false;
        }

        public KaraokeSkinConfiguration Config { get; }
        public int Lookup { get; }
        public bool Editor { get; }
    }
}
