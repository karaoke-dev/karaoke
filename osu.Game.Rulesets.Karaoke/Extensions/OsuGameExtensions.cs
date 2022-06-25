// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Testing;
using osu.Game.Overlays;
using osu.Game.Screens;

namespace osu.Game.Rulesets.Karaoke.Extensions
{
    /// <summary>
    /// Collect dirty logic to get target drawable from <see cref="OsuGame"/>
    /// </summary>
    public static class OsuGameExtensions
    {
        public static KaraokeRuleset? GetRuleset(this DependencyContainer dependencies)
        {
            var rulesets = dependencies.Get<RulesetStore>().AvailableRulesets.Select(info => info.CreateInstance());
            return rulesets.FirstOrDefault(r => r is KaraokeRuleset) as KaraokeRuleset;
        }

        private static Container? getBasePlacementContainer(this OsuGame game)
            => game.Children[4] as Container;

        public static Container? GetChangelogPlacementContainer(this OsuGame game)
            => game.getBasePlacementContainer()?.Children[0] as Container;

        public static SettingsOverlay? GetSettingsOverlay(this OsuGame game)
            => game.getBasePlacementContainer()?.ChildrenOfType<SettingsOverlay>().FirstOrDefault();

        public static OsuScreenStack? GetScreenStack(this OsuGame game)
            => ((game.Children[3] as Container)?.Child as Container)?.Children.OfType<OsuScreenStack>().FirstOrDefault();
    }
}
