// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.BottomEditor.AdjustTimeTags;

/// <summary>
/// A container for <see cref="SelectionBlueprint{T}"/> ordered by their <see cref="TimeTag"/> start times.
/// </summary>
public partial class TimeTagOrderedSelectionContainer : Container<SelectionBlueprint<TimeTag>>
{
    public override void Add(SelectionBlueprint<TimeTag> drawable)
    {
        SortInternal();
        base.Add(drawable);
    }

    public override bool Remove(SelectionBlueprint<TimeTag> drawable, bool disposeImmediately)
    {
        SortInternal();
        return base.Remove(drawable, disposeImmediately);
    }

    protected override int Compare(Drawable x, Drawable y)
    {
        var xObj = ((SelectionBlueprint<TimeTag>)x).Item;
        var yObj = ((SelectionBlueprint<TimeTag>)y).Item;

        double xTime = xObj.Time ?? 0;
        double yTime = yObj.Time ?? 0;

        // Put earlier blueprints towards the end of the list, so they handle input first
        int result = yTime.CompareTo(xTime);
        if (result != 0) return result;

        return CompareReverseChildID(x, y);
    }
}
