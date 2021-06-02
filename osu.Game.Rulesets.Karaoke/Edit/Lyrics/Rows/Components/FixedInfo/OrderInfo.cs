// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.FixedInfo
{
    public class OrderInfo : OsuSpriteText
    {
        public OrderInfo(Lyric lyric)
        {
            lyric.OrderBindable.BindValueChanged(value =>
            {
                var order = value.NewValue;
                Text = $"#{order}";
            }, true);
        }
    }
}
