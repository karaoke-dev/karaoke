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
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class IssueNavigator : Component, IIssueNavigator
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
            var lyric = getNavigateLyric(issue);
            if (lyric == null)
                return;

            lyricCaretState.MoveCaretToTargetPosition(lyric);

            // navigate to the target index in the lyric.
            object? lyricIndex = getNavigateLyricIndex(issue);
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

        private static Lyric? getNavigateLyric(Issue issue)
        {
            var lyric = issue.HitObjects.OfType<Lyric>().FirstOrDefault();
            if (lyric != null)
                return lyric;

            var note = issue.HitObjects.OfType<Note>().FirstOrDefault();
            if (note != null)
                return note.ReferenceLyric;

            return null;
        }

        private static object? getNavigateLyricIndex(Issue issue) =>
            issue switch
            {
                RubyTagIssue rubyTagIssue => rubyTagIssue.RubyTag,
                RomajiTagIssue romajiTagIssue => romajiTagIssue.RomajiTag,
                TimeTagIssue timeTagIssue => timeTagIssue.TimeTag,
                _ => null
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
