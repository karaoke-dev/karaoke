// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Checker.Lyrics;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Objects;
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
                Name = "Invalid message",
            };
        }

        public override bool SetContent(object content)
        {
            if (!(content is LyricCheckReport report))
                return false;

            // clear exist warning.
            invalidMessage.Text = "";

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
                        invalidMessage.AddAlertParagraph("Check is start time larger then end time in lyric.");
                        break;

                    case TimeInvalid.StartTimeInvalid:
                        invalidMessage.AddAlertParagraph("Check start time is larger than minimux time tag's time.");
                        break;

                    case TimeInvalid.EndTimeInvalid:
                        invalidMessage.AddAlertParagraph("Check end time is smaller than maximum time tag's time.");
                        break;
                }
            }

            void createTimeTagInvalidMessage(TimeTagInvalid invalid, TimeTag[] timeTags)
            {
                switch (invalid)
                {
                    case TimeTagInvalid.OutOfRange:
                        invalidMessage.AddAlertParagraph("Seems some time tag is out of lyric text size.");
                        break;

                    case TimeTagInvalid.Overlapping:
                        invalidMessage.AddAlertParagraph("Seems some time tag is invalid.");
                        break;
                }
            }

            void createRubyInvalidMessage(RubyTagInvalid invalid, RubyTag[] rubyTags)
            {
                switch (invalid)
                {
                    case RubyTagInvalid.OutOfRange:
                        invalidMessage.AddAlertParagraph("Seems some ruby tag is out of lyric text size.");
                        break;

                    case RubyTagInvalid.Overlapping:
                        invalidMessage.AddAlertParagraph("Seems some ruby tag is overlapping to others.");
                        break;
                }
            }

            void createRomajiInvalidMessage(RomajiTagInvalid invalid, RomajiTag[] romajiTags)
            {
                switch (invalid)
                {
                    case RomajiTagInvalid.OutOfRange:
                        invalidMessage.AddAlertParagraph("Seems some romaji tag is out of lyric text size.");
                        break;

                    case RomajiTagInvalid.Overlapping:
                        invalidMessage.AddAlertParagraph("Seems some romaji tag is overlapping to others.");
                        break;
                }
            }
        }
    }
}
