// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States
{
    public class BlueprintSelectionState
    {
        public BindableList<TimeTag> SelectedTimeTags { get; } = new BindableList<TimeTag>();

        public BindableList<RubyTag> SelectedRubyTags { get; } = new BindableList<RubyTag>();

        public BindableList<RomajiTag> SelectedRomajiTags { get; } = new BindableList<RomajiTag>();

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
