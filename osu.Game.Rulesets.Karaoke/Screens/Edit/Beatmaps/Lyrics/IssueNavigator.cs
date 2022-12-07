// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics.CodeAnalysis;
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

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics
{
    public partial class IssueNavigator : Component, IIssueNavigator
    {
        [Resolved, AllowNull]
        private ILyricEditorState lyricEditorState { get; set; }

        [Resolved, AllowNull]
        private ILyricCaretState lyricCaretState { get; set; }

        [Resolved, AllowNull]
        private IEditRubyModeState editRubyModeState { get; set; }

        [Resolved, AllowNull]
        private IEditRomajiModeState editRomajiModeState { get; set; }

        [Resolved, AllowNull]
        private ITimeTagModeState timeTagModeState { get; set; }

        [Resolved, AllowNull]
        private IEditNoteModeState noteModeState { get; set; }

        [Resolved, AllowNull]
        private EditorClock clock { get; set; }

        public void Navigate(Issue issue)
        {
            // navigate to edit mode.
            var targetEditMode = getNavigateEditMode(issue.Check);
            if (targetEditMode != null)
                lyricEditorState.SwitchMode(targetEditMode.Value);

            // navigate to sub-mode if needed.
            var targetSubMode = getNavigateSubMode(issue);
            if (targetSubMode != null)
                lyricEditorState.SwitchSubMode(targetSubMode);

            // navigate to the target lyric.
            (var lyric, object? lyricIndex) = getNavigateLyricAndIndex(issue);
            if (lyric == null)
                return;

            lyricCaretState.MoveCaretToTargetPosition(lyric);
            clock.Seek(lyric.LyricStartTime);

            // navigate to the target index in the lyric.
            if (lyricIndex == null)
                return;

            var blueprintSelection = getBlueprintSelection(lyricIndex);
            blueprintSelection?.Select(lyricIndex);

            if (lyricIndex is not TimeTag timeTag || timeTag.Time == null)
                return;

            // seek to target time-tag time if time-tag has time.
            clock.Seek(timeTag.Time.Value);
        }

        private static LyricEditorMode? getNavigateEditMode(ICheck check)
        {
            switch (check)
            {
                case CheckLyricText:
                    return LyricEditorMode.Texting;

                case CheckLyricReferenceLyric:
                    return LyricEditorMode.Reference;

                case CheckLyricLanguage:
                    return LyricEditorMode.Language;

                case CheckLyricRubyTag:
                    return LyricEditorMode.EditRuby;

                case CheckLyricRomajiTag:
                    return LyricEditorMode.EditRomaji;

                case CheckLyricTimeTag:
                    return LyricEditorMode.EditTimeTag;

                case CheckNoteReferenceLyric:
                case CheckNoteText:
                    return LyricEditorMode.EditNote;

                default:
                    return null;
            }
        }

        private static Enum? getNavigateSubMode(Issue issue)
        {
            // todo: implement.
            return null;
        }

        private static Tuple<Lyric?, object?> getNavigateLyricAndIndex(Issue issue) =>
            issue switch
            {
                LyricRubyTagIssue rubyTagIssue => new Tuple<Lyric?, object?>(rubyTagIssue.Lyric, rubyTagIssue.RubyTag),
                LyricRomajiTagIssue romajiTagIssue => new Tuple<Lyric?, object?>(romajiTagIssue.Lyric, romajiTagIssue.RomajiTag),
                LyricTimeTagIssue timeTagIssue => new Tuple<Lyric?, object?>(timeTagIssue.Lyric, timeTagIssue.TimeTag),
                LyricIssue lyricIssue => new Tuple<Lyric?, object?>(lyricIssue.Lyric, null),
                NoteIssue noteIssue => new Tuple<Lyric?, object?>(noteIssue.Note.ReferenceLyric, null),
                _ => new Tuple<Lyric?, object?>(null, null)
            };

        private IHasBlueprintSelection<TItem>? getBlueprintSelection<TItem>(TItem item) where TItem : class
        {
            object[] availableEditModes =
            {
                editRubyModeState,
                editRomajiModeState,
                timeTagModeState,
                noteModeState
            };

            return availableEditModes.OfType<IHasBlueprintSelection<TItem>>().FirstOrDefault();
        }
    }
}
