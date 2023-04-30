// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.LyricList.Rows.Info;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.LyricList.Rows;

public partial class EditLyricPreviewRow : PreviewRow
{
    private const int min_height = 75;

    public EditLyricPreviewRow(Lyric lyric)
        : base(lyric)
    {
    }

    protected override Drawable CreateLyricInfo(Lyric lyric)
    {
        return new InfoControl(lyric)
        {
            // todo : cannot use relative size to both because it will cause size cannot roll-back if make lyric smaller.
            RelativeSizeAxes = Axes.X,
            Height = min_height,
        };
    }

    protected override Drawable CreateContent(Lyric lyric)
    {
        return new EditableLyric(lyric)
        {
            Margin = new MarginPadding { Left = 10 },
            RelativeSizeAxes = Axes.X,
        };
    }
}
