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
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components
{
    public class EditorLyricPiece : DefaultLyricPiece<EditorLyricPiece.EditorLyricSpriteText>
    {
        private const int time_tag_spacing = 8;

        public Lyric HitObject;

        public EditorLyricPiece(Lyric lyric)
            : base(lyric)
        {
            HitObject = lyric;

            DisplayRuby = true;
            DisplayRomaji = true;
        }

        public TimeTag GetHoverTimeTag(float position)
        {
            var textIndex = GetHoverIndex(position);
            return HitObject?.TimeTags.FirstOrDefault(x => x.Index == textIndex);
        }

        public TextIndex GetHoverIndex(float position)
        {
            var text = Text;
            if (string.IsNullOrEmpty(text))
                return new TextIndex();

            for (int i = 0; i < text.Length; i++)
            {
                if (getTriggerPositionByTimeIndex(new TextIndex(i)) > position)
                    return new TextIndex(i);

                if (getTriggerPositionByTimeIndex(new TextIndex(i, TextIndex.IndexState.End)) > position)
                    return new TextIndex(i, TextIndex.IndexState.End);
            }

            return new TextIndex(text.Length - 1, TextIndex.IndexState.End);

            // todo : might have a better way to call GetTextIndexPosition just once.
            float getTriggerPositionByTimeIndex(TextIndex textIndex)
            {
                var charIndex = textIndex.Index;
                var startPosition = GetTextIndexPosition(new TextIndex(charIndex)).X;
                var endPosition = GetTextIndexPosition(new TextIndex(charIndex, TextIndex.IndexState.End)).X;

                if (textIndex.State == TextIndex.IndexState.Start)
                    return startPosition + (endPosition - startPosition) / 2;

                return endPosition;
            }
        }

        public float GetTextHeight()
        {
            var spriteText = getSpriteText();
            if (spriteText == null)
                return 0;

            return spriteText.GetTextHeight();
        }

        public RectangleF GetTextTagPosition(ITextTag textTag)
        {
            var spriteText = getSpriteText();
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

        public Vector2 GetTimeTagPosition(TimeTag timeTag)
        {
            var basePosition = GetTextIndexPosition(timeTag.Index);
            var extraPosition = extraSpacing(TimeTagsBindable.Value, timeTag);
            return basePosition + new Vector2(extraPosition);

            static float extraSpacing(TimeTag[] timeTagsInLyric, TimeTag timeTag)
            {
                var isStart = timeTag.Index.State == TextIndex.IndexState.Start;
                var timeTags = isStart ? timeTagsInLyric.Reverse() : timeTagsInLyric;
                var duplicatedTagAmount = timeTags.SkipWhile(t => t != timeTag).Count(x => x.Index == timeTag.Index) - 1;
                var spacing = duplicatedTagAmount * time_tag_spacing * (isStart ? 1 : -1);
                return spacing;
            }
        }

        public Vector2 GetTextIndexPosition(TextIndex index)
        {
            var spriteText = getSpriteText();
            if (spriteText == null)
                return new Vector2();

            return spriteText.GetTimeTagPosition(index);
        }

        private EditorLyricSpriteText getSpriteText()
            => (InternalChildren.FirstOrDefault() as Container)?.Child as EditorLyricSpriteText;

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
            public float GetTextHeight()
            {
                if (string.IsNullOrEmpty(Text))
                    return 0;

                return Characters.FirstOrDefault().Height;
            }

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

            public Vector2 GetTimeTagPosition(TextIndex index)
            {
                if (string.IsNullOrEmpty(Text))
                    return default;

                var charIndex = Math.Min(index.Index, Text.Length - 1);
                var drawRectangle = Characters[charIndex].DrawRectangle;
                return index.State == TextIndex.IndexState.Start ? drawRectangle.BottomLeft : drawRectangle.BottomRight;
            }

            private int skinIndex(PositionText[] positionTexts, int endIndex)
                => positionTexts.Where((x, i) => i < endIndex).Sum(x => x.Text.Length);

            private bool propertyMatched(PositionText positionText, ITextTag textTag)
                => positionText.StartIndex == textTag.StartIndex && positionText.EndIndex == textTag.EndIndex && positionText.Text == textTag.Text;
        }
    }
}
