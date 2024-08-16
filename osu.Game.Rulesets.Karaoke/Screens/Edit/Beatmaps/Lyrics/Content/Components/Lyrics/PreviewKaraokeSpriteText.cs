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
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Rulesets.Karaoke.Skinning.Tools;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;

public partial class PreviewKaraokeSpriteText : DrawableKaraokeSpriteText<PreviewKaraokeSpriteText.EditorLyricSpriteText>, IPreviewLyricPositionProvider
{
    private const int time_tag_spacing = 8;

    public Lyric HitObject;

    public Action<Vector2>? SizeChanged;

    private readonly EditorLyricSpriteText spriteText;

    public PreviewKaraokeSpriteText(Lyric lyric)
        : base(lyric)
    {
        HitObject = lyric;

        // should display ruby and romanisation by default.
        DisplayType = LyricDisplayType.Lyric;
        DisplayProperty = LyricDisplayProperty.Both;

        spriteText = getSpriteText();

        EditorLyricSpriteText getSpriteText()
        {
            if (InternalChildren.First() is not Container<EditorLyricSpriteText> lyricSpriteTexts)
                throw new ArgumentNullException();

            return lyricSpriteTexts.Child;
        }
    }

    protected override void OnPropertyChanged()
    {
        triggerSizeChangedEvent();
    }

    private void triggerSizeChangedEvent()
    {
        ScheduleAfterChildren(() =>
        {
            SizeChanged?.Invoke(DrawSize);
        });
    }

    #region Text char index

    public int? GetCharIndexByPosition(Vector2 position)
    {
        for (int i = 0; i < Text.Length; i++)
        {
            var rectangle = spriteText.GetCharacterDrawRectangle(i);
            if (rectangle.Contains(position))
                return i;
        }

        return null;
    }

    public RectangleF GetRectByCharIndex(int charIndex)
    {
        if (charIndex < 0 || charIndex >= Text.Length)
            throw new ArgumentOutOfRangeException(nameof(charIndex));

        return spriteText.GetCharacterDrawRectangle(charIndex);
    }

    #endregion

    #region Text indicator

    public int? GetCharIndicatorByPosition(Vector2 position)
    {
        for (int i = 0; i < Text.Length + 1; i++)
        {
            var rect = getTriggerPositionByTimeIndex(i);
            if (rect.Contains(position))
                return i;
        }

        return null;

        RectangleF getTriggerPositionByTimeIndex(int gapIndex)
        {
            if (gapIndex == 0)
            {
                var rectangle = spriteText.GetCharacterDrawRectangle(gapIndex);
                return new RectangleF(rectangle.Left, rectangle.Top, rectangle.Width / 2, rectangle.Height);
            }

            if (gapIndex == Text.Length)
            {
                var rectangle = spriteText.GetCharacterDrawRectangle(gapIndex - 1);
                return new RectangleF(rectangle.Centre.X, rectangle.Top, rectangle.Width / 2, rectangle.Height);
            }

            var leftRectangle = spriteText.GetCharacterDrawRectangle(gapIndex - 1);
            var rightRectangle = spriteText.GetCharacterDrawRectangle(gapIndex);

            float x = leftRectangle.Centre.X;
            float y = Math.Min(leftRectangle.Y, rightRectangle.Y);
            float width = rightRectangle.Centre.X - leftRectangle.Centre.X;
            float height = Math.Max(leftRectangle.Height, rightRectangle.Height);

            return new RectangleF(x, y, width, height);
        }
    }

    public RectangleF GetRectByCharIndicator(int gapIndex)
    {
        if (gapIndex < 0 || gapIndex > Text.Length)
            throw new ArgumentOutOfRangeException(nameof(gapIndex));

        const float min_spacing_width = 1;

        if (gapIndex == 0)
        {
            var referenceRectangle = spriteText.GetCharacterDrawRectangle(gapIndex);
            return new RectangleF(referenceRectangle.X - min_spacing_width, referenceRectangle.Y, min_spacing_width, referenceRectangle.Height);
        }

        if (gapIndex == Text.Length)
        {
            var referenceRectangle = spriteText.GetCharacterDrawRectangle(gapIndex - 1);
            return new RectangleF(referenceRectangle.Right, referenceRectangle.Top, min_spacing_width, referenceRectangle.Height);
        }

        var leftRectangle = spriteText.GetCharacterDrawRectangle(gapIndex - 1);
        var rightRectangle = spriteText.GetCharacterDrawRectangle(gapIndex);
        return new RectangleF(leftRectangle.Right, leftRectangle.Top, rightRectangle.X - leftRectangle.Right, leftRectangle.Y);
    }

    #endregion

    #region Ruby tag

    public RectangleF? GetRubyTagByPosition(RubyTag rubyTag) =>
        spriteText.GetRubyTagPosition(rubyTag);

    #endregion

    #region Time tag

    public TimeTag? GetTimeTagByPosition(Vector2 position)
    {
        var hoverIndex = getHoverIndex();
        if (hoverIndex == null)
            return null;

        // todo: will use better way to get the time-tag
        return HitObject.TimeTags.FirstOrDefault(x => x.Index == hoverIndex);

        TextIndex? getHoverIndex()
        {
            for (int i = 0; i < Text.Length; i++)
            {
                foreach (var indexState in Enum.GetValues<TextIndex.IndexState>())
                {
                    var textIndex = new TextIndex(i, indexState);
                    var triggerRange = getTriggerRange(textIndex);
                    if (triggerRange.Contains(position))
                        return textIndex;
                }
            }

            // hover the last time-tag if exceed the range.
            return null;

            RectangleF getTriggerRange(TextIndex textIndex)
            {
                var rect = spriteText.GetCharacterDrawRectangle(textIndex.Index);
                return TextIndexUtils.GetValueByState(textIndex,
                    () => new RectangleF(rect.Left, rect.Top, rect.Width / 2, rect.Height),
                    () => new RectangleF(rect.Centre.X, rect.Top, rect.Width / 2, rect.Height));
            }
        }
    }

    public Vector2 GetPositionByTimeTag(TimeTag timeTag)
    {
        var basePosition = spriteText.GetTimeTagPosition(timeTag.Index);
        float extraPosition = extraSpacing(HitObject.TimeTags, timeTag);
        return basePosition + new Vector2(extraPosition, 0);

        static float extraSpacing(IList<TimeTag> timeTagsInLyric, TimeTag timeTag)
        {
            var textIndex = timeTag.Index;
            var timeTags = TextIndexUtils.GetValueByState(textIndex, timeTagsInLyric.Reverse, () => timeTagsInLyric);
            int duplicatedTagAmount = timeTags.SkipWhile(t => t != timeTag).Count(x => x.Index == textIndex) - 1;
            int spacing = duplicatedTagAmount * time_tag_spacing * TextIndexUtils.GetValueByState(textIndex, 1, -1);
            return spacing;
        }
    }

    #endregion

    [BackgroundDependencyLoader]
    private void load(ISkinSource skin, ShaderManager? shaderManager)
    {
        skin.GetConfig<Lyric, LyricStyle>(HitObject)?.BindValueChanged(lyricStyle =>
        {
            var newStyle = lyricStyle.NewValue;
            if (newStyle == null)
                return;

            LeftLyricTextShaders = SkinConverterTool.ConvertLeftSideShader(shaderManager, newStyle);
            RightLyricTextShaders = SkinConverterTool.ConvertRightSideShader(shaderManager, newStyle);

            triggerSizeChangedEvent();
        }, true);

        skin.GetConfig<Lyric, LyricFontInfo>(HitObject)?.BindValueChanged(e =>
        {
            var newConfig = e.NewValue;
            if (newConfig == null)
                return;

            // Apply text font info
            var lyricFont = newConfig.MainTextFont;
            var rubyFont = newConfig.RubyTextFont;
            var romanisationTextFont = newConfig.RomanisationTextFont;

            Font = getFont(lyricFont.Size);
            TopTextFont = getFont(rubyFont.Size);
            BottomTextFont = getFont(romanisationTextFont.Size);

            triggerSizeChangedEvent();

            static FontUsage getFont(float? charSize = null)
                => FontUsage.Default.With(size: charSize * 2);
        }, true);
    }

    public override bool RemoveCompletedTransforms => false;

    public partial class EditorLyricSpriteText : LyricSpriteText
    {
        public RectangleF? GetRubyTagPosition(RubyTag rubyTag)
            => GetTopPositionTextDrawRectangle(RubyTagUtils.ToPositionText(rubyTag));

        public Vector2 GetTimeTagPosition(TextIndex index)
        {
            var drawRectangle = GetCharacterDrawRectangle(index.Index);
            return TextIndexUtils.GetValueByState(index, drawRectangle.BottomLeft, drawRectangle.BottomRight);
        }

        // todo: get romanisation position.
    }
}
