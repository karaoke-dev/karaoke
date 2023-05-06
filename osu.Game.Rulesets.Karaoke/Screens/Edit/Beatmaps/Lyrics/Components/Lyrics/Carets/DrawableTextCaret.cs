// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics.Primitives;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics.Carets;

public abstract partial class DrawableTextCaret<TCaretPosition> : DrawableCaret<TCaretPosition> where TCaretPosition : struct, ITextCaretPosition
{
    [Resolved]
    private IPreviewLyricPositionProvider previewLyricPositionProvider { get; set; }

    protected DrawableTextCaret(DrawableCaretType type)
        : base(type)
    {
    }

    protected RectangleF GetRect(TCaretPosition caret)
    {
        return previewLyricPositionProvider.GetRectByCharIndicator(caret.Index);
    }
}
