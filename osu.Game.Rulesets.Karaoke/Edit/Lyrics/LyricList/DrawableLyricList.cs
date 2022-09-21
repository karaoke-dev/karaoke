// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList
{
    public abstract class DrawableLyricList : OrderRearrangeableListContainer<Lyric>
    {
        private readonly IBindable<ICaretPosition> bindableCaretPosition = new Bindable<ICaretPosition>();

        protected DrawableLyricList()
        {
            // update selected style to child
            bindableCaretPosition.BindValueChanged(e =>
            {
                var oldLyric = e.OldValue?.Lyric;
                var newLyric = e.NewValue?.Lyric;
                if (newLyric == null)
                    return;

                if (!ScrollToPosition(bindableCaretPosition.Value))
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

        [BackgroundDependencyLoader]
        private void load(ILyricCaretState lyricCaretState)
        {
            bindableCaretPosition.BindTo(lyricCaretState.BindableCaretPosition);
        }

        private bool moveItemToTargetPosition(Lyric newLyric, Lyric oldLyric, int skippingRows)
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
            float itemHeight = newItem.Height + newItem.ExtendHeight;
            float contentHeight = ScrollContainer.ScrollContent.Height;
            float containerHeight = ScrollContainer.DrawHeight;
            if (contentHeight - scrollPosition + itemHeight < containerHeight - spacing)
                return false;

            ScrollContainer.ScrollTo(scrollPosition - spacing + getOffsetPosition(newItem, oldItem));
            return true;

            DrawableLyricListItem getListItem(Lyric lyric)
                => ListContainer.Children.FirstOrDefault(x => x.Model == lyric) as DrawableLyricListItem;

            float getOffsetPosition(DrawableLyricListItem newItem, DrawableLyricListItem oldItem)
            {
                if (oldItem == null)
                    return 0;

                float newItemPosition = ScrollContainer.GetChildPosInContent(newItem);
                float oldItemPosition = ScrollContainer.GetChildPosInContent(oldItem);
                if (oldItemPosition > scrollPosition)
                    return 0;

                // if previous lyric is in front of current lyric row, due to extend in previous row has been removed.
                // it will cause offset from previous row extend.
                return -newItem.ExtendHeight;
            }
        }
    }
}
