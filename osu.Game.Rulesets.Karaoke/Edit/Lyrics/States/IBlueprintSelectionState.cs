// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States
{
    public interface IBlueprintSelectionState
    {
        BindableList<TimeTag> SelectedTimeTags { get; }

        BindableList<RubyTag> SelectedRubyTags { get; }

        BindableList<RomajiTag> SelectedRomajiTags { get; }

        BindableList<Note> SelectedNotes { get; }

        void ClearSelectedTimeTags();

        void ClearSelectedTextTags();
    }
}
