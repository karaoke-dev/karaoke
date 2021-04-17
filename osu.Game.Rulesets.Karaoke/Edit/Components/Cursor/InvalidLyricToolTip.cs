// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checker.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Cursor
{
    public class InvalidLyricToolTip : BackgroundToolTip
    {
        private readonly MessageContainer invalidMessage;

        public InvalidLyricToolTip()
        {
            Child = invalidMessage = new MessageContainer(s => s.Font = s.Font.With(size: 14))
            {
                Width = 300,
                AutoSizeAxes = Axes.Y,
                Colour = Color4.White.Opacity(0.75f),
                Spacing = new Vector2(0, 5),
                Name = "Invalid message",
            };
        }

        public override bool SetContent(object content)
        {
            if (!(content is Issue[] issues))
                return false;

            // clear exist warning.
            invalidMessage.Clear();

            foreach (var issue in issues)
            {
                switch (issue)
                {
                    // Print time invalid message
                    case LyricTimeIssue lyricTimeIssue:
                        lyricTimeIssue.InvalidLyricTime?.ForEach(createTimeInvalidMessage);
                        break;

                    // Print time-tag invalid message
                    case TimeTagIssue timeTagIssue:
                        timeTagIssue.InvalidTimeTags?.ForEach(x => createTimeTagInvalidMessage(x.Key, x.Value));
                        break;

                    // Print ruby invalid message
                    case RubyTagIssue rubyTagIssue:
                        rubyTagIssue.InvalidRubyTags?.ForEach(x => createRubyInvalidMessage(x.Key, x.Value));
                        break;

                    // Print romaji invalid message
                    case RomajiTagIssue romajiTagIssue:
                        romajiTagIssue.InvalidRomajiTags?.ForEach(x => createRomajiInvalidMessage(x.Key, x.Value));
                        break;

                    // Should throw exception because every issue message should be printed.
                    default:
                        throw new ArgumentOutOfRangeException(nameof(issue));
                }
            }

            // show no problem message
            if (issues.Length == 0)
                invalidMessage.AddSuccessParagraph("Seems no issue in this lyric.");

            return true;

            void createTimeInvalidMessage(TimeInvalid timeInvalid)
            {
                switch (timeInvalid)
                {
                    case TimeInvalid.Overlapping:
                        invalidMessage.AddAlertParagraph("Start time larger then end time in lyric.");
                        break;

                    case TimeInvalid.StartTimeInvalid:
                        invalidMessage.AddAlertParagraph("Start time is larger than minimux time tag's time.");
                        break;

                    case TimeInvalid.EndTimeInvalid:
                        invalidMessage.AddAlertParagraph("End time is smaller than maximum time tag's time.");
                        break;
                }
            }

            void createTimeTagInvalidMessage(TimeTagInvalid invalid, TimeTag[] timeTags)
            {
                switch (invalid)
                {
                    case TimeTagInvalid.OutOfRange:
                        invalidMessage.AddAlertParagraph("Time tag(s) is out of lyric text size at position ");
                        break;

                    case TimeTagInvalid.Overlapping:
                        invalidMessage.AddAlertParagraph("Time tag(s) is invalid at position ");
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(invalid));
                }

                displayInvalidTag(timeTags, tag => invalidMessage.AddHighlightText(TextIndexUtils.PositionFormattedString(tag.Index)));
            }

            void createRubyInvalidMessage(RubyTagInvalid invalid, RubyTag[] rubyTags)
            {
                switch (invalid)
                {
                    case RubyTagInvalid.OutOfRange:
                        invalidMessage.AddAlertParagraph("Ruby tag(s) is out of lyric text size at position ");
                        break;

                    case RubyTagInvalid.Overlapping:
                        invalidMessage.AddAlertParagraph("Ruby tag(s) is overlapping to others at position ");
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(invalid));
                }

                displayInvalidTag(rubyTags, tag => invalidMessage.AddHighlightText(TextTagUtils.PositionFormattedString(tag)));
            }

            void createRomajiInvalidMessage(RomajiTagInvalid invalid, RomajiTag[] romajiTags)
            {
                switch (invalid)
                {
                    case RomajiTagInvalid.OutOfRange:
                        invalidMessage.AddAlertParagraph("Romaji tag(s) is out of lyric text size at position ");
                        break;

                    case RomajiTagInvalid.Overlapping:
                        invalidMessage.AddAlertParagraph("Romaji tag(s) is overlapping to others at position ");
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(invalid));
                }

                displayInvalidTag(romajiTags, tag => invalidMessage.AddHighlightText(TextTagUtils.PositionFormattedString(tag)));
            }

            void displayInvalidTag<T>(T[] tags, Action<T> action)
            {
                if (tags == null)
                    throw new ArgumentNullException(nameof(tags));

                // e.g: ka(-2~-1), ra(4~6), and ke(6~7)
                for (int i = 0; i < tags.Length; i++)
                {
                    action?.Invoke(tags[i]);

                    if (i == tags.Length - 2 && tags.Length > 1)
                    {
                        invalidMessage.AddText(" and ");
                    }
                    else if (i >= 0 && tags.Length > 1)
                    {
                        invalidMessage.AddText(", ");
                    }
                    else
                    {
                        invalidMessage.AddText(".");
                    }
                }
            }
        }
    }
}
