// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Tests;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    public class KaraokeTestBrowser : OsuTestBrowser
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            // note: not add resource store here unless there's no other better choice.
            // because it will let error related to missing resource harder to be tracked.
        }
    }
}
