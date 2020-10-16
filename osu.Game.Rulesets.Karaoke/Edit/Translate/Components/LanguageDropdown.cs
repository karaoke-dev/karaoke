// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate.Components
{
    public class LanguageDropdown : OsuDropdown<BeatmapSetOnlineLanguage>
    {
        protected override string GenerateItemText(BeatmapSetOnlineLanguage item)
            => item.Name;
    }
}
