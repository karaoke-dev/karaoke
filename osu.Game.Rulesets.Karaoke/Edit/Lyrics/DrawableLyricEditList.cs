// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class DrawableLyricEditList : OrderRearrangeableListContainer<Lyric>
    {
        protected override Vector2 Spacing => new Vector2(0, 2);

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
                    Child = new CreateNewLyricRow
                    {
                        RelativeSizeAxes = Axes.Both,
                    }
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(LyricEditorStateManager stateManager)
        {
            // update hover style to child
            stateManager.BindableHoverCursorPosition.BindValueChanged(e =>
            {
                var listItem = getListItem(e.NewValue.Lyric);
            });

            // update selected style to child
            stateManager.BindableCursorPosition.BindValueChanged(e =>
            {
                var listItem = getListItem(e.NewValue.Lyric);

                // move to target position.
                if (stateManager.BindableAutoFocusEditLyric.Value)
                {
                    var skippingRows = stateManager.BindableAutoFocusEditLyricSkipRows.Value;
                    moveItemToTargetPosition(listItem, listItem.Height * skippingRows);
                }
            });

            DrawableLyricEditListItem getListItem(Lyric lyric)
                => ListContainer.Children.FirstOrDefault(x => x.Model == lyric) as DrawableLyricEditListItem;
        }

        private bool moveItemToTargetPosition(DrawableLyricEditListItem item, float spacing)
        {
            // do not scroll if position is smaller then spacing.
            var scrollPosition = ScrollContainer.GetChildPosInContent(item);
            if (scrollPosition < spacing)
                return false;

            // do not scroll if posiiton is too large and not able to move to target position.
            var itemHeight = item.Height;
            var contentHeight = ScrollContainer.ScrollContent.Height;
            var containerHeight = ScrollContainer.DrawHeight;
            if (contentHeight - scrollPosition + itemHeight < containerHeight - spacing)
                return false;

            ScrollContainer.ScrollTo(scrollPosition - spacing);
            return true;
        }
    }
}
