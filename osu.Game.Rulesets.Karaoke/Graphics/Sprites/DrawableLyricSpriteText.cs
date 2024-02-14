// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Graphics.Sprites;

public partial class DrawableLyricSpriteText : LyricSpriteText
{
    private readonly DisplayLyricProcessor processor;

    public DrawableLyricSpriteText(Lyric lyric)
    {
        processor = new DisplayLyricProcessor(lyric);
        processor.TopTextChanged = rubies =>
        {
            Rubies = rubies;
        };
        processor.CenterTextChanged = text =>
        {
            Text = text;
        };
        processor.BottomTextChanged = romajies =>
        {
            Romajies = romajies;
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

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);
        processor.Dispose();
    }
}
