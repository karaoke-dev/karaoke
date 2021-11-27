// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Screens.Skin.Style;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens
{
    [TestFixture]
    public class TestSceneStyleScreen : KaraokeSkinEditorScreenTestScene<StyleScreen>
    {
        protected override StyleScreen CreateEditorScreen() => new();

        [BackgroundDependencyLoader]
        private void load(SkinManager skinManager)
        {
            // because skin is compared by id, so should change id to let skin manager info knows that skin has been changed.
            var defaultSkin = DefaultKaraokeSkin.Default;
            defaultSkin.ID = 100;
            skinManager.CurrentSkinInfo.Value = defaultSkin;
        }
    }
}
