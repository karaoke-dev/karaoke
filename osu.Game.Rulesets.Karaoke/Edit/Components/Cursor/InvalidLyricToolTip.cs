// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Checker.Lyrics;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
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

            // Check time
            if (report.TimeInvalid)
                invalidMessage.AddParagraph("Invalid lyric time");

            // Check time-tag
            if (report.InvalidTimeTag?.Any() ?? false)
                invalidMessage.AddParagraph("Invalid time-tag");

            // Check ruby
            if (report.InvalidRubyTag?.Any() ?? false)
                invalidMessage.AddParagraph("Invalid ruby position.");

            // romaji
            if (report.InvalidRomajiTag?.Any() ?? false)
                invalidMessage.AddParagraph("Invalid romaji position.");

            // show no problem message
            if(!invalidMessage.FlowingChildren.Any())
                invalidMessage.AddParagraph("Seems no issue in this lyric.");

            return true;
        }
    }
}
