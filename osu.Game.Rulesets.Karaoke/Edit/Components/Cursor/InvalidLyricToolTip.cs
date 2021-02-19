// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Checker.Lyrics;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
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
            if (!(content is LyricCheckReport report))
                return false;

            // clear exist warning.
            invalidMessage.Clear();

            // Print time invalid message
            foreach (var invalid in report.TimeInvalid.EmptyIfNull())
            {
                createTimeInvalidMessage(invalid);
            }

            // Print time-tag invalid message
            foreach (var invalidTimeTags in report.InvalidTimeTags.EmptyIfNull())
            {
                createTimeTagInvalidMessage(invalidTimeTags.Key, invalidTimeTags.Value);
            }

            // Print ruby invalid message
            foreach (var invalidRubyTags in report.InvalidRubyTags.EmptyIfNull())
            {
                createRubyInvalidMessage(invalidRubyTags.Key, invalidRubyTags.Value);
            }

            // Print romaji invalid message
            foreach (var invalidRomajiTags in report.InvalidRomajiTags.EmptyIfNull())
            {
                createRomajiInvalidMessage(invalidRomajiTags.Key, invalidRomajiTags.Value);
            }

            // show no problem message
            if(report.IsValid)
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
