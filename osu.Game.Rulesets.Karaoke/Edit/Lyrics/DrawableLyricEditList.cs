// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class DrawableLyricEditList : OrderRearrangeableListContainer<Lyric>
    {
        protected override Vector2 Spacing => new(0, 2);

        protected override OsuRearrangeableListItem<Lyric> CreateOsuDrawable(Lyric item)
            => new DrawableLyricEditListItem(item);

        protected override Drawable CreateBottomDrawable()
        {
            return new Container
            {
                RelativeSizeAxes = Axes.X,
                Height = 75,
                Padding = new MarginPadding { Left = 22 },
                Child = new Container
                {
                    Masking = true,
                    CornerRadius = 5,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Alpha = 0.5f,
                            Colour = Color4.Black
                        },
                        new CreateNewLyricRow
                        {
                            RelativeSizeAxes = Axes.Both,
                        }
                    }
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager, LyricCaretState lyricCaretState)
        {
            // update selected style to child
            lyricCaretState.BindableCaretPosition.BindValueChanged(e =>
            {
                var oldLyric = e.OldValue?.Lyric;
                var newLyric = e.NewValue?.Lyric;
                if (newLyric == null)
                    return;

                // move to target position if auto focus.
                var autoFocus = lyricEditorConfigManager.Get<bool>(KaraokeRulesetLyricEditorSetting.AutoFocusToEditLyric);
                if (!autoFocus)
                    return;

                var skippingRows = lyricEditorConfigManager.Get<int>(KaraokeRulesetLyricEditorSetting.AutoFocusToEditLyricSkipRows);
                moveItemToTargetPosition(newLyric, oldLyric, skippingRows);
            });
        }

        private bool moveItemToTargetPosition(Lyric newLyric, Lyric oldLyric, int skippingRows)
        {
            var oldItem = getListItem(oldLyric);
            var newItem = getListItem(newLyric);

            // new item might been deleted.
            if (newItem == null)
                return false;

            var spacing = newItem.Height * skippingRows;

            // do not scroll if position is smaller then spacing.
            var scrollPosition = ScrollContainer.GetChildPosInContent(newItem);
            if (scrollPosition < spacing)
                return false;

            // do not scroll if position is too large and not able to move to target position.
            var itemHeight = newItem.Height + newItem.ExtendHeight;
            var contentHeight = ScrollContainer.ScrollContent.Height;
            var containerHeight = ScrollContainer.DrawHeight;
            if (contentHeight - scrollPosition + itemHeight < containerHeight - spacing)
                return false;

            ScrollContainer.ScrollTo(scrollPosition - spacing + getOffsetPosition(newItem, oldItem));
            return true;

            DrawableLyricEditListItem getListItem(Lyric lyric)
                => ListContainer.Children.FirstOrDefault(x => x.Model == lyric) as DrawableLyricEditListItem;

            float getOffsetPosition(DrawableLyricEditListItem newItem, DrawableLyricEditListItem oldItem)
            {
                if (oldItem == null)
                    return 0;

                var newItemPosition = ScrollContainer.GetChildPosInContent(newItem);
                var oldItemPosition = ScrollContainer.GetChildPosInContent(oldItem);
                if (oldItemPosition > scrollPosition)
                    return 0;

                // if previous lyric is in front of current lyric row, due to extend in previous row has been removed.
                // it will cause offset from previous row extend.
                return -newItem.ExtendHeight;
            }
        }
    }
}
