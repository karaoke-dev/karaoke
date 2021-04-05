// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Screens;

namespace osu.Game.Rulesets.Karaoke.Extensions
{
    /// <summary>
    /// Collect dirty logic to get target drawable from <see cref="OsuGame"/>
    /// </summary>
    public static class OsuGameExtensions
    {
        public static Container GetDisplayContainer(this OsuGame game)
            => game?.Children[3] as Container;

        public static OsuScreenStack GetScreenStack(this OsuGame game)
            => (game?.Children[2] as Container)?.Children.OfType<OsuScreenStack>().FirstOrDefault();

        public static SettingsOverlay GetSettingsOverlay(this OsuGame game)
            => (game?.Children[5] as Container)?.Children.OfType<SettingsOverlay>().FirstOrDefault();
    }
}
