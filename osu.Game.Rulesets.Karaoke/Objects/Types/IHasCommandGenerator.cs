// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Stages;

namespace osu.Game.Rulesets.Karaoke.Objects.Types;

public interface IHasCommandGenerator
{
    IHitObjectCommandGenerator? CommandGenerator { get; }
}
