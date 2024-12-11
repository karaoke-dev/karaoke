// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;

public class ClassicStyleCategory : StageElementCategory<ClassicStyle, Lyric>
{
    protected override ClassicStyle CreateDefaultElement()
        => new()
        {
            LyricStyle = LyricStyle.CreateDefault(),
        };
}
