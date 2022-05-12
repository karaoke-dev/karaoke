// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Parts;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows
{
    public class TimeTagLayer : CompositeDrawable
    {
        [Resolved]
        private EditorKaraokeSpriteText karaokeSpriteText { get; set; }

        private readonly IBindableList<TimeTag> timeTagsBindable = new BindableList<TimeTag>();

        public TimeTagLayer(Lyric lyric)
        {
            timeTagsBindable.BindCollectionChanged((_, _) =>
            {
                ScheduleAfterChildren(updateTimeTags);
            });

            timeTagsBindable.BindTo(lyric.TimeTagsBindable);
        }

        private void updateTimeTags()
        {
            ClearInternal();

            foreach (var timeTag in timeTagsBindable)
            {
                var position = karaokeSpriteText.GetTimeTagPosition(timeTag);
                AddInternal(new DrawableTimeTag(timeTag)
                {
                    Position = position
                });
            }
        }
    }
}
