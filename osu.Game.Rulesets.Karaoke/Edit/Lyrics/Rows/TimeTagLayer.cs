// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Parts;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows
{
    public class TimeTagLayer : CompositeDrawable
    {
        [Resolved]
        private EditorLyricPiece lyricPiece { get; set; }

        private readonly Lyric lyric;

        public TimeTagLayer(Lyric lyric)
        {
            this.lyric = lyric;

            lyric.TimeTagsBindable.BindCollectionChanged((_, _) =>
            {
                ScheduleAfterChildren(updateTimeTags);
            }, true);
        }

        private void updateTimeTags()
        {
            ClearInternal();
            var timeTags = lyricPiece.TimeTagsBindable;
            if (timeTags == null)
                return;

            foreach (var timeTag in timeTags)
            {
                var position = lyricPiece.GetTimeTagPosition(timeTag);
                AddInternal(new DrawableTimeTag(lyric, timeTag)
                {
                    Position = position
                });
            }
        }
    }
}
