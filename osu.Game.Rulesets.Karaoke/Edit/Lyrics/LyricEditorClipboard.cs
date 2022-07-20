// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
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

        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();

        // we should save the serialized lyric object into here instead of save into the clipboard for some reason:
        // 1. It's hard to know which ruby/romaji or time-tag being copied.
        // 2. Maybe user did not want to copy the full json content?
        private string clipboardContent = string.Empty;

        public LyricEditorClipboard()
        {
            bindableMode.BindValueChanged(_ =>
            {
                clipboardContent = string.Empty;
            });
        }

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state)
        {
            bindableMode.BindTo(state.BindableMode);
        }

        public bool Cut()
        {
            var selectedLyric = getSelectedLyric();
            if (selectedLyric == null)
                return false;

            bool copied = performCopy(selectedLyric);
            if (!copied)
                return false;

            bool cut = performCut();
            if (!cut)
                return false;

            // todo: show the toast.
            return true;
        }

        public bool Copy()
        {
            var selectedLyric = getSelectedLyric();
            if (selectedLyric == null)
                return false;

            var settings = KaraokeJsonSerializableExtensions.CreateGlobalSettings();
            clipboardContent = JsonConvert.SerializeObject(selectedLyric, settings);

            bool copy = performCopy(selectedLyric);
            if (!copy)
                return false;

            // todo: show the toast.
            return true;
        }

        public bool Paste()
        {
            if (string.IsNullOrEmpty(clipboardContent))
                return false;

            var selectedLyric = getSelectedLyric();
            if (selectedLyric == null)
                return false;

            bool paste = performPaste(selectedLyric);
            if (!paste)
                return false;

            // todo: show the toast.
            return true;
        }

        #region logic

        private bool performCut()
        {
            switch (bindableMode.Value)
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
                    lyricTimeTagsChangeHandler.RemoveAll(timeTags);
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

        private bool performCopy(Lyric lyric)
        {
            switch (bindableMode.Value)
            {
                case LyricEditorMode.View:
                    copyObjectToClipboard(lyric.Text);
                    break;

                case LyricEditorMode.Manage:
                    copyObjectToClipboard(lyric.Text);
                    break;

                case LyricEditorMode.Typing:
                    // cut, copy or paste event should be handled in the caret.
                    return false;

                case LyricEditorMode.Language:
                    copyObjectToClipboard(lyric.Language);
                    break;

                case LyricEditorMode.EditRuby:
                    copyObjectToClipboard(editRubyModeState.SelectedItems);
                    break;

                case LyricEditorMode.EditRomaji:
                    copyObjectToClipboard(editRomajiModeState.SelectedItems);
                    break;

                case LyricEditorMode.EditTimeTag:
                    copyObjectToClipboard(timeTagModeState.SelectedItems);
                    break;

                case LyricEditorMode.EditNote:
                    return false;

                case LyricEditorMode.Singer:
                    // todo: should get all singer infos.
                    copyObjectToClipboard(lyric.Singers);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return true;
        }

        private bool performPaste(Lyric lyric)
        {
            switch (bindableMode.Value)
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

        private void copyObjectToClipboard<T>(T obj)
        {
            var settings = KaraokeJsonSerializableExtensions.CreateGlobalSettings();
            string text = JsonConvert.SerializeObject(obj, settings);

            host.GetClipboard()?.SetText(text);
        }

        private T? getObjectFromClipboard<T>()
        {
            string? text = host.GetClipboard()?.GetText();
            if (string.IsNullOrEmpty(text))
                return default;

            var settings = KaraokeJsonSerializableExtensions.CreateGlobalSettings();
            return JsonConvert.DeserializeObject<T>(text, settings);
        }

        #endregion
    }
}
