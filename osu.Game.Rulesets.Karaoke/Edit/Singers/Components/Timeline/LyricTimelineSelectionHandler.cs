// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Components.ContextMenu;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit.Compose.Components;
using static osu.Game.Rulesets.Karaoke.Edit.Components.Timeline.TimelineBlueprintContainer;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Components.Timeline
{
    internal class LyricTimelineSelectionHandler : TimelineSelectionHandler
    {
        [Resolved]
        private LyricManager lyricManager { get; set; }

        public override bool HandleMovement(MoveSelectionEvent moveEvent) => false;

        protected override IEnumerable<MenuItem> GetContextMenuItemsForSelection(IEnumerable<SelectionBlueprint> selection)
        {
            var lyrics = selection.Select(x => x.HitObject).OfType<Lyric>().ToList();
            var contectMenu = new SingerContextMenu(lyricManager, lyrics, "");
            return contectMenu.Items;
        }
    }
}
