// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags
{
    public abstract class TimeTagGeneratorConfig
    {
        public bool CheckBlankLine { get; set; }

        public bool CheckLineEndKeyUp { get; set; }

        public bool CheckWhiteSpace { get; set; }

        public bool CheckWhiteSpaceKeyUp { get; set; }

        public bool CheckWhiteSpaceAlphabet { get; set; }

        public bool CheckWhiteSpaceDigit { get; set; }

        public bool CheckWhiteSpaceAsciiSymbol { get; set; }
    }
}
