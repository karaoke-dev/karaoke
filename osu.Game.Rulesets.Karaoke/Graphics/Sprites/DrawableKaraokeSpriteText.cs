// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

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

    protected DrawableKaraokeSpriteText(Lyric lyric)
    {
        processor = new DisplayLyricProcessor(lyric);
        processor.TopTextChanged = topTexts =>
        {
            TopTexts = topTexts;
            OnPropertyChanged();
        };
        processor.CenterTextChanged = text =>
        {
            Text = text;
            OnPropertyChanged();
        };
        processor.BottomTextChanged = bottomTexts =>
        {
            BottomTexts = bottomTexts;
            OnPropertyChanged();
        };
        processor.TimeTagsChanged = timeTags =>
        {
            TimeTags = timeTags;
            OnPropertyChanged();
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
}
