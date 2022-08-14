// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Singers;
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

        [Resolved]
        private OnScreenDisplay? onScreenDisplay { get; set; }

        [Resolved, AllowNull]
        private ILyricCaretState lyricCaretState { get; set; }

        private Lyric? getSelectedLyric() => lyricCaretState.BindableCaretPosition.Value?.Lyric;

        [Resolved, AllowNull]
        private ITextingModeState textingModeState { get; set; }

        [Resolved, AllowNull]
        private IEditRubyModeState editRubyModeState { get; set; }

        [Resolved, AllowNull]
        private IEditRomajiModeState editRomajiModeState { get; set; }

        [Resolved, AllowNull]
        private ITimeTagModeState timeTagModeState { get; set; }

        [Resolved]
        private ILyricsChangeHandler? lyricsChangeHandler { get; set; }

        [Resolved]
        private ILyricLanguageChangeHandler? languageChangeHandler { get; set; }

        [Resolved]
        private ILyricRubyTagsChangeHandler? lyricRubyTagsChangeHandler { get; set; }

        [Resolved]
        private ILyricRomajiTagsChangeHandler? lyricRomajiTagsChangeHandler { get; set; }

        [Resolved]
        private ILyricTimeTagsChangeHandler? lyricTimeTagsChangeHandler { get; set; }

        [Resolved]
        private ILyricSingerChangeHandler? lyricSingerChangeHandler { get; set; }

        [Resolved]
        private ISingersChangeHandler? singersChangeHandler { get; set; }

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

            onScreenDisplay?.Display(new ClipboardToast(bindableMode.Value, ClipboardAction.Cut));
            return true;
        }

        public bool Copy()
        {
            var selectedLyric = getSelectedLyric();
            if (selectedLyric == null)
                return false;

            bool copy = performCopy(selectedLyric);
            if (!copy)
                return false;

            onScreenDisplay?.Display(new ClipboardToast(bindableMode.Value, ClipboardAction.Copy));
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

            onScreenDisplay?.Display(new ClipboardToast(bindableMode.Value, ClipboardAction.Paste));
            return true;
        }

        #region logic

        private bool performCut()
        {
            switch (bindableMode.Value)
            {
                case LyricEditorMode.View:
                    return false;

                case LyricEditorMode.Texting:
                    switch (textingModeState.EditMode)
                    {
                        case TextingEditMode.Typing:
                            // cut, copy or paste event should be handled in the caret.
                            return false;

                        case TextingEditMode.Split:
                            if (lyricsChangeHandler == null)
                                throw new NullDependencyException($"Missing {nameof(lyricsChangeHandler)}");

                            lyricsChangeHandler.Remove();
                            return true;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                case LyricEditorMode.Reference:
                    return false;

                case LyricEditorMode.Language:
                    languageChangeHandler?.SetLanguage(null);
                    return true;

                case LyricEditorMode.EditRuby:
                    var rubies = editRubyModeState.SelectedItems;
                    if (!rubies.Any())
                        return false;

                    if (lyricRubyTagsChangeHandler == null)
                        throw new NullDependencyException($"Missing {nameof(lyricRubyTagsChangeHandler)}");

                    lyricRubyTagsChangeHandler.RemoveRange(rubies);
                    return true;

                case LyricEditorMode.EditRomaji:
                    var romajies = editRomajiModeState.SelectedItems;
                    if (!romajies.Any())
                        return false;

                    if (lyricRomajiTagsChangeHandler == null)
                        throw new NullDependencyException($"Missing {nameof(lyricRomajiTagsChangeHandler)}");

                    lyricRomajiTagsChangeHandler.RemoveRange(romajies);
                    return true;

                case LyricEditorMode.EditTimeTag:
                    var timeTags = timeTagModeState.SelectedItems;
                    if (!timeTags.Any())
                        return false;

                    if (lyricTimeTagsChangeHandler == null)
                        throw new NullDependencyException($"Missing {nameof(lyricTimeTagsChangeHandler)}");

                    lyricTimeTagsChangeHandler.RemoveRange(timeTags);
                    return true;

                case LyricEditorMode.EditNote:
                    return false;

                case LyricEditorMode.Singer:
                    if (lyricSingerChangeHandler == null)
                        throw new NullDependencyException($"Missing {nameof(lyricSingerChangeHandler)}");

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
                    // for letting user to copy the lyric as plain text.
                    copyObjectToClipboard(lyric.Text);
                    return true;

                case LyricEditorMode.Texting:
                    switch (textingModeState.EditMode)
                    {
                        case TextingEditMode.Typing:
                            // cut, copy or paste event should be handled in the caret.
                            return false;

                        case TextingEditMode.Split:
                            saveObjectToTheClipboardContent(lyric);
                            copyObjectToClipboard(lyric.Text);
                            return true;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                case LyricEditorMode.Reference:
                    return false;

                case LyricEditorMode.Language:
                    saveObjectToTheClipboardContent(lyric.Language);
                    copyObjectToClipboard(lyric.Language);
                    return true;

                case LyricEditorMode.EditRuby:
                    var rubies = editRubyModeState.SelectedItems;
                    if (!rubies.Any())
                        return false;

                    saveObjectToTheClipboardContent(rubies);
                    copyObjectToClipboard(rubies);
                    return true;

                case LyricEditorMode.EditRomaji:
                    var romajies = editRomajiModeState.SelectedItems;
                    if (!romajies.Any())
                        return false;

                    saveObjectToTheClipboardContent(romajies);
                    copyObjectToClipboard(romajies);
                    return true;

                case LyricEditorMode.EditTimeTag:
                    var timeTags = timeTagModeState.SelectedItems;
                    if (!timeTags.Any())
                        return false;

                    saveObjectToTheClipboardContent(timeTags);
                    copyObjectToClipboard(timeTags);
                    return true;

                case LyricEditorMode.EditNote:
                    return false;

                case LyricEditorMode.Singer:
                    saveObjectToTheClipboardContent(lyric.Singers);
                    var singers = getMatchedSinges(lyric.Singers);
                    copyObjectToClipboard(singers);
                    return true;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool performPaste(Lyric lyric)
        {
            switch (bindableMode.Value)
            {
                case LyricEditorMode.View:
                    return false;

                case LyricEditorMode.Texting:
                    switch (textingModeState.EditMode)
                    {
                        case TextingEditMode.Typing:
                            // cut, copy or paste event should be handled in the caret.
                            return false;

                        case TextingEditMode.Split:
                            var pasteLyric = getObjectFromClipboardContent<Lyric>();
                            if (pasteLyric == null)
                                return false;

                            if (lyricsChangeHandler == null)
                                throw new NullDependencyException($"Missing {nameof(lyricsChangeHandler)}");

                            lyricsChangeHandler.AddBelowToSelection(pasteLyric);
                            return true;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                case LyricEditorMode.Reference:
                    return false;

                case LyricEditorMode.Language:
                    var pasteLanguage = getObjectFromClipboardContent<CultureInfo>();
                    if (pasteLanguage == null)
                        return false;

                    if (languageChangeHandler == null)
                        throw new NullDependencyException($"Missing {nameof(languageChangeHandler)}");

                    languageChangeHandler.SetLanguage(pasteLanguage);
                    return true;

                case LyricEditorMode.EditRuby:
                    var pasteRubies = getObjectFromClipboardContent<RubyTag[]>();
                    if (pasteRubies == null)
                        return false;

                    if (lyricRubyTagsChangeHandler == null)
                        throw new NullDependencyException($"Missing {nameof(lyricRubyTagsChangeHandler)}");

                    lyricRubyTagsChangeHandler.AddRange(pasteRubies);
                    return true;

                case LyricEditorMode.EditRomaji:
                    var pasteRomajies = getObjectFromClipboardContent<RomajiTag[]>();
                    if (pasteRomajies == null)
                        return false;

                    if (lyricRomajiTagsChangeHandler == null)
                        throw new NullDependencyException($"Missing {nameof(lyricRomajiTagsChangeHandler)}");

                    lyricRomajiTagsChangeHandler.AddRange(pasteRomajies);
                    return true;

                case LyricEditorMode.EditTimeTag:
                    var pasteTimeTags = getObjectFromClipboardContent<TimeTag[]>();
                    if (pasteTimeTags == null)
                        return false;

                    if (lyricTimeTagsChangeHandler == null)
                        throw new NullDependencyException($"Missing {nameof(lyricTimeTagsChangeHandler)}");

                    lyricTimeTagsChangeHandler.AddRange(pasteTimeTags);
                    return true;

                case LyricEditorMode.EditNote:
                    return false;

                case LyricEditorMode.Singer:
                    int[]? pasteSingerIds = getObjectFromClipboardContent<int[]>();
                    if (pasteSingerIds == null)
                        return false;

                    if (lyricSingerChangeHandler == null)
                        throw new NullDependencyException($"Missing {nameof(lyricSingerChangeHandler)}");

                    var singers = getMatchedSinges(pasteSingerIds);
                    lyricSingerChangeHandler.AddRange(singers);
                    return true;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerable<Singer> getMatchedSinges(IEnumerable<int> singerIds)
        {
            return singersChangeHandler == null ? throw new NullDependencyException($"Missing {nameof(singersChangeHandler)}") : singersChangeHandler.Singers.Where(x => singerIds.Contains(x.ID));
        }

        private void copyObjectToClipboard<T>(T obj)
        {
            var settings = KaraokeJsonSerializableExtensions.CreateGlobalSettings();
            string text = JsonConvert.SerializeObject(obj, settings);

            host.GetClipboard()?.SetText(text);
        }

        private void saveObjectToTheClipboardContent<T>(T obj)
        {
            var settings = KaraokeJsonSerializableExtensions.CreateGlobalSettings();
            clipboardContent = JsonConvert.SerializeObject(obj, settings);
        }

        private T? getObjectFromClipboardContent<T>()
        {
            if (string.IsNullOrEmpty(clipboardContent))
                return default;

            var settings = KaraokeJsonSerializableExtensions.CreateGlobalSettings();
            return JsonConvert.DeserializeObject<T>(clipboardContent, settings);
        }

        #endregion
    }
}
