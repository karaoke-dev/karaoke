// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework;
using osu.Framework.Platform;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    public static class VisualTestRunner
    {
        [STAThread]
        public static int Main(string[] args)
        {
            using (DesktopGameHost host = Host.GetSuitableHost(@"karaoke-visual-test-runner", true))
            {
                host.Run(new KaraokeTestBrowser());
                return 0;
            }
        }
    }
}
