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
            state.BindableCaretPosition.BindValueChanged(x =>
            {
                Lyric = x.NewValue.Lyric;
                var romajiTags = Lyric?.RomajiTags;
                TextTags.Clear();
                if (romajiTags != null)
                    TextTags.AddRange(romajiTags);
            });
        }
    }
}
