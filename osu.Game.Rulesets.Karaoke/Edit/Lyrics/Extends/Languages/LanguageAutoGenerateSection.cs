// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages
{
    public class LanguageAutoGenerateSection : Section
    {
        protected override string Title => "Auto generate";

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap beatmap, LyricManager lyricManager, LyricSelectionState lyricSelectionState)
        {
            Children = new[]
            {
                new AutoGenerateButton
                {
                    StartSelecting = () =>
                        beatmap.HitObjects.OfType<Lyric>().Where(x => !string.IsNullOrEmpty(x.Text))
                               .ToDictionary(k => k, i => "Should have text in lyric.")
                },
            };

            lyricSelectionState.Action = e =>
            {
                if (e != LyricEditorSelectingAction.Apply)
                    return;

                var selectedLyric = lyricSelectionState.SelectedLyrics.ToList();
                lyricManager.AutoDetectLyricLanguage(selectedLyric);
            };
        }
    }
}
