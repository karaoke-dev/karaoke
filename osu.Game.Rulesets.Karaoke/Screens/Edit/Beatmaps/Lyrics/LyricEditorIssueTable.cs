// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Screens.Edit;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

public abstract partial class LyricEditorIssueTable : EditorTable
{
    private const float horizontal_inset = 20;

    [Resolved, AllowNull]
    private IIssueNavigator issueNavigator { get; set; }

    protected LyricEditorIssueTable()
    {
        Padding = new MarginPadding { Horizontal = horizontal_inset };
        BackgroundFlow.Padding = new MarginPadding { Horizontal = -horizontal_inset };

        Columns = CreateHeaders();
    }

    public new bool ShowHeaders
    {
        get => base.ShowHeaders;
        set
        {
            base.ShowHeaders = value;
            BackgroundFlow.Margin = new MarginPadding { Top = value ? ROW_HEIGHT : 0 };
        }
    }

    public IEnumerable<Issue> Issues
    {
        set
        {
            Content = null;
            BackgroundFlow.Clear();

            foreach (var issue in value)
            {
                BackgroundFlow.Add(new IssueTableRowBackground(issue)
                {
                    Action = () =>
                    {
                        issueNavigator.Navigate(issue);
                    },
                });
            }

            Content = value.Select(CreateContent).ToArray().ToRectangular();
        }
    }

    protected abstract TableColumn[] CreateHeaders();

    protected abstract Drawable[] CreateContent(Issue issue);

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
