// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

public partial class IssueNavigator : Component, IIssueNavigator
{
    [Resolved]
    private ILyricEditorState lyricEditorState { get; set; } = null!;

    [Resolved]
    private ILyricCaretState lyricCaretState { get; set; } = null!;

    [Resolved]
    private IEditRubyModeState editRubyModeState { get; set; } = null!;

    [Resolved]
    private IEditTimeTagModeState editTimeTagModeState { get; set; } = null!;

    [Resolved]
    private IEditRomanisationModeState editRomanisationModeState { get; set; } = null!;

    [Resolved]
    private IEditNoteModeState noteModeState { get; set; } = null!;

    [Resolved]
    private EditorClock clock { get; set; } = null!;

    public void Navigate(Issue issue)
    {
        // seek the time if contains the time in the issue.
        if (issue.Time.HasValue)
            clock.Seek(issue.Time.Value);

        // navigate to edit mode.
        var targetEditMode = getNavigateEditMode(issue.Check);
        if (targetEditMode != null)
            lyricEditorState.SwitchMode(targetEditMode.Value);

        // navigate to sub-mode if needed.
        var targetEditStep = getNavigateEditStep(issue);
        if (targetEditStep != null)
            lyricEditorState.SwitchEditStep(targetEditStep);

        // navigate to the target lyric.
        (var lyric, object? lyricIndex) = getNavigateLyricAndIndex(issue);
        if (lyric == null)
            return;

        lyricCaretState.MoveCaretToTargetPosition(lyric);

        // navigate to the target index in the lyric.
        if (lyricIndex == null)
            return;

        var blueprintSelection = getBlueprintSelection(lyricIndex);
        blueprintSelection?.Select(lyricIndex);
    }

    private static LyricEditorMode? getNavigateEditMode(ICheck check)
    {
        switch (check)
        {
            case CheckLyricText:
                return LyricEditorMode.EditText;

            case CheckLyricReferenceLyric:
                return LyricEditorMode.EditReferenceLyric;

            case CheckLyricLanguage:
                return LyricEditorMode.EditLanguage;

            case CheckLyricRubyTag:
                return LyricEditorMode.EditRuby;

            case CheckLyricTimeTagOnly:
                return LyricEditorMode.EditTimeTag;

            case CheckLyricRomanisation:
                return LyricEditorMode.EditRomanisation;

            case CheckNoteReferenceLyric:
            case CheckNoteText:
                return LyricEditorMode.EditNote;

            default:
                return null;
        }
    }

    private static Enum? getNavigateEditStep(Issue issue)
    {
        // todo: implement.
        return null;
    }

    private static Tuple<Lyric?, object?> getNavigateLyricAndIndex(Issue issue) =>
        issue switch
        {
            LyricRubyTagIssue rubyTagIssue => new Tuple<Lyric?, object?>(rubyTagIssue.Lyric, rubyTagIssue.RubyTag),
            LyricTimeTagIssue timeTagIssue => new Tuple<Lyric?, object?>(timeTagIssue.Lyric, timeTagIssue.TimeTag),
            LyricIssue lyricIssue => new Tuple<Lyric?, object?>(lyricIssue.Lyric, null),
            NoteIssue noteIssue => new Tuple<Lyric?, object?>(noteIssue.Note.ReferenceLyric, null),
            _ => new Tuple<Lyric?, object?>(null, null),
        };

    private IHasBlueprintSelection<TItem>? getBlueprintSelection<TItem>(TItem item) where TItem : class
    {
        object[] availableEditModes =
        {
            editRubyModeState,
            editRomanisationModeState,
            editTimeTagModeState,
            noteModeState,
        };

        return availableEditModes.OfType<IHasBlueprintSelection<TItem>>().FirstOrDefault();
    }
}
