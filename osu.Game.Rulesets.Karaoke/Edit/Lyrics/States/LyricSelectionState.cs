// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
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

        public BindableDictionary<Lyric, string> DisableSelectingLyric { get; } = new();

        public BindableList<Lyric> SelectedLyrics { get; } = new();

        public Action<LyricEditorSelectingAction> Action { get; set; }

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        private readonly BindableBool selecting = new();

        public void StartSelecting()
        {
            SelectedLyrics.Clear();
            selecting.Value = true;
        }

        public void EndSelecting(LyricEditorSelectingAction action)
        {
            selecting.Value = false;

            if (beatmap == null)
                return;

            // should sync selection to editor beatmap because auto-generate will be apply to those lyric that being selected.
            var selectedLyrics = SelectedLyrics.ToArray();
            beatmap.SelectedHitObjects.Clear();
            beatmap.SelectedHitObjects.AddRange(selectedLyrics);

            Action?.Invoke(action);

            // after being applied, should clear the selection.
            beatmap.SelectedHitObjects.Clear();
        }
    }
}
