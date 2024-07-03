// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose;

public partial class ComposeContent : MainContent
{
    public ComposeContent()
    {
        InternalChildren = new Drawable[]
        {
            new LyricComposer
            {
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1, 0.6f),
            },
            new DetailLyricList
            {
                RelativePositionAxes = Axes.Y,
                Position = new Vector2(0, 0.6f),
                Size = new Vector2(1, 0.4f),
                RelativeSizeAxes = Axes.Both,
            },
        };
    }
}
