// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate
{
    /// <summary>
    /// Handle create or delete language.
    /// </summary>
    public class TranslateManager : Component
    {
        public readonly BindableList<CultureInfo> Languages = new BindableList<CultureInfo>();

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

        public void AddLanguage(CultureInfo language)
        {
            Languages.Add(language);
        }

        public void RemoveLanguage(CultureInfo cultureInfo)
        {
            Languages.Remove(cultureInfo);
        }

        public bool IsLanguageContainsTranslate(CultureInfo cultureInfo)
            => LanguageContainsTranslateAmount(cultureInfo) > 0;

        public int LanguageContainsTranslateAmount(CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
                return 0;

            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();
            return lyrics.Count(x => x.Translates.ContainsKey(cultureInfo));
        }
    }
}
