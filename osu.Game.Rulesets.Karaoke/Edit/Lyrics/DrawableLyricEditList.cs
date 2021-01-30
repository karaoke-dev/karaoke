// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
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
    }
}
