// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Components.Cursor;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.RecordingTimeTags
{
    public class DrawableRecordingTimeTag : CompositeDrawable, IHasCustomTooltip
    {
        [Resolved]
        private EditorClock editorClock { get; set; }

        private readonly Bindable<double?> bindableTIme;

        private readonly TimeTag timeTag;

        public DrawableRecordingTimeTag(TimeTag timeTag)
        {
            this.timeTag = timeTag;
            var start = timeTag.Index.State == TextIndex.IndexState.Start;

            // Size = new Vector2(RecordingTimeTagEditor.TIMELINE_HEIGHT);
            Anchor = Anchor.CentreLeft;
            Origin = Anchor.CentreLeft;
            // Origin = start ? Anchor.CentreLeft : Anchor.CentreRight;
            RelativePositionAxes = Axes.X;
            RelativeSizeAxes = Axes.Y;
            AutoSizeAxes = Axes.X;

            bindableTIme = timeTag.TimeBindable.GetBoundCopy();
            InternalChild = new RightTriangle
            {
                Name = "Time tag triangle",
                Size = new Vector2(RecordingTimeTagEditor.TIMELINE_HEIGHT),
                Scale = new Vector2(start ? 1 : -1, 1)
            };
        }

        [BackgroundDependencyLoader]
        private void load(RecordingTimeTagEditor timeline)
        {
            bindableTIme.BindValueChanged(e =>
            {
                var hasValue = e.NewValue.HasValue;
                Alpha = hasValue ? 1 : 0;

                if (!hasValue)
                    return;

                X = (float)timeline.GetPreviewTime(timeTag);
            }, true);
        }

        public object TooltipContent => timeTag;

        public ITooltip GetCustomTooltip() => new TimeTagTooltip();
    }
}
