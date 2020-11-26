// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Framework.Timing;
using osu.Game.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyric.Components.Badges
{
    public class TimeInfoBadge : Badge
    {
        [Resolved]
        private IAdjustableClock adjustableClock { get; set; }

        public TimeInfoBadge(Objects.Lyric lyric)
            : base(lyric)
        {
            lyric.StartTimeBindable.BindValueChanged(value => { changeTime(); }, true);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            BadgeColour = colours.Gray7;
        }

        protected override bool OnClick(ClickEvent e)
        {
            adjustableClock.Seek(Lyric.StartTime);
            return base.OnClick(e);
        }

        private void changeTime()
        {
            BadgeText = $"{getTime(Lyric.StartTime)} - {getTime(Lyric.EndTime)}";

            static string getTime(double time) => TimeSpan.FromMilliseconds(time).ToString(@"mm\:ss\:fff");
        }
    }
}
