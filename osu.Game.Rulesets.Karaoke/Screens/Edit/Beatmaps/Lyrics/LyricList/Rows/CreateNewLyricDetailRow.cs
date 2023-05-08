// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.LyricList.Rows;

public partial class CreateNewLyricDetailRow : DetailRow
{
    [Resolved]
    private ILyricsChangeHandler lyricsChangeHandler { get; set; } = null!;

    public CreateNewLyricDetailRow()
        : base(new Lyric { Text = "New lyric" })
    {
    }

    protected override IEnumerable<Dimension> GetColumnDimensions() =>
        new[]
        {
            new Dimension(GridSizeMode.Absolute, TIMING_WIDTH),
            new Dimension()
        };

    protected override Drawable CreateTimingInfo(Lyric lyric)
        => Empty();

    protected override Drawable CreateContent(Lyric lyric)
    {
        return new IconButton
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Icon = FontAwesome.Solid.PlusCircle,
            Size = new Vector2(32),
            TooltipText = "Click to add new lyric",
            Action = () =>
            {
                lyricsChangeHandler.CreateAtLast();
            }
        };
    }
}
