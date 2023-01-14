// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Stages.Classic;

public partial class TestSceneStageScreen : ClassicStageScreenTestScene<StageScreen>
{
    protected override StageScreen CreateEditorScreen() => new();
}
