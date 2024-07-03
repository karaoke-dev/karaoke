// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

public partial class BeatmapLanguagesChangeHandler : BeatmapListPropertyChangeHandler<CultureInfo>, IBeatmapLanguagesChangeHandler
{
    public IBindableList<CultureInfo> Languages => Items;

    protected override IList<CultureInfo> GetItemsFromBeatmap(KaraokeBeatmap beatmap)
        => beatmap.AvailableTranslates;

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

    public bool IsLanguageContainsTranslate(CultureInfo cultureInfo)
        => Lyrics.Any(x => x.Translations.ContainsKey(cultureInfo));
}
