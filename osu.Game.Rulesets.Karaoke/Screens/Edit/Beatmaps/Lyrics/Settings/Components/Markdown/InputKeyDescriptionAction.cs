// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Localisation;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;

public struct InputKeyDescriptionAction : IDescriptionAction
{
    public LocalisableString Text { get; set; }

    public IList<KaraokeEditAction> AdjustableActions { get; set; }
}
