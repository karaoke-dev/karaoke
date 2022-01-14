// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Default;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas;
using osu.Game.Rulesets.Karaoke.Skinning.Tools;
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
            string text = Text;
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
                int charIndex = textIndex.Index;
                float startPosition = GetTextIndexPosition(new TextIndex(charIndex)).X;
                float endPosition = GetTextIndexPosition(new TextIndex(charIndex, TextIndex.IndexState.End)).X;

                if (textIndex.State == TextIndex.IndexState.Start)
                    return startPosition + (endPosition - startPosition) / 2;

                return endPosition;
            }
        }

        public float LineBaseHeight
        {
            get
            {
                var spriteText = getSpriteText();
                if (spriteText == null)
                    throw new ArgumentNullException(nameof(spriteText));

                return spriteText.LineBaseHeight;
            }
        }

        public RectangleF GetTextTagPosition(ITextTag textTag)
        {
            var spriteText = getSpriteText();
            if (spriteText == null)
                throw new ArgumentNullException(nameof(spriteText));

            return textTag switch
            {
                RubyTag rubyTag => spriteText.GetRubyTagPosition(rubyTag),
                RomajiTag romajiTag => spriteText.GetRomajiTagPosition(romajiTag),
                _ => throw new ArgumentOutOfRangeException(nameof(textTag))
            };
        }

        public Vector2 GetTimeTagPosition(TimeTag timeTag)
        {
            var basePosition = GetTextIndexPosition(timeTag.Index);
            float extraPosition = extraSpacing(TimeTagsBindable, timeTag);
            return basePosition + new Vector2(extraPosition, 0);

            static float extraSpacing(IEnumerable<TimeTag> timeTagsInLyric, TimeTag timeTag)
            {
                bool isStart = timeTag.Index.State == TextIndex.IndexState.Start;
                var timeTags = isStart ? timeTagsInLyric.Reverse() : timeTagsInLyric;
                int duplicatedTagAmount = timeTags.SkipWhile(t => t != timeTag).Count(x => x.Index == timeTag.Index) - 1;
                int spacing = duplicatedTagAmount * time_tag_spacing * (isStart ? 1 : -1);
                return spacing;
            }
        }

        public Vector2 GetTextIndexPosition(TextIndex index)
        {
            var spriteText = getSpriteText();
            if (spriteText == null)
                throw new ArgumentNullException(nameof(spriteText));

            return spriteText.GetTimeTagPosition(index);
        }

        private EditorLyricSpriteText getSpriteText()
            => (InternalChildren.FirstOrDefault() as MaskingContainer<EditorLyricSpriteText>)?.Child;

        [BackgroundDependencyLoader(true)]
        private void load(ISkinSource skin, ShaderManager shaderManager)
        {
            // this is a temp way to apply style.
            skin.GetConfig<KaraokeSkinLookup, LyricStyle>(new KaraokeSkinLookup(KaraokeSkinConfiguration.LyricStyle, HitObject.Singers))?.BindValueChanged(lyricStyle =>
            {
                var newStyle = lyricStyle.NewValue;
                if (newStyle == null)
                    return;

                LeftLyricTextShaders = SkinConvertorTool.ConvertLeftSideShader(shaderManager, newStyle);
                RightLyricTextShaders = SkinConvertorTool.ConvertRightSideShader(shaderManager, newStyle);
            }, true);

            skin.GetConfig<KaraokeSkinLookup, LyricConfig>(new KaraokeSkinLookup(KaraokeSkinConfiguration.LyricConfig, HitObject.Singers))?.BindValueChanged(lyricConfig =>
            {
                var newConfig = lyricConfig.NewValue;
                if (newConfig == null)
                    return;

                // Apply text font info
                var lyricFont = newConfig.MainTextFont;
                var rubyFont = newConfig.RubyTextFont;
                var romajiFont = newConfig.RomajiTextFont;

                Font = getFont(lyricFont.Size);
                RubyFont = getFont(rubyFont.Size);
                RomajiFont = getFont(romajiFont.Size);

                static FontUsage getFont(float? charSize = null)
                    => FontUsage.Default.With(size: charSize * 2);
            }, true);
        }

        public class EditorLyricSpriteText : LyricSpriteText
        {
            public RectangleF GetRubyTagPosition(RubyTag rubyTag)
            {
                var matchedRuby = Rubies.FirstOrDefault(x => propertyMatched(x, rubyTag));
                int rubyIndex = Rubies.IndexOf(matchedRuby);
                if (rubyIndex < 0)
                    throw new ArgumentOutOfRangeException(nameof(rubyIndex));

                int startCharacterIndex = Text.Length + skinIndex(Rubies, rubyIndex);
                int count = matchedRuby.Text.Length;
                var rectangles = Characters.ToList().GetRange(startCharacterIndex, count).Select(x => x.DrawRectangle).ToArray();
                return RectangleFUtils.Union(rectangles);
            }

            public RectangleF GetRomajiTagPosition(RomajiTag romajiTag)
            {
                var matchedRomaji = Romajies.FirstOrDefault(x => propertyMatched(x, romajiTag));
                int romajiIndex = Romajies.IndexOf(matchedRomaji);
                if (romajiIndex < 0)
                    throw new ArgumentOutOfRangeException(nameof(romajiIndex));

                int startCharacterIndex = Text.Length + skinIndex(Rubies, Rubies.Count) + skinIndex(Romajies, romajiIndex);
                int count = matchedRomaji.Text.Length;
                var rectangles = Characters.ToList().GetRange(startCharacterIndex, count).Select(x => x.DrawRectangle).ToArray();
                return RectangleFUtils.Union(rectangles);
            }

            public Vector2 GetTimeTagPosition(TextIndex index)
            {
                if (string.IsNullOrEmpty(Text))
                    return default;

                int charIndex = Math.Min(index.Index, Text.Length - 1);
                var character = Characters[charIndex];
                var drawRectangle = character.DrawRectangle;

                float x = index.State == TextIndex.IndexState.Start ? drawRectangle.Left : drawRectangle.Right;
                float y = drawRectangle.Top - character.YOffset + LineBaseHeight;
                return new Vector2(x, y);
            }

            private int skinIndex(IEnumerable<PositionText> positionTexts, int endIndex)
                => positionTexts.Where((_, i) => i < endIndex).Sum(x => x.Text.Length);

            private bool propertyMatched(PositionText positionText, ITextTag textTag)
                => positionText.StartIndex == textTag.StartIndex && positionText.EndIndex == textTag.EndIndex && positionText.Text == textTag.Text;
        }
    }
}
