// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose;

public partial class DrawableDetailLyricListItem : DrawableLyricListItem
{
    public DrawableDetailLyricListItem(Lyric item)
        : base(item)
    {
    }

    protected override Row CreateEditRow(Lyric lyric)
        => new EditLyricDetailRow(lyric);
}
