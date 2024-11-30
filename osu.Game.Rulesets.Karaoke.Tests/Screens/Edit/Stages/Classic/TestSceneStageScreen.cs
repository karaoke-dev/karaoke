// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Stages.Classic;

[Ignore("Ingore this test until able to edit the stage.")]
public partial class TestSceneStageScreen : ClassicStageScreenTestScene<StageScreen>
{
    protected override StageScreen CreateEditorScreen() => new();

    [Test]
    public void TestSwitchCategoryAndEditMode()
    {
        var stageScreen = Children.OfType<StageScreen>().First();

        AddWaitStep("wait for editor to load", 5);

        foreach (var category in Enum.GetValues<StageEditorEditCategory>())
        {
            foreach (var editMode in Enum.GetValues<StageEditorEditMode>())
            {
                AddLabel($"{Enum.GetName(category)} category with {Enum.GetName(editMode)} mode");

                AddStep($"switch to mode {Enum.GetName(editMode)}", () =>
                {
                    stageScreen.ChangeEditCategory(category);
                    stageScreen.BindableEditMode.Value = editMode;
                });
                AddWaitStep("wait for switch to new mode", 5);
            }
        }
    }
}
