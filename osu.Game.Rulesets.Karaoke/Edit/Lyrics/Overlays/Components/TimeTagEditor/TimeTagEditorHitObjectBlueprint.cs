// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Overlays.Components.TimeTagEditor
{
    public class TimeTagEditorHitObjectBlueprint : SelectionBlueprint<TimeTag>
    {
        public Action<DragEvent> OnDragHandled;

        public TimeTagEditorHitObjectBlueprint(TimeTag item)
            : base(item)
        {
        }
    }
}
