// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyric.Components.Badges
{
    public class StyleInfoBadge : Badge
    {
        public StyleInfoBadge(Objects.Lyric lyric)
            : base(lyric)
        {
            lyric.SingersBindable.BindValueChanged(value =>
            {
                var newStyleIndex = value.NewValue;
                BadgeText = $"Singer : {newStyleIndex}";
            }, true);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            BadgeColour = colours.Green;
        }
    }
}
