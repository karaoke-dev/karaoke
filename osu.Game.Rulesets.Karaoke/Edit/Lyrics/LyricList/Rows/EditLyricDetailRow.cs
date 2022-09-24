// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows.Info.Badge;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows
{
    public class EditLyricDetailRow : DetailRow
    {
        public EditLyricDetailRow(Lyric lyric)
            : base(lyric)
        {
        }

        protected override Drawable CreateTimingInfo(Lyric lyric)
        {
            return new TimeTagInfo(lyric)
            {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Margin = new MarginPadding { Right = 10 }
            };
        }

        protected override Drawable CreateContent(Lyric lyric)
        {
            return new SingleLyricEditor(lyric)
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                Margin = new MarginPadding { Left = 10 },
                RelativeSizeAxes = Axes.X,
            };
        }
    }
}
