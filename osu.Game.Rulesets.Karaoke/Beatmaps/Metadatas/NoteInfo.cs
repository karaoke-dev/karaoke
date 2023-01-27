// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

public class NoteInfo
{
    public int Columns { get; set; } = 9;

    public Tone MaxTone =>
        new()
        {
            Scale = Columns / 2
        };

    public Tone MinTone => -MaxTone;
}
