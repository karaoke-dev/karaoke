// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Screens.Edit.Compose.Components;
using System.Collections.Generic;
using static osu.Game.Rulesets.Karaoke.Edit.Components.Timeline.TimelineBlueprintContainer;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Components.Timeline
{
    internal class LyricTimelineSelectionHandler : TimelineSelectionHandler
    {
        public override bool HandleMovement(MoveSelectionEvent moveEvent) => false;

        protected override IEnumerable<MenuItem> GetContextMenuItemsForSelection(IEnumerable<SelectionBlueprint> selection)
        {
            // todo : make implementation of singers selection.
            var menu = new List<MenuItem>
            {
                new OsuMenuItem("Test1", MenuItemType.Standard, () => { })
            };
            return menu.ToArray();
        }
    }
}
