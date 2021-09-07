// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Screens.Edit.Setup;

namespace osu.Game.Rulesets.Karaoke.Edit.Setup
{
    public class KaraokeSetupSection : RulesetSetupSection
    {
        public KaraokeSetupSection()
            : base(new KaraokeRuleset().RulesetInfo)
        {
        }
    }
}
