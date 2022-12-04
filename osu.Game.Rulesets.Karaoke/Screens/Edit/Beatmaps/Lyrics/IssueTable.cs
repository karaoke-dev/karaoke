// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Edit.Checks.Components;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

public abstract class IssueTable : LyricEditorTable
{
    [Resolved, AllowNull]
    private IIssueNavigator issueNavigator { get; set; }

    protected IssueTable()
    {
        Columns = CreateHeaders();
    }

    public IEnumerable<Issue> Issues
    {
        set
        {
            Content = null;
            BackgroundFlow.Clear();

            foreach (var issue in value)
            {
                BackgroundFlow.Add(new RowBackground(issue)
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
}
