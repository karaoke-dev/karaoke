// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Localisation;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown
{
    public struct DescriptionFormat
    {
        public const string LINK_KEY_ACTION = "action";

        public LocalisableString Text { get; set; }

        public IDictionary<string, IDescriptionAction> Actions { get; set; }

        // todo: will be removed eventually.
        public static implicit operator DescriptionFormat(string text)
            => (LocalisableString)text;

        public static implicit operator DescriptionFormat(LocalisableString text) => new()
        {
            Text = text
        };
    }
}
