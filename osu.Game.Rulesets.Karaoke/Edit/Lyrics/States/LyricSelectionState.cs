// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States
{
    public class LyricSelectionState : Component, ILyricSelectionState
    {
        public IBindable<bool> Selecting => selecting;

        private readonly BindableDictionary<Lyric, string> bindableDisableSelectingLyric = new();
        private readonly BindableList<Lyric> bindableSelectedLyrics = new();

        public IBindableDictionary<Lyric, string> DisableSelectingLyric => bindableDisableSelectingLyric;

        public IBindableList<Lyric> SelectedLyrics => bindableSelectedLyrics;

        public Action<LyricEditorSelectingAction> Action { get; set; }

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [Resolved]
        private ILyricCaretState lyricCaretState { get; set; }

        private readonly BindableBool selecting = new();

        public void StartSelecting()
        {
            bindableSelectedLyrics.Clear();
            selecting.Value = true;
        }

        public void EndSelecting(LyricEditorSelectingAction action)
        {
            if (selecting.Value == false)
                return;

            selecting.Value = false;

            if (beatmap == null)
                return;

            // should sync selection to editor beatmap because auto-generate will be apply to those lyric that being selected.
            var selectedLyrics = bindableSelectedLyrics.ToArray();
            beatmap.SelectedHitObjects.Clear();
            beatmap.SelectedHitObjects.AddRange(selectedLyrics);

            Action?.Invoke(action);

            // after being applied, should clear the selection.
            beatmap.SelectedHitObjects.Clear();

            // should add selected lyric back.
            var caretPosition = lyricCaretState.BindableCaretPosition.Value;
            if (caretPosition?.Lyric != null)
                beatmap.SelectedHitObjects.Add(caretPosition.Lyric);
        }

        public void Select(Lyric lyric)
        {
            if (bindableSelectedLyrics.Contains(lyric))
                return;

            bindableSelectedLyrics.Add(lyric);
        }

        public void UnSelect(Lyric lyric)
        {
            bindableSelectedLyrics.Remove(lyric);
        }

        public void SelectAll()
        {
            var disableSelectingLyrics = bindableDisableSelectingLyric.Keys;
            var lyrics = beatmap.HitObjects.OfType<Lyric>().Where(x => !disableSelectingLyrics.Contains(x));

            foreach (var lyric in lyrics)
            {
                Select(lyric);
            }
        }

        public void UnSelectAll()
        {
            bindableSelectedLyrics.Clear();
        }

        public void UpdateDisableLyricList(IDictionary<Lyric, string> disableLyrics)
        {
            bindableDisableSelectingLyric.Clear();

            if (disableLyrics == null)
                return;

            foreach ((var lyric, string reason) in disableLyrics)
                bindableDisableSelectingLyric.Add(lyric, reason);
        }
    }
}
