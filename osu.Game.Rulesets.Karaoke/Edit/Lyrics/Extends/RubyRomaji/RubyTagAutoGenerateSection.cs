// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.RubyRomaji;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RubyTagAutoGenerateSection : Section
    {
        protected override string Title => "Auto generate";

        [BackgroundDependencyLoader]
        private void load(RubyRomajiManager rubyRomaji, ILyricEditorState state)
        {
            Children = new[]
            {
                new AutoGenerateButton(),
            };

            state.Action = e =>
            {
                if (e != LyricEditorSelectingAction.Apply)
                    return;

                var selectedLyric = state.SelectedLyrics.ToList();
                rubyRomaji.AutoGenerateLyricRuby(selectedLyric);
            };
        }
    }
}
