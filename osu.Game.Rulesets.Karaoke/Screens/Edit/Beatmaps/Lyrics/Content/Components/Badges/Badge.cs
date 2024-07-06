// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Badges;

public abstract partial class Badge : CompositeDrawable
{
    private readonly Box background;
    private readonly OsuSpriteText text;

    protected Lyric Lyric { get; }

    [Resolved]
    private ILyricCaretState lyricCaretState { get; set; } = null!;

    protected Badge(Lyric lyric)
    {
        Lyric = lyric;

        AutoSizeAxes = Axes.Both;
        Masking = true;
        CornerRadius = 3;
        InternalChildren = new Drawable[]
        {
            background = new Box
            {
                RelativeSizeAxes = Axes.Both,
            },
            text = new OsuSpriteText
            {
                Margin = new MarginPadding
                {
                    Vertical = 2,
                    Horizontal = 5,
                },
                Text = "Badge",
            },
        };
    }

    protected LocalisableString Text
    {
        get => text.Text;
        set => text.Text = value;
    }

    protected ColourInfo BackgroundColour
    {
        get => background.Colour;
        set => background.Colour = value;
    }

    protected override bool OnClick(ClickEvent e)
    {
        lyricCaretState.MoveCaretToTargetPosition(Lyric);
        return base.OnClick(e);
    }
}
