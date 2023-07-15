// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Config;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic;

public partial class ClassicStageEditor : GenericEditor<ClassicStageEditorScreenMode>
{
    [Cached]
    private readonly OverlayColourProvider colourProvider = new(OverlayColourScheme.Green);

    protected override GenericEditorScreen<ClassicStageEditorScreenMode> GenerateScreen(ClassicStageEditorScreenMode screenMode) =>
        screenMode switch
        {
            ClassicStageEditorScreenMode.Stage => new StageScreen(),
            ClassicStageEditorScreenMode.Config => new ConfigScreen(),
            _ => throw new InvalidOperationException("Editor menu bar switched to an unsupported mode"),
        };

    protected override MenuItem[] GenerateMenuItems(ClassicStageEditorScreenMode screenMode)
    {
        return screenMode switch
        {
            _ => Array.Empty<MenuItem>(),
        };
    }
}
