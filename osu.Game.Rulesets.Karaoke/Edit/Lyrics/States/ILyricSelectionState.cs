// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States
{
    public interface ILyricSelectionState
    {
        IBindable<bool> Selecting { get; }

        BindableDictionary<Lyric, string> DisableSelectingLyric { get; }

        BindableList<Lyric> SelectedLyrics { get; }

        Action<LyricEditorSelectingAction> Action { get; set; }

        void StartSelecting();

        void EndSelecting(LyricEditorSelectingAction action);
    }
}
