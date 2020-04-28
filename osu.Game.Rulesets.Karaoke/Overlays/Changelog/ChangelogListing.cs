// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Overlays.Changelog
{
    public class ChangelogListing : ChangelogContent
    {
        private readonly List<KaraokeChangelogBuild> entries;

        public ChangelogListing(List<KaraokeChangelogBuild> entries)
        {
            this.entries = entries;
        }
    }
}
