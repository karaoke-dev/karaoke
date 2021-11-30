// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Skin;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Skinning;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens
{
    public abstract class KaraokeSkinEditorScreenTestScene<T> : EditorClockTestScene where T : KaraokeSkinEditorScreen
    {
        [Cached]
        private readonly OverlayColourProvider colourProvider = new(OverlayColourScheme.Pink);

        private KaraokeSkin karaokeSkin;

        [BackgroundDependencyLoader]
        private void load(SkinManager skinManager)
        {
            // because skin is compared by id, so should change id to let skin manager info knows that skin has been changed.
            var defaultSkin = DefaultKaraokeSkin.Default;
            defaultSkin.ID = 100;
            skinManager.CurrentSkinInfo.Value = defaultSkin;

            karaokeSkin = skinManager.CurrentSkin.Value as KaraokeSkin;
        }

        [Test]
        public void TestKaraoke() => runForRuleset(new KaraokeRuleset().RulesetInfo);

        private void runForRuleset(RulesetInfo rulesetInfo)
        {
            AddStep("create screen", () =>
            {
                Child = CreateEditorScreen(karaokeSkin).With(x =>
                {
                    x.State.Value = Visibility.Visible;
                });
            });
        }

        protected abstract T CreateEditorScreen(KaraokeSkin karaokeSkin);
    }
}
