// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.IO.Serialization;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class LyricEditorClipboard : Component, ILyricEditorClipboard
    {
        [Resolved, AllowNull]
        private GameHost host { get; set; }

        [Resolved, AllowNull]
        private ILyricEditorState state { get; set; }

        [Resolved, AllowNull]
        private ILyricCaretState lyricCaretState { get; set; }

        private Lyric? getSelectedLyric() => lyricCaretState.BindableCaretPosition.Value?.Lyric;

        [Resolved, AllowNull]
        private IEditRubyModeState editRubyModeState { get; set; }

        [Resolved, AllowNull]
        private IEditRomajiModeState editRomajiModeState { get; set; }

        [Resolved, AllowNull]
        private ITimeTagModeState timeTagModeState { get; set; }

        [Resolved, AllowNull]
        private ILyricsChangeHandler lyricsChangeHandler { get; set; }

        [Resolved, AllowNull]
        private ILyricLanguageChangeHandler languageChangeHandler { get; set; }

        [Resolved, AllowNull]
        private ILyricRubyTagsChangeHandler lyricRubyTagsChangeHandler { get; set; }

        [Resolved, AllowNull]
        private ILyricRomajiTagsChangeHandler lyricRomajiTagsChangeHandler { get; set; }

        [Resolved, AllowNull]
        private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; }

        [Resolved, AllowNull]
        private ILyricSingerChangeHandler lyricSingerChangeHandler { get; set; }

        public bool Cut()
        {
            bool copied = Copy();
            if (!copied)
                return false;

            switch (state.Mode)
            {
                case LyricEditorMode.View:
                    return false;

                case LyricEditorMode.Manage:
                    lyricsChangeHandler.Remove();
                    return true;

                case LyricEditorMode.Typing:
                    // cut, copy or paste event should be handled in the caret.
                    return false;

                case LyricEditorMode.Language:
                    languageChangeHandler.SetLanguage(null);
                    return true;

                case LyricEditorMode.EditRuby:
                    var rubies = editRubyModeState.SelectedItems;
                    lyricRubyTagsChangeHandler.RemoveAll(rubies);
                    return true;

                case LyricEditorMode.EditRomaji:
                    var romajies = editRomajiModeState.SelectedItems;
                    lyricRomajiTagsChangeHandler.RemoveAll(romajies);
                    return true;

                case LyricEditorMode.EditTimeTag:
                    var timeTags = timeTagModeState.SelectedItems;
                    // todo: implement
                    // lyricTimeTagsChangeHandler.Clear();
                    return true;

                case LyricEditorMode.EditNote:
                    return false;

                case LyricEditorMode.Singer:
                    lyricSingerChangeHandler.Clear();
                    return true;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool Copy()
        {
            var selectedLyric = getSelectedLyric();
            if (selectedLyric == null)
                return false;

            // deserializer the lyric.
            var settings = KaraokeJsonSerializableExtensions.CreateGlobalSettings();
            string text = JsonConvert.SerializeObject(selectedLyric, settings);

            host.GetClipboard()?.SetText(text);

            return true;
        }

        public bool Paste()
        {
            string? text = host.GetClipboard()?.GetText();
            if (string.IsNullOrEmpty(text))
                return false;

            var selectedLyric = getSelectedLyric();
            if (selectedLyric == null)
                return false;

            switch (state.Mode)
            {
                case LyricEditorMode.View:
                    return false;

                case LyricEditorMode.Manage:
                    return false;

                case LyricEditorMode.Typing:
                    return false;

                case LyricEditorMode.Language:
                    return false;

                case LyricEditorMode.EditRuby:
                    return false;

                case LyricEditorMode.EditRomaji:
                    return false;

                case LyricEditorMode.EditTimeTag:
                    return false;

                case LyricEditorMode.EditNote:
                    return false;

                case LyricEditorMode.Singer:
                    return false;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
