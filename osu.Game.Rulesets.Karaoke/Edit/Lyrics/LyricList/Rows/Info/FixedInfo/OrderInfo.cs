// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows.Info.FixedInfo
{
    public class OrderInfo : OsuSpriteText
    {
        private readonly IBindable<int> bindableOrder;

        public OrderInfo(Lyric lyric)
        {
            bindableOrder = lyric.OrderBindable.GetBoundCopy();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            bindableOrder.BindValueChanged(value =>
            {
                int order = value.NewValue;
                Text = $"#{order}";
            }, true);
        }
    }
}
