// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Edit.Checks.Components;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

public abstract partial class LyricEditorIssueTable : IssueTable
{
    [Resolved]
    private IIssueNavigator issueNavigator { get; set; } = null!;

    private Color4 colourHover;
    private Color4 colourSelected;

    [BackgroundDependencyLoader]
    private void load(LyricEditorColourProvider colourProvider, ILyricEditorState state)
    {
        colourHover = colourProvider.Background1(state.Mode);
        colourSelected = colourProvider.Colour3(state.Mode);
    }

    protected sealed override Color4 GetBackgroundColour(bool selected)
    {
        return selected ? colourSelected : colourHover;
    }

    protected override void OnIssueClicked(Issue issue)
    {
        issueNavigator.Navigate(issue);
    }
}
