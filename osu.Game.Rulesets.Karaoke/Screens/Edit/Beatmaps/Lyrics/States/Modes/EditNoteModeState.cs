// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Notes;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

public partial class EditNoteModeState : ModeStateWithBlueprintContainer<Note>, IEditNoteModeState
{
    private readonly BindableList<HitObject> selectedHitObjects = new();

    [Resolved]
    private EditorBeatmap editorBeatmap { get; set; } = null!;

    public Bindable<NoteEditStep> BindableEditStep { get; } = new();

    public Bindable<NoteEditModeSpecialAction> BindableSpecialAction { get; } = new();

    public Bindable<NoteEditPropertyMode> BindableNoteEditPropertyMode { get; } = new();

    [BackgroundDependencyLoader]
    private void load()
    {
        BindablesUtils.Sync(SelectedItems, selectedHitObjects);
        selectedHitObjects.BindTo(editorBeatmap.SelectedHitObjects);
    }

    protected override bool IsWriteLyricPropertyLocked(Lyric lyric)
        => HitObjectWritableUtils.IsCreateOrRemoveNoteLocked(lyric);

    protected override bool SelectFirstProperty(Lyric lyric)
        => BindableEditStep.Value == NoteEditStep.Edit;

    protected override IEnumerable<Note> SelectableProperties(Lyric lyric)
        => EditorBeatmapUtils.GetNotesByLyric(editorBeatmap, lyric);
}
