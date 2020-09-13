// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Timing;
using osu.Game.Input.Bindings;
using osu.Game.Rulesets.Configuration;
using osu.Game.Rulesets.Karaoke.UI.HUD;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class SettingHUDOverlay : Container
    {
        public readonly ControlLayer controlLayer;

        private readonly DrawableKaraokeRuleset drawableRuleset;

        public SettingHUDOverlay(DrawableKaraokeRuleset drawableRuleset)
        {
            this.drawableRuleset = drawableRuleset;

            RelativeSizeAxes = Axes.Both;

            Children = new Drawable[]
            {
                new KaraokeActionContainer(drawableRuleset)
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = controlLayer = CreateControlLayer()
                }
            };
        }

        protected virtual ControlLayer CreateControlLayer() => new ControlLayer(drawableRuleset.Beatmap)
        {
            Clock = new FramedClock(new StopwatchClock(true)),
            RelativeSizeAxes = Axes.Both
        };

        public class KaraokeActionContainer : DatabasedKeyBindingContainer<KaraokeAction>
        {
            private readonly DrawableKaraokeRuleset drawableRuleset;

            protected IRulesetConfigManager Config;

            public KaraokeActionContainer(DrawableKaraokeRuleset drawableRuleset)
                : base(drawableRuleset.Ruleset.RulesetInfo, 0, SimultaneousBindingMode.Unique)
            {
                this.drawableRuleset = drawableRuleset;
            }

            protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            {
                var dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
                dependencies.Cache(drawableRuleset.Config);
                dependencies.Cache(drawableRuleset.Session);
                return dependencies;
            }
        }
    }
}
