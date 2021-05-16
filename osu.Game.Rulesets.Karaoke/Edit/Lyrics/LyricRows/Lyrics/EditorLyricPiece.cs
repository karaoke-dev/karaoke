// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Default;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Fonts;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics
{
    public class EditorLyricPiece : DefaultLyricPiece<EditorLyricPiece.EditorLyricSpriteText>
    {
        public Action<LyricFont> ApplyFontAction;

        protected Lyric HitObject;

        public EditorLyricPiece(Lyric lyric)
            : base(lyric)
        {
            HitObject = lyric;

            DisplayRuby = true;
            DisplayRomaji = true;
        }

        public override void ApplyFont(LyricFont font)
        {
            ApplyFontAction?.Invoke(font);
            base.ApplyFont(font);
        }

        public float GetPercentageWidth(int startIndex, int endIndex, float percentage = 0)
        {
            return GetPercentageWidth(getTextIndexByIndex(startIndex), getTextIndexByIndex(endIndex), percentage);

            // todo : it's a temp way to get position.
            TextIndex getTextIndexByIndex(int index)
            {
                if (Text?.Length <= index)
                    return new TextIndex(index - 1, TextIndex.IndexState.End);

                return new TextIndex(index);
            }
        }

        public TextIndex GetHoverIndex(float position)
        {
            var text = Text;
            if (string.IsNullOrEmpty(text))
                return new TextIndex();

            for (int i = 0; i < text.Length; i++)
            {
                if (GetPercentageWidth(i, i + 1, 0.5f) > position)
                    return new TextIndex(i);

                if (GetPercentageWidth(i, i + 1, 1f) > position)
                    return new TextIndex(i, TextIndex.IndexState.End);
            }

            return new TextIndex(text.Length - 1, TextIndex.IndexState.End);
        }

        public RectangleF GetTextTagPosition(ITextTag textTag)
        {
            var spriteText = (InternalChildren.FirstOrDefault() as Container)?.Child as EditorLyricSpriteText;
            if (spriteText == null)
                return new RectangleF();

            switch (textTag)
            {
                case RubyTag rubyTag:
                    return spriteText.GetRubyTagPosition(rubyTag);

                case RomajiTag romajiTag:
                    return spriteText.GetRomajiTagPosition(romajiTag);

                default:
                    throw new ArgumentOutOfRangeException(nameof(textTag));
            }
        }

        [BackgroundDependencyLoader(true)]
        private void load(ISkinSource skin)
        {
            // this is a temp way to apply font.
            skin.GetConfig<KaraokeSkinLookup, LyricFont>(new KaraokeSkinLookup(KaraokeSkinConfiguration.LyricStyle, HitObject.Singers))?.BindValueChanged(karaokeFont =>
            {
                var newFont = karaokeFont.NewValue;
                if (newFont == null)
                    return;

                ApplyFont(karaokeFont.NewValue);

                // Apply text font info
                var lyricFont = newFont.LyricTextFontInfo.LyricTextFontInfo;
                Font = getFont(lyricFont.CharSize);

                var rubyFont = newFont.RubyTextFontInfo.LyricTextFontInfo;
                RubyFont = getFont(rubyFont.CharSize);

                var romajiFont = newFont.RomajiTextFontInfo.LyricTextFontInfo;
                RomajiFont = getFont(romajiFont.CharSize);

                static FontUsage getFont(float? charSize = null)
                    => FontUsage.Default.With(size: charSize * 2);
            }, true);
        }

        public class EditorLyricSpriteText : LyricSpriteText
        {
            public RectangleF GetRubyTagPosition(RubyTag rubyTag)
            {
                var matchedRuby = Rubies.FirstOrDefault(x => propertyMatched(x, rubyTag));
                var rubyIndex = Rubies.IndexOf(matchedRuby);
                if (rubyIndex < 0)
                    throw new IndexOutOfRangeException(nameof(rubyIndex));

                var startCharacterIndex = Text.Length + skinIndex(Rubies, rubyIndex);
                var count = matchedRuby.Text.Length;
                var rectangles = Characters.GetRange(startCharacterIndex, count).Select(x => x.DrawRectangle).ToArray();
                return RectangleFUtils.Union(rectangles);
            }

            public RectangleF GetRomajiTagPosition(RomajiTag romajiTag)
            {
                var matchedRomaji = Romajies.FirstOrDefault(x => propertyMatched(x, romajiTag));
                var romajiIndex = Romajies.IndexOf(matchedRomaji);
                if (romajiIndex < 0)
                    throw new IndexOutOfRangeException(nameof(romajiIndex));

                var startCharacterIndex = Text.Length + skinIndex(Rubies, Rubies.Length) + skinIndex(Romajies, romajiIndex);
                var count = matchedRomaji.Text.Length;
                var rectangles = Characters.GetRange(startCharacterIndex, count).Select(x => x.DrawRectangle).ToArray();
                return RectangleFUtils.Union(rectangles);
            }

            private int skinIndex(PositionText[] positionTexts, int endIndex)
                => positionTexts.Where((x, i) => i < endIndex).Sum(x => x.Text.Length);

            private bool propertyMatched(PositionText positionText, ITextTag textTag)
                => positionText.StartIndex == textTag.StartIndex && positionText.EndIndex == textTag.EndIndex && positionText.Text == textTag.Text;
        }
    }
}
