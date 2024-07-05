// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Issues;

public partial class IssueIcon : CompositeDrawable
{
    private Issue? issue;

    public virtual Issue? Issue
    {
        get => issue;
        set
        {
            issue = value;
            updateIssue();
        }
    }

    private void updateIssue()
    {
        InternalChild = Issue != null ? createDrawableByIssue(Issue) : null;
    }

    private static Drawable createDrawableByIssue(Issue issue)
    {
        return createDrawable(issue).With(x =>
        {
            x.Colour = issue.Template.Colour;
            x.RelativeSizeAxes = Axes.Both;
        });

        static Drawable createDrawable(Issue issue) =>
            issue switch
            {
                LyricTimeTagIssue lyricTimeTagIssue => new DrawableTextIndex { State = lyricTimeTagIssue.TimeTag.Index.State },
                _ => new SpriteIcon
                {
                    Icon = getIconByIssue(issue),
                },
            };
    }

    private static IconUsage getIconByIssue(Issue issue)
        => getIconByIssueTemplate(issue.Template);

    private static IconUsage getIconByIssueTemplate(IssueTemplate issueTemplate)
        => getIconUsageByIssueTemplate(issueTemplate) ?? getIconUsageByCheck(issueTemplate.Check);

    private static IconUsage? getIconUsageByIssueTemplate(IssueTemplate issueTemplate)
    {
        // will override the icon if needed.
        return null;
    }

    private static IconUsage getIconUsageByCheck(ICheck check) =>
        check switch
        {
            CheckBeatmapAvailableTranslations => FontAwesome.Solid.Language,
            CheckBeatmapClassicStageInfo => FontAwesome.Solid.AlignLeft,
            CheckBeatmapNoteInfo => FontAwesome.Solid.Microphone,
            CheckBeatmapPageInfo => FontAwesome.Solid.Pager,
            CheckLyricLanguage => FontAwesome.Solid.Globe,
            CheckLyricReferenceLyric => FontAwesome.Solid.Link,
            CheckLyricRubyTag => FontAwesome.Solid.Tag,
            CheckLyricSinger => FontAwesome.Solid.Music,
            CheckLyricText => FontAwesome.Solid.TextHeight,
            CheckLyricTime => FontAwesome.Solid.Times,
            CheckLyricTimeTag => FontAwesome.Solid.Tag,
            CheckLyricTranslations => FontAwesome.Solid.Language,
            CheckNoteReferenceLyric => FontAwesome.Solid.Link,
            CheckNoteText => FontAwesome.Solid.Link,
            CheckNoteTime => FontAwesome.Solid.Times,
            _ => throw new ArgumentOutOfRangeException(nameof(check), check, null),
        };
}
