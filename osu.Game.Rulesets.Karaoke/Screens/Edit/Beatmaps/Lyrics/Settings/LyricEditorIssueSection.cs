// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Colour;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public abstract partial class LyricEditorIssueSection : IssueSection
{
    protected abstract LyricEditorMode EditMode { get; }

    protected abstract LyricsIssueTable CreateLyricsIssueTable();

    protected sealed override EmptyIssue CreateEmptyIssue() => new LyricEditorEmptyIssue();

    protected sealed override IssueNavigator CreateIssueNavigator() => new LyricEditorIssueNavigator();

    protected sealed override IssueTable CreateIssueTable() => CreateLyricsIssueTable();

    [BackgroundDependencyLoader]
    private void load(ILyricEditorVerifier verifier)
    {
        Issues.BindTo(verifier.GetIssueByType(EditMode));
    }

    private partial class LyricEditorEmptyIssue : EmptyIssue
    {
        [Resolved]
        private ILyricEditorVerifier verifier { get; set; } = null!;

        [BackgroundDependencyLoader]
        private void load(LyricEditorColourProvider colourProvider, ILyricEditorState state)
        {
            Background.Colour = colourProvider.Background5(state.Mode);
            Text.Colour = colourProvider.Colour1(state.Mode);
        }

        protected override void OnRefreshButtonClicked()
            => verifier.Refresh();
    }

    private partial class LyricEditorIssueNavigator : IssueNavigator
    {
        [Resolved]
        private ILyricEditorVerifier verifier { get; set; } = null!;

        [BackgroundDependencyLoader]
        private void load(LyricEditorColourProvider colourProvider, ILyricEditorState state)
        {
            var colour = colourProvider.Background5(state.Mode);
            Background.Colour = colour;
            BlockBox.Colour = ColourInfo.GradientHorizontal(colour.Opacity(0), colour);
        }

        protected override void OnRefreshButtonClicked()
            => verifier.Refresh();
    }

    protected abstract partial class LyricsIssueTable : LyricEditorIssueTable
    {
    }
}
