// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RubyTagEditSection : TextTagEditSection<RubyTag>
    {
        protected override string Title => "Ruby";

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state)
        {
            state.BindableCaretPosition.BindValueChanged(e =>
            {
                Lyric = e.NewValue.Lyric;
                var rubyTags = Lyric?.RubyTags;
                TextTags.Clear();
                if (rubyTags != null)
                    TextTags.AddRange(rubyTags);
            });
        }
    }
}
