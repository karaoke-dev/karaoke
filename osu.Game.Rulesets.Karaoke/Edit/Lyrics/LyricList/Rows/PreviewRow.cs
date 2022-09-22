// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows
{
    public abstract class PreviewRow : Row
    {
        private const int info_part_spacing = 210;
        private const int min_height = 75;
        private const int max_height = 120;

        protected PreviewRow(Lyric lyric)
            : base(lyric)
        {
        }

        protected override IEnumerable<Dimension> GetColumnDimensions() =>
            new[]
            {
                new Dimension(GridSizeMode.Absolute, info_part_spacing),
                new Dimension()
            };

        protected override Dimension GetRowDimensions() => new(GridSizeMode.AutoSize, minSize: min_height, maxSize: max_height);

        protected override IEnumerable<Drawable> GetDrawables(Lyric lyric) =>
            new[]
            {
                CreateLyricInfo(lyric),
                CreateContent(lyric)
            };

        protected abstract Drawable CreateLyricInfo(Lyric lyric);

        protected abstract Drawable CreateContent(Lyric lyric);
    }
}
