// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Localisation;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components.Description
{
    public struct DescriptionFormat
    {
        public LocalisableString Text { get; set; }

        public IDictionary<string, InputKey> Keys { get; set; }
    }

    public struct InputKey
    {
        public LocalisableString Text { get; set; }

        public IList<KaraokeEditAction> AdjustableActions { get; set; }
    }
}
