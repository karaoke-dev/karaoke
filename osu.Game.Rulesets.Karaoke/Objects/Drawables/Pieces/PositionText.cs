// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables.Pieces
{
    public struct PositionText
    {
        public PositionText(string text, int startIndex, int endIndex)
        {
            Text = text;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public string Text { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }
    }
}
