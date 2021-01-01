// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Infos.SubInfo
{
    public class LayoutInfo : SubInfo
    {
        public LayoutInfo(Lyric lyric)
            : base(lyric)
        {
            lyric.LayoutIndexBindable.BindValueChanged(value =>
            {
                var newLayoutIndex = value.NewValue;
                BadgeText = $"Layout : {newLayoutIndex}";
            }, true);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            BadgeColour = colours.Pink;
        }
    }
}
