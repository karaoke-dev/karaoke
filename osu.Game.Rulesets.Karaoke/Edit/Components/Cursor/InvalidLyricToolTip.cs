// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
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

        [Resolved]
        private LyricChecker lyricChecker { get; set; }

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
            if (!(content is Lyric lyric))
                return false;

            // clear exist warning.
            invalidMessage.Text = "";

            // Check time
            if (lyricChecker.InvalidLyricTime(lyric))
                invalidMessage.AddParagraph("Invalid lyric time");

            // Check time-tag
            if (lyricChecker.CheckInvalidTimeTagTime(lyric).Any())
                invalidMessage.AddParagraph("Invalid time-tag");

            // Check ruby
            if (lyricChecker.CheckInvalidRubyRange(lyric).Any())
                invalidMessage.AddParagraph("Invalid ruby position.");
            if (lyricChecker.CheckOverlappingRubyPosition(lyric).Any())
                invalidMessage.AddParagraph("Invalid ruby overlapping.");

            // romaji
            if (lyricChecker.CheckInvalidRomajiRange(lyric).Any())
                invalidMessage.AddParagraph("Invalid romaji position.");
            if (lyricChecker.CheckOverlappingRomajiPosition(lyric).Any())
                invalidMessage.AddParagraph("Invalid romaji overlapping.");

            // show no problem message
            if(!invalidMessage.FlowingChildren.Any())
                invalidMessage.AddParagraph("Seems no issue in this lyric.");

            return true;
        }
    }
}
