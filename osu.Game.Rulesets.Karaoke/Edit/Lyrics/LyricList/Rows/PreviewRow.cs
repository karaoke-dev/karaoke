// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows
{
    public abstract class PreviewRow : Row
    {
        protected const int DEFAULT_HEIGHT = 75;

        private const int info_part_spacing = 210;
        private const int min_height = DEFAULT_HEIGHT;
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

        protected override bool HighlightBackgroundWhenSelected(ICaretPosition caretPosition)
        {
            // should not show the background in the assign language mode.
            if (caretPosition is ClickingCaretPosition)
                return false;

            return true;
        }

        protected override Func<LyricEditorMode, Color4> GetBackgroundColour(BackgroundStyle style, LyricEditorColourProvider colourProvider) =>
            style switch
            {
                BackgroundStyle.Idle => colourProvider.Background5,
                BackgroundStyle.Hover => colourProvider.Background4,
                BackgroundStyle.Focus => colourProvider.Background3,
                _ => throw new ArgumentOutOfRangeException(nameof(style), style, null)
            };

        protected abstract Drawable CreateLyricInfo(Lyric lyric);

        protected abstract Drawable CreateContent(Lyric lyric);
    }
}
