// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class TranslateDictionary : KaraokeHitObject
    {
        public IDictionary<string, List<string>> Translates { get; } = new Dictionary<string, List<string>>();
    }
}
