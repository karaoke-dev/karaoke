// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Issues;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.LyricList.Rows.Info.FixedInfo;

public partial class InvalidInfo : SpriteIcon, IHasContextMenu, IHasCustomTooltip<Issue[]>
{
    // todo : might able to have auto-fix option by right-click
    public MenuItem[] ContextMenuItems => Array.Empty<MenuItem>();

    private readonly IBindableList<Issue> bindableIssues = new BindableList<Issue>();
    private readonly Lyric lyric;

    public InvalidInfo(Lyric lyric)
    {
        this.lyric = lyric;

        Size = new Vector2(12);
    }

    [BackgroundDependencyLoader]
    private void load(OsuColour colours, ILyricEditorVerifier verifier)
    {
        bindableIssues.BindTo(verifier.GetBindable(lyric));
        bindableIssues.BindCollectionChanged((_, args) =>
        {
            TooltipContent = bindableIssues.ToArray();

            var issue = getDisplayIssue(bindableIssues);

            if (issue == null)
            {
                Icon = FontAwesome.Solid.CheckCircle;
                Colour = colours.Green;
                return;
            }

            var displayIssueType = issue.Template.Type;
            var targetColour = issue.Template.Colour;

            switch (displayIssueType)
            {
                case IssueType.Problem:
                    Icon = FontAwesome.Solid.TimesCircle;
                    Colour = targetColour;
                    break;

                case IssueType.Warning:
                    Icon = FontAwesome.Solid.ExclamationCircle;
                    Colour = targetColour;
                    break;

                case IssueType.Error: // it's caused by internal error.
                    Icon = FontAwesome.Solid.ExclamationTriangle;
                    Colour = targetColour;
                    break;

                case IssueType.Negligible:
                    Icon = FontAwesome.Solid.InfoCircle;
                    Colour = targetColour;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }, true);
    }

    private static Issue? getDisplayIssue(IReadOnlyList<Issue> issues)
    {
        if (!issues.Any())
            return null;

        return issues.OrderByDescending(x => x.Template.Type).First();
    }

    public ITooltip<Issue[]> GetCustomTooltip()
        => new IssuesToolTip();

    public Issue[]? TooltipContent { get; private set; }
}
