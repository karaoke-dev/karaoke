// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows
{
    public abstract class DetailRow : Row
    {
        public const int TIMIMG_WIDTH = 210;

        protected DetailRow(Lyric lyric)
            : base(lyric)
        {
        }

        protected override IEnumerable<Dimension> GetColumnDimensions() =>
            new[]
            {
                new Dimension(GridSizeMode.Absolute, TIMIMG_WIDTH),
                new Dimension()
            };

        protected override Dimension GetRowDimensions()
            => new(GridSizeMode.Absolute, 40);

        protected override IEnumerable<Drawable> GetDrawables(Lyric lyric) =>
            new[]
            {
                CreateTimingInfo(lyric),
                CreateContent(lyric)
            };

        protected override bool HighlightBackgroundWhenSelected(ICaretPosition? caretPosition) => true;

        protected override Func<LyricEditorMode, Color4> GetBackgroundColour(BackgroundStyle style, LyricEditorColourProvider colourProvider) =>
            style switch
            {
                BackgroundStyle.Idle => _ => new Color4(), // should not have background if not hover.
                BackgroundStyle.Hover => colourProvider.Background2, // follow the colour in the editor table.
                BackgroundStyle.Focus => colourProvider.Background1, // follow the colour in the editor table.
                _ => throw new ArgumentOutOfRangeException(nameof(style), style, null)
            };

        protected abstract Drawable CreateTimingInfo(Lyric lyric);

        protected abstract Drawable CreateContent(Lyric lyric);
    }
}
