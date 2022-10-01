// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList
{
    public class DrawablePreviewLyricListItem : DrawableLyricListItem
    {
        public DrawablePreviewLyricListItem(Lyric item)
            : base(item)
        {
        }

        protected override Row CreateEditRow(Lyric lyric)
            => new EditLyricPreviewRow(lyric);
    }
}
