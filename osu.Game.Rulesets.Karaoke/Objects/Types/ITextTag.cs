// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Objects.Types
{
    public interface ITextTag : IHasText
    {
        int StartIndex { get; set; }

        int EndIndex { get; set; }
    }
}
