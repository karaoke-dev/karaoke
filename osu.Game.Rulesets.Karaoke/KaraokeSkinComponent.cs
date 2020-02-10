// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke
{
    public class KaraokeSkinComponent : GameplaySkinComponent<KaraokeSkinComponents>
    {
        public KaraokeSkinComponent(KaraokeSkinComponents component)
            : base(component)
        {
        }

        protected override string RulesetPrefix => KaraokeRuleset.SHORT_NAME;

        protected override string ComponentName => Component.ToString().ToLower();
    }
}
