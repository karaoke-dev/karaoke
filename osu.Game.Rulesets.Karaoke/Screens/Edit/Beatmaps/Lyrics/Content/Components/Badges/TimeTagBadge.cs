// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Badges;

public partial class TimeTagBadge : Badge
{
    private readonly IBindable<int> bindableTimeTagsTimingVersion;
    private readonly IBindableList<TimeTag> bindableTimeTags;

    public TimeTagBadge(Lyric lyric)
        : base(lyric)
    {
        bindableTimeTagsTimingVersion = lyric.TimeTagsTimingVersion.GetBoundCopy();
        bindableTimeTags = lyric.TimeTagsBindable.GetBoundCopy();
    }

    [BackgroundDependencyLoader]
    private void load(OsuColour colours)
    {
        BadgeColour = colours.Green;

        bindableTimeTagsTimingVersion.BindValueChanged(_ => updateBadgeText());
        bindableTimeTags.BindCollectionChanged((_, _) => updateBadgeText());

        updateBadgeText();

        void updateBadgeText()
            => BadgeText = LyricUtils.TimeTagTimeFormattedString(Lyric);
    }
}
