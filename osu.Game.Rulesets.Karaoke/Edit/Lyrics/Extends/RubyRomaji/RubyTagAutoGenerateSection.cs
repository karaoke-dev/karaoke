// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.RubyRomaji;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RubyTagAutoGenerateSection : Section
    {
        protected override string Title => "Auto generate";

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap beatmap, RubyRomajiManager rubyRomaji, LyricSelectionState lyricSelectionState)
        {
            Children = new[]
            {
                new AutoGenerateButton
                {
                    StartSelecting = () =>
                        beatmap.HitObjects.OfType<Lyric>().Where(x => x.Language == null)
                               .ToDictionary(k => k, i => "Before generate time-tag, need to assign language first.")
                },
            };

            lyricSelectionState.Action = e =>
            {
                if (e != LyricEditorSelectingAction.Apply)
                    return;

                var selectedLyric = lyricSelectionState.SelectedLyrics.ToList();
                rubyRomaji.AutoGenerateLyricRuby(selectedLyric);
            };
        }
    }
}
