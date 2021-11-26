// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public enum KaraokeEditorScreenMode
    {
        [Description("Lyric")]
        Lyric,

        [Description("Singer")]
        Singer,

        [Description("Translate")]
        Translate,
    }
}
