// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

public partial class BeatmapTranslationsChangeHandler : BeatmapListPropertyChangeHandler<CultureInfo>, IBeatmapTranslationsChangeHandler
{
    public IBindableList<CultureInfo> Languages => Items;

    protected override IList<CultureInfo> GetItemsFromBeatmap(KaraokeBeatmap beatmap)
        => beatmap.AvailableTranslationLanguages;

    protected override void OnItemAdded(CultureInfo item)
    {
        // there's no need to do anything.
    }

    protected override void OnItemRemoved(CultureInfo item)
    {
        // Delete from lyric also.
        foreach (var lyric in Lyrics.Where(lyric => lyric.Translations.ContainsKey(item)))
        {
            lyric.Translations.Remove(item);
        }
    }

    public bool IsLanguageContainsTranslation(CultureInfo cultureInfo)
        => Lyrics.Any(x => x.Translations.ContainsKey(cultureInfo));
}
