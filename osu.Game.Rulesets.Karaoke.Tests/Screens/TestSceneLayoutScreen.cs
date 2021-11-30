// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Screens.Skin.Layout;
using osu.Game.Rulesets.Karaoke.Skinning;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens
{
    [TestFixture]
    public class TestSceneLayoutScreen : KaraokeSkinEditorScreenTestScene<LayoutScreen>
    {
        protected override LayoutScreen CreateEditorScreen(KaraokeSkin karaokeSkin) => new(karaokeSkin);
    }
}
