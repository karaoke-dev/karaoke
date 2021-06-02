// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.TimeTags
{
    /// <summary>
    /// A container for <see cref="SelectionBlueprint{T}"/> ordered by their <see cref="TimeTag"/> start times.
    /// </summary>
    public class TimeTagOrderedSelectionContainer : Container<SelectionBlueprint<TimeTag>>
    {
        public override void Add(SelectionBlueprint<TimeTag> drawable)
        {
            base.Add(drawable);
            bindStartTime(drawable);
        }

        public override bool Remove(SelectionBlueprint<TimeTag> drawable)
        {
            if (!base.Remove(drawable))
                return false;

            unbindStartTime(drawable);
            return true;
        }

        public override void Clear(bool disposeChildren)
        {
            base.Clear(disposeChildren);
            unbindAllStartTimes();
        }

        private readonly Dictionary<SelectionBlueprint<TimeTag>, IBindable> startTimeMap = new Dictionary<SelectionBlueprint<TimeTag>, IBindable>();

        private void bindStartTime(SelectionBlueprint<TimeTag> blueprint)
        {
            var bindable = blueprint.Item.TimeBindable.GetBoundCopy();

            bindable.BindValueChanged(_ =>
            {
                if (LoadState >= LoadState.Ready)
                    SortInternal();
            });

            startTimeMap[blueprint] = bindable;
        }

        private void unbindStartTime(SelectionBlueprint<TimeTag> blueprint)
        {
            startTimeMap[blueprint].UnbindAll();
            startTimeMap.Remove(blueprint);
        }

        private void unbindAllStartTimes()
        {
            foreach (var kvp in startTimeMap)
                kvp.Value.UnbindAll();
            startTimeMap.Clear();
        }

        protected override int Compare(Drawable x, Drawable y)
        {
            var xObj = (SelectionBlueprint<TimeTag>)x;
            var yObj = (SelectionBlueprint<TimeTag>)y;

            // todo : have a better way to compare two object with nullable time.
            // Put earlier blueprints towards the end of the list, so they handle input first
            // int i = yObj.Item.Time.Value.CompareTo(xObj.Item.Time.Value);
            // if (i != 0) return i;

            return CompareReverseChildID(y, x);
        }
    }
}
