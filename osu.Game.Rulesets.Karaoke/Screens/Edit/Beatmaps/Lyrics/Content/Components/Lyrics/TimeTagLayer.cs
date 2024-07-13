// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;

public partial class TimeTagLayer : Layer
{
    [Resolved]
    private IPreviewLyricPositionProvider previewLyricPositionProvider { get; set; } = null!;

    private readonly IBindableList<TimeTag> timeTagsBindable = new BindableList<TimeTag>();

    public TimeTagLayer(Lyric lyric)
        : base(lyric)
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
            var position = previewLyricPositionProvider.GetPositionByTimeTag(timeTag);
            AddInternal(new DrawableTimeTag
            {
                Size = new Vector2(6),
                TimeTag = timeTag,
                Position = position,
            });
        }
    }

    public override void UpdateDisableEditState(bool editable)
    {
        this.FadeTo(editable ? 1 : 0.5f, 100);
    }
}
