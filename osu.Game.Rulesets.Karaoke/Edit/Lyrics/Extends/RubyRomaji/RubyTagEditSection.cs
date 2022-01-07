// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RubyTagEditSection : TextTagEditSection<RubyTag>
    {
        protected override string Title => "Ruby";

        [BackgroundDependencyLoader]
        private void load(ILyricCaretState lyricCaretState)
        {
            lyricCaretState.BindableCaretPosition.BindValueChanged(e =>
            {
                Lyric = e.NewValue?.Lyric;

                if (e.OldValue?.Lyric != null)
                {
                    TextTags.UnbindFrom(e.OldValue.Lyric.RubyTagsBindable);
                }

                if (e.NewValue?.Lyric != null)
                {
                    TextTags.BindTo(e.NewValue.Lyric.RubyTagsBindable);
                }
            }, true);
        }

        protected override LabelledTextTagTextBox<RubyTag> CreateLabelledTextTagTextBox(RubyTag textTag)
            => new LabelledRubyTagTextBox(textTag);

        protected class LabelledRubyTagTextBox : LabelledTextTagTextBox<RubyTag>
        {
            [Resolved]
            private ILyricRubyTagsChangeHandler rubyTagsChangeHandler { get; set; }

            public LabelledRubyTagTextBox(RubyTag textTag)
                : base(textTag)
            {
            }

            protected override void SetText(RubyTag item, string value)
            {
                rubyTagsChangeHandler.SetText(item, value);
            }

            [BackgroundDependencyLoader]
            private void load(IBlueprintSelectionState blueprintSelectionState)
            {
                blueprintSelectionState.SelectedRubyTags.BindTo(SelectedItems);
            }
        }
    }
}
