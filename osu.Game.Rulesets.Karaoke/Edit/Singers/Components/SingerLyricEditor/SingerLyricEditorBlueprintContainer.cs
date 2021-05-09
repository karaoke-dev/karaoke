// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.Components.ContextMenu;
using osu.Game.Rulesets.Karaoke.Edit.Components.Timeline;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Components.SingerLyricEditor
{
    public class LyricBlueprintContainer : TimelineBlueprintContainer
    {
        private readonly Singer singer;

        public LyricBlueprintContainer(HitObjectComposer composer, Singer singer)
            : base(composer)
        {
            this.singer = singer;
        }

        protected override SelectionHandler<HitObject> CreateSelectionHandler()
            => new LyricTimelineSelectionHandler();

        protected override SelectionBlueprint<HitObject> CreateBlueprintFor(HitObject hitObject)
        {
            if (!(hitObject is Lyric))
                return null;

            return new LyricTimelineHitObjectBlueprint(hitObject, singer);
        }

        internal class LyricTimelineSelectionHandler : TimelineSelectionHandler
        {
            [Resolved]
            private LyricManager lyricManager { get; set; }

            public override bool HandleMovement(MoveSelectionEvent<HitObject> moveEvent) => false;

            protected override IEnumerable<MenuItem> GetContextMenuItemsForSelection(IEnumerable<SelectionBlueprint<HitObject>> selection)
            {
                var lyrics = selection.Select(x => x.Item).OfType<Lyric>().ToList();
                var contextMenu = new SingerContextMenu(lyricManager, lyrics, "");
                return contextMenu.Items;
            }
        }
    }
}
