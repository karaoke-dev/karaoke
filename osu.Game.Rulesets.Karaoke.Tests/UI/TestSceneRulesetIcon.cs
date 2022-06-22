// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Effects;
using osu.Game.Graphics.Containers;
using osu.Game.Tests.Visual;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Tests.UI
{
    [TestFixture]
    public class TestSceneRulesetIcon : OsuTestScene
    {
        public TestSceneRulesetIcon()
        {
            Child = new ConstrainedIconContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Icon = new KaraokeRuleset().CreateIcon(),
                EdgeEffect = new EdgeEffectParameters
                {
                    Type = EdgeEffectType.Glow,
                    Colour = new Color4(255, 194, 224, 100),
                    Radius = 15,
                    Roundness = 15,
                },
                Size = new Vector2(40)
            };
        }
    }
}
