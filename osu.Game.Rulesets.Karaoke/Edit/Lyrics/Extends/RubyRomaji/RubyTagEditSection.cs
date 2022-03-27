// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RubyTagEditSection : TextTagEditSection<RubyTag>
    {
        protected override string Title => "Ruby";

        [Resolved]
        private ILyricRubyTagsChangeHandler rubyTagsChangeHandler { get; set; }

        protected override IBindableList<RubyTag> GetBindableTextTags(Lyric lyric)
            => lyric.RubyTagsBindable;

        protected override LabelledTextTagTextBox<RubyTag> CreateLabelledTextTagTextBox(RubyTag textTag)
            => new LabelledRubyTagTextBox(Lyric, textTag);

        protected override void AddTextTag(RubyTag textTag)
            => rubyTagsChangeHandler.Add(textTag);

        protected class LabelledRubyTagTextBox : LabelledTextTagTextBox<RubyTag>
        {
            [Resolved]
            private ILyricRubyTagsChangeHandler rubyTagsChangeHandler { get; set; }

            public LabelledRubyTagTextBox(Lyric lyric, RubyTag textTag)
                : base(lyric, textTag)
            {
                Debug.Assert(lyric.RubyTags.Contains(textTag));
            }

            protected override void ApplyValue(RubyTag item, string value)
                => rubyTagsChangeHandler.SetText(item, value);

            protected override void SetIndex(RubyTag item, int? startIndex, int? endIndex)
                => rubyTagsChangeHandler.SetIndex(item, startIndex, endIndex);

            protected override void RemoveTextTag(RubyTag textTag)
                => rubyTagsChangeHandler.Remove(textTag);

            [BackgroundDependencyLoader]
            private void load(IEditRubyModeState editRubyModeState)
            {
                SelectedItems.BindTo(editRubyModeState.SelectedItems);
            }
        }
    }
}
