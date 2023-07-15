// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Overlays;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage;

public partial class StageCategoryScreenStack : WorkspaceScreenStack<StageEditorEditCategory>
{
    public const float LEFT_SIDE_PADDING = 200;

    private readonly Box background;

    public StageCategoryScreenStack()
    {
        AddInternal(background = new Box
        {
            RelativeSizeAxes = Axes.Both,
            Depth = float.MaxValue,
        });
    }

    [BackgroundDependencyLoader]
    private void load(OverlayColourProvider colourProvider)
    {
        background.Colour = colourProvider.Background5;
    }

    protected override WorkspaceScreen<StageEditorEditCategory>? CreateWorkspaceScreen(StageEditorEditCategory item) =>
        item switch
        {
            StageEditorEditCategory.Layout => null,
            StageEditorEditCategory.Timing => null,
            StageEditorEditCategory.Style => null,
            _ => throw new InvalidOperationException("Editor menu bar switched to an unsupported mode"),
        };

    protected override WorkspaceScreenStackTabControl CreateTabControl() => new StageCategoriesTabControl();

    private partial class StageCategoriesTabControl : WorkspaceScreenStackTabControl
    {
        public StageCategoriesTabControl()
        {
            TabContainer.Margin = new MarginPadding { Horizontal = LEFT_SIDE_PADDING };
        }
    }
}
