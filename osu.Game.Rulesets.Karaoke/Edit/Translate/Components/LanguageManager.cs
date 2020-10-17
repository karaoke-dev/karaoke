// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate.Components
{
    /// <summary>
    /// Handle create or delete language.
    /// </summary>
    public class LanguageManager : Component
    {
        public readonly BindableList<BeatmapSetOnlineLanguage> Languages = new BindableList<BeatmapSetOnlineLanguage>();

        public LanguageManager(EditorBeatmap beatmap)
        {
            // todo : 
        }

        public void AddLanguage(BeatmapSetOnlineLanguage language)
        {

        }

        public void RemoveLanguage(BeatmapSetOnlineLanguage language)
        {

        }

        public bool IsLanguageContaineTranslate(BeatmapSetOnlineLanguage language)
            => LanguageContaineTranslateAmount(language) > 0;

        public int LanguageContaineTranslateAmount(BeatmapSetOnlineLanguage language)
        {
            return 10;
        }
    }
}
