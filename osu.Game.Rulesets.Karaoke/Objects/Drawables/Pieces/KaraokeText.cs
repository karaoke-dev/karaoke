// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Text;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables.Pieces
{
    public class KaraokeText : SpriteText
    {
        public KaraokeText()
        {
            Font = new FontUsage(null, 48);
        }

        private PositionText[] rubies;

        public PositionText[] Rubies
        {
            get => rubies;
            set
            {
                rubies = filterValidValues(value);

                // Trigger update text
                Text = Text;
            }
        }

        private PositionText[] romajis;

        public PositionText[] Romajis
        {
            get => romajis;
            set
            {
                romajis = filterValidValues(value);

                // Trigger update text
                Text = Text;
            }
        }

        private PositionText[] filterValidValues(PositionText[] texts)
        {
            string text = Text;
            return texts?.Where(positionText => Math.Min(positionText.StartIndex, positionText.EndIndex) >= 0
                                                && Math.Max(positionText.StartIndex, positionText.EndIndex) <= text.Length
                                                && positionText.EndIndex > positionText.StartIndex).ToArray();
        }

        private FontUsage rubyFont = FontUsage.Default;

        /// <summary>
        /// Contains information on the font used to display the text.
        /// </summary>
        public FontUsage RubyFont
        {
            get => rubyFont;
            set
            {
                rubyFont = value;

                // Trigger update ruby
                Font = Font;
            }
        }

        private FontUsage romajiFont = FontUsage.Default;

        /// <summary>
        /// Contains information on the font used to display the text.
        /// </summary>
        public FontUsage RomajiFont
        {
            get => romajiFont;
            set
            {
                romajiFont = value;

                // Trigger update ruby
                Font = Font;
            }
        }

        private int rubyMargin;

        public int RubyMargin
        {
            get => rubyMargin;
            set
            {
                rubyMargin = value;

                // Trigger update text
                Text = Text;
            }
        }

        private int romajiMargin;

        public int RomajiMargin
        {
            get => romajiMargin;
            set
            {
                romajiMargin = value;

                // Trigger update text
                Text = Text;
            }
        }

        public Vector2 RubySpacing { get; set; }

        public Vector2 RomajiSpacing { get; set; }

        protected TextBuilderGlyph[] Characters;

        /// <summary>
        /// Creates a <see cref="TextBuilder"/> to generate the character layout for this <see cref="SpriteText"/>.
        /// </summary>
        /// <param name="store">The <see cref="ITexturedGlyphLookupStore"/> where characters should be retrieved from.</param>
        /// <returns>The <see cref="TextBuilder"/>.</returns>
        protected override TextBuilder CreateTextBuilder(ITexturedGlyphLookupStore store)
        {
            // Print text
            var textBuilder = base.CreateTextBuilder(store);
            textBuilder.AddText(Text);

            var builderMaxWidth = int.MaxValue;
            var excludeCharacters = FixedWidthExcludeCharacters;
            var charactersBacking = textBuilder.Characters;

            // Save text characters
            Characters = charactersBacking.ToArray();

            // Print ruby
            var rubyYPosition = -RubyFont.Size / 2 - RubyMargin;
            createTexts(Rubies, RubyFont, rubyYPosition, RubySpacing);

            // Print romaji
            var romajiYPosition = charactersBacking.FirstOrDefault().Height + RomajiFont.Size / 2 + 5 + RomajiMargin;
            createTexts(Romajis, RomajiFont, romajiYPosition, RomajiSpacing);

            // Return TextBuilder that do not renderer text anymore
            return new TextBuilder(store, Font, builderMaxWidth, UseFullGlyphHeight,
                new Vector2(Padding.Left, Padding.Top), Spacing, null,
                excludeCharacters, FallbackCharacter);

            // Create texts
            void createTexts(PositionText[] texts, FontUsage font, float yPosition, Vector2 spacing)
            {
                if (texts != null)
                {
                    foreach (var text in texts)
                    {
                        var romajiText = text.Text;
                        if (string.IsNullOrEmpty(romajiText))
                            continue;

                        var romajiPosition = getTextPosition(text, yPosition);

                        var romajiTextBuilder = new TextBuilder(store, font, builderMaxWidth, UseFullGlyphHeight,
                            romajiPosition, spacing, charactersBacking, excludeCharacters, FallbackCharacter);

                        romajiTextBuilder.AddText(romajiText);
                    }
                }

                // Convert
                Vector2 getTextPosition(PositionText text, float y)
                {
                    var x = (charactersBacking[text.StartIndex].DrawRectangle.Left + charactersBacking[text.EndIndex - 1].DrawRectangle.Right) / 2;
                    var textWidth = text.Text.Sum(c => store.Get(font.FontName, c).Width * font.Size + Spacing.X) - Spacing.X;
                    return new Vector2(x - textWidth / 2, y);
                }
            }
        }

        public float GetPercentageWidth(int startIndex, int endIndex, float percentage = 0)
        {
            if (Characters == null || !Characters.Any())
                return 0;

            if (endIndex <= 0)
                return 0;

            var left = Characters[Math.Max(startIndex, 0)].DrawRectangle.Left;
            var right = Characters[Math.Min(endIndex - 1, Characters.Length + 1)].DrawRectangle.Right;

            var width = left * (1 - percentage) + right * percentage;
            return width + Margin.Left;
        }
    }
}
