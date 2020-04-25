// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using System;

namespace osu.Game.Rulesets.Karaoke.Edit.LyricEditor.Components.Badges
{
    public class TimeInfoBadge : Badge
    {
        public TimeInfoBadge(LyricLine lyric)
            : base(lyric)
        {
            lyric.StartTimeBindable.BindValueChanged(value =>
            {
                var startTime = value.NewValue;
                ChangeTime();
            }, true);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            BadgeColour = colours.Gray7;
        }

        private void ChangeTime()
        {
            BadgeText = $"{getTime(Lyric.StartTime)} - {getTime(Lyric.EndTime)}";

            string getTime(double time) => TimeSpan.FromMilliseconds(time).ToString(@"mm\:ss\:fff"); ;
        }
    }
}
