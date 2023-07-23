// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romajies;

public struct RomajiGenerateResult
{
    public TimeTag TimeTag { get; set; }

    public bool InitialRomaji { get; set; }

    public string? RomajiText { get; set; }
}
