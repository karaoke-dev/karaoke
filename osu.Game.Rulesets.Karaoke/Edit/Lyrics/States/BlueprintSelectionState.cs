// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States
{
    public class BlueprintSelectionState : Component, IBlueprintSelectionState
    {
        public BindableList<TimeTag> SelectedTimeTags { get; } = new();

        public BindableList<RubyTag> SelectedRubyTags { get; } = new();

        public BindableList<RomajiTag> SelectedRomajiTags { get; } = new();

        public BindableList<Note> SelectedNotes { get; } = new();

        private readonly BindableList<HitObject> selectedHitObjects = new();

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap editorBeatmap)
        {
            BindablesUtils.Sync(SelectedNotes, selectedHitObjects);
            selectedHitObjects.BindTo(editorBeatmap.SelectedHitObjects);
        }

        public void ClearSelectedTimeTags()
        {
            SelectedTimeTags.Clear();
        }

        public void ClearSelectedTextTags()
        {
            SelectedRubyTags.Clear();
            SelectedRomajiTags.Clear();
        }
    }
}
