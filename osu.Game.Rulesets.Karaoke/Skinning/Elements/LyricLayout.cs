// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Skinning.Elements;

public class LyricLayout : IKaraokeSkinElement
{
    public int ID { get; set; }

    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Group
    /// </summary>
    public int Group { get; set; }

    /// <summary>
    /// Lyric alignment
    /// </summary>
    public Anchor Alignment { get; set; } = Anchor.Centre;

    /// <summary>
    /// Horizontal margin
    /// </summary>
    public int HorizontalMargin { get; set; }

    /// <summary>
    /// Vertical margin
    /// </summary>
    public int VerticalMargin { get; set; }

    public void ApplyTo(Drawable d)
    {
        if (d is not DrawableLyric drawableLyric)
            throw new InvalidDrawableTypeException(nameof(d));

        // Layout relative to parent
        drawableLyric.Anchor = Alignment;
        drawableLyric.Origin = Alignment;
        drawableLyric.Margin = new MarginPadding
        {
            Left = Alignment.HasFlag(Anchor.x0) ? HorizontalMargin : 0,
            Right = Alignment.HasFlag(Anchor.x2) ? HorizontalMargin : 0,
            Top = Alignment.HasFlag(Anchor.y0) ? VerticalMargin : 0,
            Bottom = Alignment.HasFlag(Anchor.y2) ? VerticalMargin : 0,
        };
    }
}
