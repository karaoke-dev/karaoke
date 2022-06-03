// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States
{
    public interface ILyricSelectionState
    {
        IBindable<bool> Selecting { get; }

        IBindableDictionary<Lyric, LocalisableString> DisableSelectingLyric { get; }

        IBindableList<Lyric> SelectedLyrics { get; }

        Action<LyricEditorSelectingAction> Action { get; set; }

        void StartSelecting();

        void EndSelecting(LyricEditorSelectingAction action);

        void Select(Lyric lyric);

        void UnSelect(Lyric lyric);

        void SelectAll();

        void UnSelectAll();

        void UpdateDisableLyricList(IDictionary<Lyric, LocalisableString> disableLyrics);
    }
}
