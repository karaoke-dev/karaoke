// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Edit.Checks.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings
{
    public abstract class IssueSection : LyricEditorSection
    {
        protected sealed override LocalisableString Title => "Issues";

        private readonly IBindableList<Issue> bindableIssues = new BindableList<Issue>();

        protected IssueSection()
        {
            IssueTable issueTable;

            Children = new[]
            {
                // todo: add the issue amount display also.
                issueTable = CreateIssueTable()
            };

            bindableIssues.BindCollectionChanged((_, _) =>
            {
                issueTable.Issues = bindableIssues.Take(100);
            });
        }

        protected abstract LyricEditorMode EditMode { get; }

        protected abstract IssueTable CreateIssueTable();

        [BackgroundDependencyLoader]
        private void load(ILyricEditorVerifier verifier)
        {
            bindableIssues.BindTo(verifier.GetIssueByEditMode(EditMode));
        }

        protected abstract class IssueTable : LyricEditorTable
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
    }
}
