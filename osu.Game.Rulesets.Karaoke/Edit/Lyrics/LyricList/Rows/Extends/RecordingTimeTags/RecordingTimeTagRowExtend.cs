// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows.Extends.RecordingTimeTags
{
    public class RecordingTimeTagRowExtend : EditRowExtend
    {
        public override float ContentHeight => 60;

        public RecordingTimeTagRowExtend(Lyric lyric)
            : base(lyric)
        {
        }

        protected override Drawable CreateInfo(Lyric lyric)
        {
            // todo : waiting for implementation.
            return new Container();
        }

        protected override Drawable CreateContent(Lyric lyric)
        {
            return new RecordingTimeTagEditor(lyric)
            {
                RelativeSizeAxes = Axes.X,
                Height = 60,
            };
        }
    }
}
