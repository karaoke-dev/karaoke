// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States
{
    public class BlueprintSelectionState
    {
        public BindableList<TimeTag> SelectedTimeTags { get; } = new();

        public BindableList<RubyTag> SelectedRubyTags { get; } = new();

        public BindableList<RomajiTag> SelectedRomajiTags { get; } = new();

        public BindableList<Note> SelectedNotes { get; } = new();

        public void ClearSelectedTimeTags()
        {
            SelectedTimeTags.Clear();
        }

        public void ClearSelectedTextTags()
        {
            SelectedRubyTags.Clear();
            SelectedRomajiTags.Clear();
        }
    }
}
