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

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics;

public partial class PreviewKaraokeSpriteText : DrawableKaraokeSpriteText<PreviewKaraokeSpriteText.EditorLyricSpriteText>, IPreviewLyricPositionProvider
{
    private const int time_tag_spacing = 8;

    public Lyric HitObject;

    public Action? SizeChanged;

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
            SizeChanged?.Invoke();
        });
    }

    #region Text char index

    public int? GetCharIndexByPosition(float position)
    {
        for (int i = 0; i < Text.Length; i++)
        {
            (double startX, double endX) = getTriggerPositionByTimeIndex(i);
            if (position >= startX && position <= endX)
                return i;
        }

        return null;

        Tuple<double, double> getTriggerPositionByTimeIndex(int charIndex)
        {
            var rectangle = spriteText.GetCharacterDrawRectangle(charIndex);
            return new Tuple<double, double>(rectangle.Left, rectangle.Right);
        }
    }

    public RectangleF GetRectByCharIndex(int charIndex)
    {
        if (charIndex < 0 || charIndex >= Text.Length)
            throw new ArgumentOutOfRangeException(nameof(charIndex));

        return spriteText.GetCharacterDrawRectangle(charIndex);
    }

    #endregion

    #region Text indicator

    public int GetCharIndicatorByPosition(float position)
    {
        for (int i = 0; i < Text.Length; i++)
        {
            float textCenterPosition = getTriggerPositionByTimeIndex(i);
            if (position < textCenterPosition)
                return i;
        }

        return Text.Length;

        float getTriggerPositionByTimeIndex(int charIndex)
        {
            var rectangle = spriteText.GetCharacterDrawRectangle(charIndex);
            return rectangle.Centre.X;
        }
    }

    public RectangleF GetRectByCharIndicator(int charIndex)
    {
        if (charIndex < 0 || charIndex > Text.Length)
            throw new ArgumentOutOfRangeException(nameof(charIndex));

        const float min_spacing_width = 1;

        if (charIndex == 0)
        {
            var referenceRectangle = spriteText.GetCharacterDrawRectangle(charIndex);
            return new RectangleF(referenceRectangle.X - min_spacing_width, referenceRectangle.Y, min_spacing_width, referenceRectangle.Height);
        }

        if (charIndex == Text.Length)
        {
            var referenceRectangle = spriteText.GetCharacterDrawRectangle(charIndex - 1);
            return new RectangleF(referenceRectangle.Right, referenceRectangle.Top, min_spacing_width, referenceRectangle.Height);
        }

        var leftRectangle = spriteText.GetCharacterDrawRectangle(charIndex - 1);
        var rightRectangle = spriteText.GetCharacterDrawRectangle(charIndex);
        return new RectangleF(leftRectangle.Right, leftRectangle.Top, rightRectangle.X - leftRectangle.Right, leftRectangle.Y);
    }

    #endregion

    #region Ruby tag

    public RectangleF? GetRubyTagByPosition(RubyTag rubyTag) =>
        spriteText.GetRubyTagPosition(rubyTag);

    #endregion

    #region Time tag

    public TimeTag? GetTimeTagByPosition(float position)
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
                    if (position >= triggerRange.Item1 && position <= triggerRange.Item2)
                        return textIndex;
                }
            }

            // hover the last time-tag if exceed the range.
            return null;

            Tuple<float, float> getTriggerRange(TextIndex textIndex)
            {
                var rect = spriteText.GetCharacterDrawRectangle(textIndex.Index);
                return TextIndexUtils.GetValueByState(textIndex,
                    () => new Tuple<float, float>(rect.Left, rect.Centre.X),
                    () => new Tuple<float, float>(rect.Centre.X, rect.Right));
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
            var romajiFont = newConfig.RomanisationTextFont;

            Font = getFont(lyricFont.Size);
            TopTextFont = getFont(rubyFont.Size);
            BottomTextFont = getFont(romajiFont.Size);

            triggerSizeChangedEvent();

            static FontUsage getFont(float? charSize = null)
                => FontUsage.Default.With(size: charSize * 2);
        }, true);
    }

    public override bool RemoveCompletedTransforms => false;

    public partial class EditorLyricSpriteText : LyricSpriteText
    {
        public RectangleF? GetRubyTagPosition(RubyTag rubyTag)
            => GetTopPositionTextDrawRectangle(TextTagUtils.ToPositionText(rubyTag));

        public Vector2 GetTimeTagPosition(TextIndex index)
        {
            var drawRectangle = GetCharacterDrawRectangle(index.Index);
            return TextIndexUtils.GetValueByState(index, drawRectangle.BottomLeft, drawRectangle.BottomRight);
        }

        // todo: get romanization position.
    }
}
