// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

public interface IBeatmapPagesChangeHandler : IAutoGenerateChangeHandler
{
    LocalisableString? GetGeneratorNotSupportedMessage();

    void Add(Page page);

    void Remove(Page page);

    void RemoveRange(IEnumerable<Page> pages);

    void ShiftingPageTime(IEnumerable<Page> pages, double offset);
}
