// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Languages
{
    public class LanguagesChangeHandler : BeatmapChangeHandler<CultureInfo>, ILanguagesChangeHandler
    {
        public BindableList<CultureInfo> Languages { get; } = new();

        private IEnumerable<Lyric> lyrics => Beatmap.HitObjects.OfType<Lyric>();

        [BackgroundDependencyLoader]
        private void load()
        {
            Languages.AddRange(Beatmap.AvailableTranslates);
            Languages.BindCollectionChanged((_, _) => { Beatmap.AvailableTranslates = Languages.ToArray(); });
        }

        public override void Add(CultureInfo item)
        {
            if (Languages.Contains(item))
                return;

            PerformObjectChanged(item, cultureInfo =>
            {
                Languages.Add(cultureInfo);
            });
        }

        public override void Remove(CultureInfo item)
        {
            if (!Languages.Contains(item))
                throw new InvalidOperationException($"{nameof(item)} is not in the list");

            PerformObjectChanged(item, cultureInfo =>
            {
                // Delete from list.
                Languages.Remove(cultureInfo);

                // Delete from lyric also.
                foreach (var lyric in lyrics.Where(lyric => lyric.Translates.ContainsKey(cultureInfo)))
                {
                    lyric.Translates.Remove(cultureInfo);
                }
            });
        }

        public bool IsLanguageContainsTranslate(CultureInfo cultureInfo)
            => lyrics.Any(x => x.Translates.ContainsKey(cultureInfo));
    }
}
