// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RomajiTagEditSection : TextTagEditSection<RomajiTag>
    {
        protected override string Title => "Romaji";

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state)
        {
            state.BindableCaretPosition.BindValueChanged(e =>
            {
                Lyric = e.NewValue?.Lyric;

                if (e.OldValue?.Lyric != null)
                {
                    TextTags.UnbindFrom(e.OldValue.Lyric.RomajiTagsBindable);
                }

                if (e.NewValue?.Lyric != null)
                {
                    TextTags.BindTo(e.NewValue.Lyric.RomajiTagsBindable);
                }
            });
        }
    }
}
