// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

public abstract partial class IssueTable : EditorTable
{
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
                BackgroundFlow.Add(CreateRowBackground(issue).With(x =>
                {
                    x.Action = () =>
                    {
                        OnIssueClicked(issue);
                    };
                }));
            }

            Content = value.Select(CreateContent).ToArray().ToRectangular();
        }
    }

    protected abstract void OnIssueClicked(Issue issue);

    protected virtual RowBackground CreateRowBackground(Issue issue) => new(issue);

    protected abstract TableColumn[] CreateHeaders();

    protected abstract Drawable[] CreateContent(Issue issue);
}
