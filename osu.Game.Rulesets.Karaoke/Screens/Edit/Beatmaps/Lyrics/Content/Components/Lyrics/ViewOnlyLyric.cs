// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;

public partial class ViewOnlyLyric : InteractableLyric
{
    public ViewOnlyLyric(Lyric lyric)
        : base(lyric)
    {
        Layers = new Layer[]
        {
            new LyricLayer(lyric),
            new InteractLyricLayer(lyric),
            new TimeTagLayer(lyric),
        };
    }
}
