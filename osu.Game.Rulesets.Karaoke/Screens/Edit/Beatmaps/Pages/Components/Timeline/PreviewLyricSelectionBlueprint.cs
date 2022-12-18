// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Timeline;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages.Components.Timeline;

public partial class PreviewLyricSelectionBlueprint : EditableLyricTimelineSelectionBlueprint
{
    public PreviewLyricSelectionBlueprint(Lyric item)
        : base(item)
    {
        Selectable = false;
    }

    protected override void OnSelectableStatusChanged(bool selectable)
    {
        Alpha = 0.5f;
    }
}
