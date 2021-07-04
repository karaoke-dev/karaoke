// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components
{
    public abstract class AutoGenerateSection : Section
    {
        protected sealed override string Title => "Auto generate";

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap beatmap, LyricSelectionState lyricSelectionState)
        {
            Children = new[]
            {
                new AutoGenerateButton
                {
                    StartSelecting = () =>
                        GetDisableSelectingLyrics(beatmap.HitObjects.OfType<Lyric>().ToArray())
                },
            };

            lyricSelectionState.Action = e =>
            {
                if (e != LyricEditorSelectingAction.Apply)
                    return;

                var selectedLyrics = lyricSelectionState.SelectedLyrics.ToArray();
                Apply(selectedLyrics);
            };
        }

        protected abstract Dictionary<Lyric, string> GetDisableSelectingLyrics(Lyric[] lyrics);

        protected abstract void Apply(Lyric[] lyrics);

        private class AutoGenerateButton : SelectLyricButton
        {
            protected override string StandardText => "Generate";

            protected override string SelectingText => "Cancel generate";
        }
    }
}
