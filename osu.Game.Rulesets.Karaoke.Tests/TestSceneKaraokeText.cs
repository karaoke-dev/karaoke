// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects.Drawables.Pieces;
using osu.Game.Tests.Visual;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    public class TestSceneKaraokeText : OsuTestScene
    {
        public TestSceneKaraokeText()
        {
            AddStep("Text", () => SetContents(() => testSingle("ローマ字")));
            AddStep("Text and ruby", () => SetContents(() => testSingle("ローマ字", new[] { null, null, null, "じ" })));
            AddStep("Text and romaji", () => SetContents(() => testSingle("ローマ字", null, new[] { "ro", null, "ma", "ji" })));
            AddStep("Text and ruby and romaji", () => SetContents(() => testSingle("ローマ字", new[] { null, null, null, "じ" }, new[] { "ro", null, "ma", "ji" })));

            AddStep("Text and ruby", () => SetContents(() => testSingle("ローマ字", "ろーまじ")));
            AddStep("Text and romaji", () => SetContents(() => testSingle("ローマ字", null, "Romaji")));
            AddStep("Text and ruby and romaji", () => SetContents(() => testSingle("ローマ字", "ろーまじ", "Romaji")));
        }

        public void SetContents(Func<Drawable> creationFunction)
        {
            Child = creationFunction();
        }

        private Drawable testSingle(string text, string[] rubyStrings, string[] romajiStrings = null)
        {
            var rubies = new List<PositionText>();

            for (int i = 0; i < rubyStrings?.Length; i++)
            {
                rubies.Add(new PositionText(rubyStrings[i], i, i + 1));
            }

            var romajies = new List<PositionText>();

            for (int i = 0; i < romajiStrings?.Length; i++)
            {
                romajies.Add(new PositionText(romajiStrings[i], i, i + 1));
            }

            return testSingle(text, rubies, romajies);
        }

        private Drawable testSingle(string text, string ruby, string romaji = null)
        {
            var starIndex = 0;
            var endIndex = text.Length;

            var rubies = new List<PositionText>
            {
                new PositionText(ruby, starIndex, endIndex)
            };

            var romajies = new List<PositionText>
            {
                new PositionText(romaji, starIndex, endIndex)
            };

            return testSingle(text, rubies, romajies);
        }

        private Drawable testSingle(string text, List<PositionText> rubies = null, List<PositionText> romajies = null)
        {
            return new TestKaraokeText
            {
                Text = text,
                Rubies = rubies?.ToArray(),
                Romajis = romajies?.ToArray(),
                Margin = new MarginPadding(30),
            };
        }

        protected class TestKaraokeText : KaraokeText
        {
        }
    }
}
