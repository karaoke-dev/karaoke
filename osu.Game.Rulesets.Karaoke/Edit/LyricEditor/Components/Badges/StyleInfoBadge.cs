// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.LyricEditor.Components.Badges
{
    public class StyleInfoBadge : Badge
    {
        public StyleInfoBadge(Lyric lyric)
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
