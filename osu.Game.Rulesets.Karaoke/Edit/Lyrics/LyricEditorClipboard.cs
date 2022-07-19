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

        public void Cut()
        {
            Copy();

            switch (state.Mode)
            {
                case LyricEditorMode.View:
                    break;

                case LyricEditorMode.Manage:
                    lyricsChangeHandler.Remove();
                    break;

                case LyricEditorMode.Typing:
                    // cut, copy or paste event should be handled in the caret.
                    break;

                case LyricEditorMode.Language:
                    languageChangeHandler.SetLanguage(null);
                    break;

                case LyricEditorMode.EditRuby:
                    var rubies = editRubyModeState.SelectedItems;
                    lyricRubyTagsChangeHandler.RemoveAll(rubies);
                    break;

                case LyricEditorMode.EditRomaji:
                    var romajies = editRomajiModeState.SelectedItems;
                    lyricRomajiTagsChangeHandler.RemoveAll(romajies);
                    break;

                case LyricEditorMode.EditTimeTag:
                    var timeTags = timeTagModeState.SelectedItems;
                    // todo: implement
                    // lyricTimeTagsChangeHandler.Clear();
                    break;

                case LyricEditorMode.EditNote:

                    break;

                case LyricEditorMode.Singer:
                    lyricSingerChangeHandler.Clear();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Copy()
        {
            var selectedLyric = getSelectedLyric();
            if (selectedLyric == null)
                return;

            // deserializer the lyric.
            var settings = KaraokeJsonSerializableExtensions.CreateGlobalSettings();
            string text = JsonConvert.SerializeObject(selectedLyric, settings);

            host.GetClipboard()?.SetText(text);
        }

        public void Paste()
        {
            string? text = host.GetClipboard()?.GetText();
            if (string.IsNullOrEmpty(text))
                return;

            var selectedLyric = getSelectedLyric();
            if (selectedLyric == null)
                return;

            switch (state.Mode)
            {
                case LyricEditorMode.View:
                    return;

                case LyricEditorMode.Manage:
                    break;

                case LyricEditorMode.Typing:
                    break;

                case LyricEditorMode.Language:
                    break;

                case LyricEditorMode.EditRuby:
                    break;

                case LyricEditorMode.EditRomaji:
                    break;

                case LyricEditorMode.EditTimeTag:
                    break;

                case LyricEditorMode.EditNote:
                    break;

                case LyricEditorMode.Singer:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
