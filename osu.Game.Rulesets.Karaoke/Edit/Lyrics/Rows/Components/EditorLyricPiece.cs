// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
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

                // Apply text font info
                var lyricFont = newStyle.MainTextFont;
                var rubyFont = newStyle.RubyTextFont;
                var romajiFont = newStyle.RomajiTextFont;

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
                var rubyIndex = Rubies.IndexOf(matchedRuby);
                if (rubyIndex < 0)
                    throw new ArgumentOutOfRangeException(nameof(rubyIndex));

                var startCharacterIndex = Text.Length + skinIndex(Rubies, rubyIndex);
                var count = matchedRuby.Text.Length;
                var rectangles = Characters.ToList().GetRange(startCharacterIndex, count).Select(x => x.DrawRectangle).ToArray();
                return RectangleFUtils.Union(rectangles);
            }

            public RectangleF GetRomajiTagPosition(RomajiTag romajiTag)
            {
                var matchedRomaji = Romajies.FirstOrDefault(x => propertyMatched(x, romajiTag));
                var romajiIndex = Romajies.IndexOf(matchedRomaji);
                if (romajiIndex < 0)
                    throw new ArgumentOutOfRangeException(nameof(romajiIndex));

                var startCharacterIndex = Text.Length + skinIndex(Rubies, Rubies.Length) + skinIndex(Romajies, romajiIndex);
                var count = matchedRomaji.Text.Length;
                var rectangles = Characters.ToList().GetRange(startCharacterIndex, count).Select(x => x.DrawRectangle).ToArray();
                return RectangleFUtils.Union(rectangles);
            }

            public Vector2 GetTimeTagPosition(TextIndex index)
            {
                if (string.IsNullOrEmpty(Text))
                    return default;

                var charIndex = Math.Min(index.Index, Text.Length - 1);
                var character = Characters[charIndex];
                var drawRectangle = character.DrawRectangle;

                var x = index.State == TextIndex.IndexState.Start ? drawRectangle.Left : drawRectangle.Right;
                var y = drawRectangle.Top - character.YOffset + LineBaseHeight;
                return new Vector2(x, y);
            }

            private int skinIndex(PositionText[] positionTexts, int endIndex)
                => positionTexts.Where((_, i) => i < endIndex).Sum(x => x.Text.Length);

            private bool propertyMatched(PositionText positionText, ITextTag textTag)
                => positionText.StartIndex == textTag.StartIndex && positionText.EndIndex == textTag.EndIndex && positionText.Text == textTag.Text;
        }
    }
}
