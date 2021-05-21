// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics.Blueprints;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics
{
    public class TimeTagBlueprintContainer : ExtendBlueprintContainer<TimeTag>
    {
        [Resolved]
        private ILyricEditorState state { get; set; }

        [UsedImplicitly]
        private readonly Bindable<TimeTag[]> timeTags;

        protected readonly Lyric Lyric;

        public TimeTagBlueprintContainer(Lyric lyric)
        {
            Lyric = lyric;
            timeTags = lyric.TimeTagsBindable.GetBoundCopy();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            SelectedItems.BindTo(state.SelectedTimeTags);

            // Add time tag into blueprint container
            RegistBindable(timeTags);
        }

        protected override SelectionHandler<TimeTag> CreateSelectionHandler()
            => new TimeTahSelectionHandler();

        protected override SelectionBlueprint<TimeTag> CreateBlueprintFor(TimeTag item)
            => new TimeTagSelectionBlueprint(item);

        public override bool HandlePositionalInput => false;

        protected override void DeselectAll()
        {
            state.ClearSelectedTimeTags();
        }

        protected class TimeTahSelectionHandler : ExtendSelectionHandler<TimeTag>
        {
            [Resolved]
            private ILyricEditorState state { get; set; }

            [Resolved]
            private LyricManager lyricManager { get; set; }

            [BackgroundDependencyLoader]
            private void load()
            {
                SelectedItems.BindTo(state.SelectedTimeTags);
            }

            // for now we always allow movement. snapping is provided by the Timeline's "distance" snap implementation
            public override bool HandleMovement(MoveSelectionEvent<TimeTag> moveEvent) => true;

            protected override void DeleteItems(IEnumerable<TimeTag> items)
            {
                // todo : delete time-tag
            }
        }
    }
}
