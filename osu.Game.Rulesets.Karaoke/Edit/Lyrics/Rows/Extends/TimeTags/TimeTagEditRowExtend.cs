// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.TimeTags
{
    public class TimeTagEditRowExtend : EditRowExtend
    {
        public override float ContentHeight => 100;

        public TimeTagEditRowExtend(Lyric lyric)
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
            // todo : waiting for implementation.
            return new TimeTagEditor(lyric)
            {
                RelativeSizeAxes = Axes.X,
                Height = 100,
            };
        }
    }
}
