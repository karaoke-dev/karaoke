// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit.Compose.Components;
using System.Collections.Generic;
using System.Linq;
using static osu.Game.Rulesets.Karaoke.Edit.Components.Timeline.TimelineBlueprintContainer;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Components.Timeline
{
    internal class LyricTimelineSelectionHandler : TimelineSelectionHandler
    {
        [Resolved]
        private SingerManager singerManager { get; set; }

        public override bool HandleMovement(MoveSelectionEvent moveEvent) => false;

        protected override IEnumerable<MenuItem> GetContextMenuItemsForSelection(IEnumerable<SelectionBlueprint> selection)
            => singerManager.CreateSingerContextMenu(selection.Select(x => x.HitObject).OfType<Lyric>().ToList());
    }
}
