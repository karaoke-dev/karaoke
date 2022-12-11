// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Edit.Checks.Components;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

public abstract partial class LyricEditorIssueTable : IssueTable
{
    [Resolved, AllowNull]
    private IIssueNavigator issueNavigator { get; set; }

    public new bool ShowHeaders
    {
        get => base.ShowHeaders;
        set
        {
            base.ShowHeaders = value;
            BackgroundFlow.Margin = new MarginPadding { Top = value ? ROW_HEIGHT : 0 };
        }
    }

    protected override void OnIssueClicked(Issue issue)
    {
        issueNavigator.Navigate(issue);
    }

    protected override RowBackground CreateRowBackground(Issue issue)
        => new IssueTableRowBackground(issue);

    /// <summary>
    /// Inherit the class just for able to override the colour.
    /// </summary>
    protected partial class IssueTableRowBackground : RowBackground
    {
        private const int fade_duration = 100;

        private readonly Box hoveredBackground;

        public IssueTableRowBackground(object item)
            : base(item)
        {
            hoveredBackground = Children.OfType<Box>().First();
        }

        private Color4 colourHover;
        private Color4 colourSelected;

        [BackgroundDependencyLoader]
        private void load(LyricEditorColourProvider colourProvider, ILyricEditorState state)
        {
            colourHover = colourProvider.Background1(state.Mode);
            colourSelected = colourProvider.Colour3(state.Mode);

            Schedule(() =>
            {
                hoveredBackground.Colour = colourHover;
            });
        }

        protected override bool OnHover(HoverEvent e)
        {
            bool hover = base.OnHover(e);
            updateState();
            return hover;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);
            updateState();
        }

        private void updateState()
        {
            hoveredBackground.FadeColour(Selected ? colourSelected : colourHover, 450, Easing.OutQuint);

            if (Selected || IsHovered)
                hoveredBackground.FadeIn(fade_duration, Easing.OutQuint);
            else
                hoveredBackground.FadeOut(fade_duration, Easing.OutQuint);
        }
    }
}
