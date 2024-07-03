// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.List;

public partial class ListContent : MainContent
{
    public ListContent()
    {
        InternalChildren = new[]
        {
            new PreviewLyricList
            {
                RelativeSizeAxes = Axes.Both,
            },
        };
    }
}
