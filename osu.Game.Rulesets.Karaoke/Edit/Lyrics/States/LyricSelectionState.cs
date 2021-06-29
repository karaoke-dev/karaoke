// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States
{
    public class LyricSelectionState
    {
        public BindableBool Selecting { get; } = new BindableBool();

        public BindableList<Lyric> DisableSelectingLyric { get; } = new BindableList<Lyric>();

        public BindableList<Lyric> SelectedLyrics { get; } = new BindableList<Lyric>();

        public Action<LyricEditorSelectingAction> Action { get; set; }

        public void StartSelecting()
        {
            SelectedLyrics.Clear();
            DisableSelectingLyric.Clear();
            Selecting.Value = true;
        }

        public void EndSelecting(LyricEditorSelectingAction action)
        {
            Selecting.Value = false;
            DisableSelectingLyric.Clear();
            Action?.Invoke(action);
        }
    }
}
