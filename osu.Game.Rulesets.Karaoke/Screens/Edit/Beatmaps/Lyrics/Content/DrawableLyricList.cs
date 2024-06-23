// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content;

/// <summary>
/// Visualises a list of <see cref="Lyric"/>s.
/// </summary>
public abstract partial class DrawableLyricList : OrderRearrangeableListContainer<Lyric>
{
    private readonly IBindable<ICaretPosition?> bindableCaretPosition = new Bindable<ICaretPosition?>();

    protected DrawableLyricList()
    {
        // update selected style to child
        bindableCaretPosition.BindValueChanged(e =>
        {
            var newLyric = e.NewValue?.Lyric;
            if (newLyric == null || !ValueChangedEventUtils.LyricChanged(e))
                return;

            if (!ScrollToPosition(e.NewValue!))
                return;

            int skippingRows = SkipRows();
            moveItemToTargetPosition(newLyric, skippingRows);
        });
    }

    protected abstract bool ScrollToPosition(ICaretPosition caret);

    protected abstract int SkipRows();

    protected abstract DrawableLyricListItem CreateLyricListItem(Lyric item);

    protected sealed override OsuRearrangeableListItem<Lyric> CreateOsuDrawable(Lyric item)
        => CreateLyricListItem(item);

    protected sealed override Drawable CreateBottomDrawable()
    {
        return new Container
        {
            // todo: should based on the row's height.
            RelativeSizeAxes = Axes.X,
            Height = 75,
            Padding = new MarginPadding { Left = DrawableLyricListItem.HANDLER_WIDTH },
            Child = GetCreateNewLyricRow(),
        };
    }

    protected abstract Row GetCreateNewLyricRow();

    [BackgroundDependencyLoader]
    private void load(ILyricCaretState lyricCaretState)
    {
        bindableCaretPosition.BindTo(lyricCaretState.BindableCaretPosition);
    }

    private void moveItemToTargetPosition(Lyric targetLyric, int skippingRows)
    {
        var drawable = getListItem(targetLyric);
        if (drawable == null)
            return;

        float topSpacing = drawable.Height * skippingRows;
        float bottomSpacing = DrawHeight - drawable.Height * (skippingRows + 1);
        ScrollContainer.ScrollIntoViewWithSpacing(drawable, new MarginPadding
        {
            Top = topSpacing,
            Bottom = bottomSpacing,
        });
        return;

        DrawableLyricListItem? getListItem(Lyric? lyric)
            => ListContainer.Children.OfType<DrawableLyricListItem>().FirstOrDefault(x => x.Model == lyric);
    }
}
