// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.IO.Stores;
using osu.Game.Tests;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    public class KaraokeTestBrowser : OsuTestBrowser
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            // add shader resource from font package.
            Resources.AddStore(new NamespacedResourceStore<byte[]>(new ShaderResourceStore(), "Resources"));
        }
    }
}
