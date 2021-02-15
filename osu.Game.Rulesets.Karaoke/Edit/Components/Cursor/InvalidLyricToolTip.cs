// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Checker.Lyrics;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Cursor
{
    public class InvalidLyricToolTip : BackgroundToolTip
    {
        private readonly OsuTextFlowContainer invalidMessage;

        public InvalidLyricToolTip()
        {
            AutoSizeAxes = Axes.Y;
            Width = 300;

            Child = invalidMessage = new OsuTextFlowContainer(s => s.Font = s.Font.With(size: 14))
            {
                RelativeSizeAxes = Axes.X,
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
            foreach (var invalid in report.TimeInvalid)
            {
                createTimeInvalidMessage(invalid);
            }

            // Print time-tag invalid message
            foreach (var invalidTimeTags in report.InvalidTimeTags)
            {
                createTimeTagInvalidMessage(invalidTimeTags.Key, invalidTimeTags.Value);
            }

            // Print ruby invalid message
            foreach (var invalidRubyTags in report.InvalidRubyTags)
            {
                createRubyInvalidMessage(invalidRubyTags.Key, invalidRubyTags.Value);
            }

            // Print romaji invalid message
            foreach (var invalidRomajiTags in report.InvalidRomajiTags)
            {
                createRomajiInvalidMessage(invalidRomajiTags.Key, invalidRomajiTags.Value);
            }

            // show no problem message
            if(report.IsValid)
                invalidMessage.AddParagraph("Seems no issue in this lyric.");

            return true;

            void createTimeInvalidMessage(TimeInvalid timeInvalid)
            {
                switch (timeInvalid)
                {
                    case TimeInvalid.Overlapping:
                        invalidMessage.AddParagraph("Invalid lyric time");
                        break;
                }
            }

            void createTimeTagInvalidMessage(TimeTagInvalid invalid, TimeTag[] timeTags)
            {
                switch (invalid)
                {
                    case TimeTagInvalid.OutOfRange:
                        invalidMessage.AddParagraph("Invalid time-tag");
                        break;
                }
            }

            void createRubyInvalidMessage(RubyTagInvalid invalid, RubyTag[] rubyTags)
            {
                switch (invalid)
                {
                    case RubyTagInvalid.OutOfRange:
                        invalidMessage.AddParagraph("Invalid ruby-tag");
                        break;
                }
            }

            void createRomajiInvalidMessage(RomajiTagInvalid invalid, RomajiTag[] romajiTags)
            {
                switch (invalid)
                {
                    case RomajiTagInvalid.OutOfRange:
                        invalidMessage.AddParagraph("Invalid tiromajime-tag");
                        break;
                }
            }
        }
    }
}
