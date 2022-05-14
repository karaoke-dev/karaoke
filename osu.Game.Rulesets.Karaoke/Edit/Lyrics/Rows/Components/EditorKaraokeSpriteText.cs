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
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Rulesets.Karaoke.Skinning.Tools;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components
{
    public class EditorKaraokeSpriteText : DrawableKaraokeSpriteText<EditorKaraokeSpriteText.EditorLyricSpriteText>
    {
        private const int time_tag_spacing = 8;

        public Lyric HitObject;

        public EditorKaraokeSpriteText(Lyric lyric)
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
            float extraPosition = extraSpacing(HitObject.TimeTags, timeTag);
            return basePosition + new Vector2(extraPosition, 0);

            static float extraSpacing(IList<TimeTag> timeTagsInLyric, TimeTag timeTag)
            {
                var textIndex = timeTag.Index;
                var timeTags = TextIndexUtils.GetValueByState(textIndex, timeTagsInLyric.Reverse(), timeTagsInLyric);
                int duplicatedTagAmount = timeTags.SkipWhile(t => t != timeTag).Count(x => x.Index == textIndex) - 1;
                int spacing = duplicatedTagAmount * time_tag_spacing * TextIndexUtils.GetValueByState(textIndex, 1, -1);
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
            skin.GetConfig<Lyric, LyricStyle>(HitObject)?.BindValueChanged(lyricStyle =>
            {
                var newStyle = lyricStyle.NewValue;
                if (newStyle == null)
                    return;

                LeftLyricTextShaders = SkinConvertorTool.ConvertLeftSideShader(shaderManager, newStyle);
                RightLyricTextShaders = SkinConvertorTool.ConvertRightSideShader(shaderManager, newStyle);
            }, true);

            skin.GetConfig<Lyric, LyricConfig>(HitObject)?.BindValueChanged(lyricConfig =>
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

        public override bool RemoveCompletedTransforms => false;

        public class EditorLyricSpriteText : LyricSpriteText
        {
            public RectangleF GetRubyTagPosition(RubyTag rubyTag)
                => GetRubyTagPosition(TextTagUtils.ToPositionText(rubyTag));

            public RectangleF GetRomajiTagPosition(RomajiTag romajiTag)
                => GetRomajiTagPosition(TextTagUtils.ToPositionText(romajiTag));

            public Vector2 GetTimeTagPosition(TextIndex index)
            {
                var drawRectangle = GetCharacterRectangle(index.Index);
                return TextIndexUtils.GetValueByState(index, drawRectangle.BottomLeft, drawRectangle.BottomRight);
            }
        }
    }
}
