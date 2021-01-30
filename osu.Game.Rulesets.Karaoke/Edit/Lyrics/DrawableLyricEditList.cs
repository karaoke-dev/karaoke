// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Graphics.Containers;
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
    }
}
