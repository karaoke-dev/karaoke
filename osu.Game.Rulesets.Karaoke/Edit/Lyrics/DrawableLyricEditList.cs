// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
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
        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
        private readonly IBindable<ICaretPosition> bindableCaretPosition = new Bindable<ICaretPosition>();
        private readonly IBindable<bool> bindableAutoFocusToEditLyric = new Bindable<bool>();
        private readonly IBindable<int> bindableAutoFocusToEditLyricSkipRows = new Bindable<int>();

        public DrawableLyricEditList()
        {
            // update selected style to child
            bindableCaretPosition.BindValueChanged(e =>
            {
                var oldLyric = e.OldValue?.Lyric;
                var newLyric = e.NewValue?.Lyric;
                if (newLyric == null)
                    return;

                // should not move the position if caret is only support clicking.
                if (bindableCaretPosition.Value is ClickingCaretPosition)
                    return;

                // should not move the position in manage lyric mode.
                if (bindableMode.Value == LyricEditorMode.Manage)
                    return;

                // move to target position if auto focus.
                bool autoFocus = bindableAutoFocusToEditLyric.Value;
                if (!autoFocus)
                    return;

                int skippingRows = bindableAutoFocusToEditLyricSkipRows.Value;
                moveItemToTargetPosition(newLyric, oldLyric, skippingRows);
            });
        }

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
        private void load(ILyricEditorState state, ILyricCaretState lyricCaretState, KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
        {
            bindableMode.BindTo(state.BindableMode);
            bindableCaretPosition.BindTo(lyricCaretState.BindableCaretPosition);

            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.AutoFocusToEditLyric, bindableAutoFocusToEditLyric);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.AutoFocusToEditLyricSkipRows, bindableAutoFocusToEditLyricSkipRows);
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

            DrawableLyricEditListItem getListItem(Lyric lyric)
                => ListContainer.Children.FirstOrDefault(x => x.Model == lyric) as DrawableLyricEditListItem;

            float getOffsetPosition(DrawableLyricEditListItem newItem, DrawableLyricEditListItem oldItem)
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
