// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Info.Badge
{
    public class TimeTagInfo : SubInfo
    {
        private readonly IBindable<int> bindableTimeTagsVersion;
        private readonly IBindableList<TimeTag> bindableTimeTags;

        public TimeTagInfo(Lyric lyric)
            : base(lyric)
        {
            bindableTimeTagsVersion = lyric.TimeTagsVersion.GetBoundCopy();
            bindableTimeTags = lyric.TimeTagsBindable.GetBoundCopy();
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            BadgeColour = colours.Green;

            bindableTimeTagsVersion.BindValueChanged(_ => updateBadgeText());
            bindableTimeTags.BindCollectionChanged((_, _) => updateBadgeText());

            updateBadgeText();

            void updateBadgeText()
                => BadgeText = LyricUtils.TimeTagTimeFormattedString(Lyric);
        }
    }
}
