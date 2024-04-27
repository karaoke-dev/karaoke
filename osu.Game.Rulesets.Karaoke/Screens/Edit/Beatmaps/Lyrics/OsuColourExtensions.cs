// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

public static class OsuColourExtensions
{
    // todo: this should be moved to a more appropriate place.
    // or just delete it.
    public static Color4 GetRecordingTimeTagCaretColour(this OsuColour colours, TimeTag timeTag)
        => timeTag.Time.HasValue ? colours.Red : colours.Gray3;
}
