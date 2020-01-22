// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Objects.Types
{
    public interface ITag
    {
        string Text { get; set; }

        int StartIndex { get; set; }

        int EndIndex { get; set; }
    }
}
