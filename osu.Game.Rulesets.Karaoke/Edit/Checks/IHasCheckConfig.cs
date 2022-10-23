// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Checks.Configs;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public interface IHasCheckConfig<TConfig> where TConfig : IHasConfig<TConfig>, new()
    {
        TConfig Config { get; set; }
    }
}
