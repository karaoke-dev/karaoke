// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Languages
{
    public class LanguagesChangeHandler : BeatmapChangeHandler<CultureInfo>
    {
        private readonly BindableList<CultureInfo> languages = new();

        [BackgroundDependencyLoader]
        private void load()
        {
            languages.AddRange(Beatmap.AvailableTranslates);
            languages.BindCollectionChanged((_, _) => { Beatmap.AvailableTranslates = languages.ToArray(); });
        }

        public override void Add(CultureInfo item)
        {
            if (languages.Contains(item))
                return;

            PerformObjectChanged(item, cultureInfo =>
            {
                languages.Add(cultureInfo);
            });
        }

        public override void Remove(CultureInfo item)
        {
            if (!languages.Contains(item))
                throw new InvalidOperationException($"{nameof(item)} is not in the list");

            PerformObjectChanged(item, cultureInfo =>
            {
                // Delete from list.
                languages.Remove(cultureInfo);

                // Delete from lyric also.
                var lyrics = Beatmap.HitObjects.OfType<Lyric>().ToList();

                foreach (var lyric in lyrics.Where(lyric => lyric.Translates.ContainsKey(cultureInfo)))
                {
                    lyric.Translates.Remove(cultureInfo);
                }
            });
        }
    }
}
