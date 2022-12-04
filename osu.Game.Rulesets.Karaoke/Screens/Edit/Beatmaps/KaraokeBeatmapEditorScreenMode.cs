// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.ComponentModel;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps
{
    public enum KaraokeBeatmapEditorScreenMode
    {
        [Description("Lyric")]
        Lyric,

        [Description("Singer")]
        Singer,

        [Description("Translate")]
        Translate,
    }
}
