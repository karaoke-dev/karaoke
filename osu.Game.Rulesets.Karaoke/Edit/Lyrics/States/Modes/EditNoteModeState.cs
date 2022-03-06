// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes
{
    public class EditNoteModeState : Component, IEditNoteModeState
    {
        private readonly Bindable<NoteEditMode> bindableEditMode = new();
        private readonly BindableList<HitObject> selectedHitObjects = new();

        public IBindable<NoteEditMode> BindableEditMode => bindableEditMode;

        public void ChangeEditMode(NoteEditMode mode)
            => bindableEditMode.Value = mode;

        public BindableList<Note> SelectedItems { get; } = new();

        public Bindable<NoteEditModeSpecialAction> BindableSpecialAction { get; } = new();

        public Bindable<NoteEditPropertyMode> NoteEditPropertyMode { get; } = new();

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap editorBeatmap)
        {
            BindablesUtils.Sync(SelectedItems, selectedHitObjects);
            selectedHitObjects.BindTo(editorBeatmap.SelectedHitObjects);
        }
    }
}
