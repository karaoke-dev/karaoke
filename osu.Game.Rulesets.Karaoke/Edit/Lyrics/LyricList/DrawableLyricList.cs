// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList
{
    public abstract class DrawableLyricList : OrderRearrangeableListContainer<Lyric>
    {
        private readonly IBindable<ICaretPosition?> bindableCaretPosition = new Bindable<ICaretPosition?>();

        protected DrawableLyricList()
        {
            // update selected style to child
            bindableCaretPosition.BindValueChanged(e =>
            {
                var oldLyric = e.OldValue?.Lyric;
                var newLyric = e.NewValue?.Lyric;
                if (newLyric == null || !ValueChangedEventUtils.LyricChanged(e))
                    return;

                if (!ScrollToPosition(e.NewValue!))
                    return;

                int skippingRows = SkipRows();
                moveItemToTargetPosition(newLyric, oldLyric, skippingRows);
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

        private bool moveItemToTargetPosition(Lyric newLyric, Lyric? oldLyric, int skippingRows)
        {
            var oldItem = getListItem(oldLyric);
            var newItem = getListItem(newLyric);

            // new item might been deleted.
            if (newItem == null)
                return false;

            float spacing = newItem.Height * skippingRows;

            // do not scroll if position is smaller then spacing.
            float scrollPosition = ScrollContainer.GetChildPosInContent(newItem);
            if (scrollPosition < spacing)
                return false;

            // do not scroll if position is too large and not able to move to target position.
            float contentHeight = ScrollContainer.ScrollContent.Height;
            float containerHeight = ScrollContainer.DrawHeight;
            if (contentHeight - scrollPosition < containerHeight - spacing)
                return false;

            ScrollContainer.ScrollTo(scrollPosition - spacing);
            return true;

            DrawableLyricListItem? getListItem(Lyric? lyric)
                => ListContainer.Children.FirstOrDefault(x => x.Model == lyric) as DrawableLyricListItem;
        }
    }
}
