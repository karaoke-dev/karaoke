// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit.Components.Timelines.Summary.Parts;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Overlays.Components.TimeTagEditor
{
    public class TimeTagEditorBlueprintContainer : BlueprintContainer<TimeTag>
    {
        protected override Container<SelectionBlueprint<TimeTag>> CreateSelectionBlueprintContainer()
            => new TimeTagEditorSelectionBlueprintContainer { RelativeSizeAxes = Axes.Both };

        protected override SelectionHandler<TimeTag> CreateSelectionHandler()
            => new TimeTagEditorSelectionHandler();

        protected override SelectionBlueprint<TimeTag> CreateBlueprintFor(TimeTag item)
        {
            return new TimeTagEditorHitObjectBlueprint(item)
            {
                OnDragHandled = handleScrollViaDrag
            };
        }

        protected class TimeTagEditorSelectionHandler : SelectionHandler<TimeTag>
        {
            [Resolved]
            private LyricManager lyricManager { get; set; }

            protected override void DeleteItems(IEnumerable<TimeTag> items)
            {
                // todo : delete time-line
                foreach (var item in items)
                {
                    lyricManager.RemoveTimeTag(item);
                }
            }
        }

        protected class TimeTagEditorSelectionBlueprintContainer : Container<SelectionBlueprint<TimeTag>>
        {
            protected override Container<SelectionBlueprint<TimeTag>> Content { get; }

            public TimeTagEditorSelectionBlueprintContainer()
            {
                AddInternal(new TimelinePart<SelectionBlueprint<TimeTag>>(Content = new TimeTagOrderedSelectionContainer { RelativeSizeAxes = Axes.Both }) { RelativeSizeAxes = Axes.Both });
            }

            protected class TimeTagOrderedSelectionContainer : Container<SelectionBlueprint<TimeTag>>
            {
            }
        }
    }
}
