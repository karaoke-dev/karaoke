// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Overlays.Changelog
{
    /// <summary>
    /// Display list of <see cref="KaraokeChangelogBuild"/>
    /// </summary>
    public class ChangelogListing : ChangelogContent
    {
        private readonly List<KaraokeChangelogBuild> entries;

        public ChangelogListing(List<KaraokeChangelogBuild> entries)
        {
            this.entries = entries;
        }

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider)
        {
            if (entries == null)
                return;

            foreach (var build in entries)
            {
                Add(new ChangelogBuild(build) { SelectBuild = SelectBuild });
            }
        }
    }
}
