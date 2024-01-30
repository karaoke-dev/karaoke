// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.UI;

[TestFixture]
public partial class TestSceneRulesetIcon : OsuTestScene
{
    public TestSceneRulesetIcon()
    {
        Child = new ConstrainedIconContainer
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Icon = new KaraokeRuleset().CreateIcon(),
            Size = new Vector2(40),
        };
    }
}
