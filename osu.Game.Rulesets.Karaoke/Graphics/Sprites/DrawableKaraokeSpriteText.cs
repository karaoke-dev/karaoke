// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Tools;

namespace osu.Game.Rulesets.Karaoke.Graphics.Sprites;

public partial class DrawableKaraokeSpriteText : DrawableKaraokeSpriteText<LyricSpriteText>
{
    public DrawableKaraokeSpriteText(Lyric lyric)
        : base(lyric)
    {
    }
}

public abstract partial class DrawableKaraokeSpriteText<TSpriteText> : KaraokeSpriteText<TSpriteText> where TSpriteText : LyricSpriteText, new()
{
    private readonly DisplayLyricProcessor processor;

    [Resolved]
    private ShaderManager? shaderManager { get; set; }

    protected DrawableKaraokeSpriteText(Lyric lyric)
    {
        processor = new DisplayLyricProcessor(lyric)
        {
            TopTextChanged = topTexts =>
            {
                TopTexts = topTexts;
                OnPropertyChanged();
            },
            CenterTextChanged = text =>
            {
                Text = text;
                OnPropertyChanged();
            },
            BottomTextChanged = bottomTexts =>
            {
                BottomTexts = bottomTexts;
                OnPropertyChanged();
            },
            TimeTagsChanged = timeTags =>
            {
                TimeTags = timeTags;
                OnPropertyChanged();
            },
        };
        processor.UpdateAll();
    }

    public LyricDisplayType DisplayType
    {
        get => processor.DisplayType;
        set => processor.DisplayType = value;
    }

    public LyricDisplayProperty DisplayProperty
    {
        get => processor.DisplayProperty;
        set => processor.DisplayProperty = value;
    }

    // not a good practice but child class need to know the property changed.
    protected virtual void OnPropertyChanged()
    {
    }

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);
        processor.Dispose();
    }

    public void UpdateStyle(LyricStyle style)
    {
        // for prevent issue Collection was modified; enumeration operation may not execute.
        Schedule(() =>
        {
            LeftLyricTextShaders = SkinConverterTool.ConvertLeftSideShader(shaderManager, style);
            RightLyricTextShaders = SkinConverterTool.ConvertRightSideShader(shaderManager, style);
        });
    }
}
