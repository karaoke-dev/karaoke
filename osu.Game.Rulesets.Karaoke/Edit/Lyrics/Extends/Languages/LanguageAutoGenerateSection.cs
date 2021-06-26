// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages
{
    public class LanguageAutoGenerateSection : Section
    {
        protected override string Title => "Auto generate";

        [BackgroundDependencyLoader]
        private void load(LyricManager lyricManager, ILyricEditorState state)
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
                lyricManager.AutoDetectLyricLanguage(selectedLyric);
            };
        }
    }
}
