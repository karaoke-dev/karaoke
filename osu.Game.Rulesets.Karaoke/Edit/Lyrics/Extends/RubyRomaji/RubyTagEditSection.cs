// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RubyTagEditSection : TextTagEditSection<RubyTag>
    {
        protected override string Title => "Ruby";

        [BackgroundDependencyLoader]
        private void load(LyricCaretState lyricCaretState)
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
            public LabelledRubyTagTextBox(RubyTag textTag)
                : base(textTag)
            {
            }

            [BackgroundDependencyLoader]
            private void load(BlueprintSelectionState blueprintSelectionState)
            {
                blueprintSelectionState.SelectedRubyTags.BindTo(SelectedTextTag);
            }
        }
    }
}
