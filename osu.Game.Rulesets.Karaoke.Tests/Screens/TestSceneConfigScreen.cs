// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Rulesets.Karaoke.Screens.Skin.Config;
using osu.Game.Rulesets.Karaoke.Skinning;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens
{
    public class TestSceneConfigScreen : KaraokeSkinEditorScreenTestScene<ConfigScreen>
    {
        protected override ConfigScreen CreateEditorScreen(KaraokeSkin karaokeSkin) => new(karaokeSkin);
    }
}
