// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate
{
    /// <summary>
    /// Handle creating or deleting languages.
    /// </summary>
    public class LanguageManager : Component
    {
        public readonly BindableList<BeatmapSetOnlineLanguage> Languages = new BindableList<BeatmapSetOnlineLanguage>();

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            if (beatmap?.PlayableBeatmap is KaraokeBeatmap karaokeBeatmap)
            {
                Languages.AddRange(karaokeBeatmap.AvailableTranslates);
                Languages.BindCollectionChanged((a, b) => { karaokeBeatmap.AvailableTranslates = Languages.ToArray(); });
            }
        }

        public void AddLanguage(BeatmapSetOnlineLanguage language)
        {
            Languages.Add(language);
        }

        public void UpdateLanguageName(BeatmapSetOnlineLanguage language, string name)
        {
            language.Name = name;
        }

        public void RemoveLanguage(BeatmapSetOnlineLanguage language)
        {
            Languages.Remove(language);
        }

        public bool IsLanguageContainsTranslate(BeatmapSetOnlineLanguage language)
            => LanguageContainsTranslateAmount(language) > 0;

        public int LanguageContainsTranslateAmount(BeatmapSetOnlineLanguage language)
        {
            if (language == null)
                return 0;

            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();
            return lyrics.Count(x => x.Translates.ContainsKey(language.Id));
        }
    }
}
