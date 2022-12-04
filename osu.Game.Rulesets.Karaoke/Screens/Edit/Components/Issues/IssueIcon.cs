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

public class IssueIcon : CompositeDrawable
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
        InternalChild = Issue != null ? getDrawableByIssue(Issue) : null;
    }

    private static Drawable getDrawableByIssue(Issue issue)
    {
        return createIssueIcon(issue).With(x =>
        {
            x.Colour = issue.Template.Colour;
            x.RelativeSizeAxes = Axes.Both;
        });

        static Drawable createIssueIcon(Issue issue)
        {
            var drawableByIssue = GetDrawableByIssue(issue);
            if (drawableByIssue != null)
                return drawableByIssue;

            return new SpriteIcon
            {
                Icon = GetIconUsageByIssueTemplate(issue.Template) ?? GetIconUsageByCheck(issue.Check)
            };
        }
    }

    internal static Drawable? GetDrawableByIssue(Issue issue) =>
        issue switch
        {
            LyricTimeTagIssue lyricTimeTagIssue => new DrawableTextIndex { State = lyricTimeTagIssue.TimeTag.Index.State },
            _ => null
        };

    internal static IconUsage? GetIconUsageByIssueTemplate(IssueTemplate issueTemplate)
    {
        // will override the icon if needed.
        return null;
    }

    internal static IconUsage GetIconUsageByCheck(ICheck check) =>
        check switch
        {
            CheckBeatmapAvailableTranslates => FontAwesome.Solid.Language,
            CheckLyricLanguage => FontAwesome.Solid.Globe,
            CheckLyricReferenceLyric => FontAwesome.Solid.Link,
            CheckLyricRomajiTag => FontAwesome.Solid.Tag,
            CheckLyricRubyTag => FontAwesome.Solid.Tag,
            CheckLyricSinger => FontAwesome.Solid.Music,
            CheckLyricText => FontAwesome.Solid.TextHeight,
            CheckLyricTime => FontAwesome.Solid.Times,
            CheckLyricTimeTag => FontAwesome.Solid.Tag,
            CheckLyricTranslate => FontAwesome.Solid.Language,
            CheckNoteReferenceLyric => FontAwesome.Solid.Link,
            CheckNoteText => FontAwesome.Solid.Link,
            _ => throw new ArgumentOutOfRangeException(nameof(check), check, null)
        };
}
