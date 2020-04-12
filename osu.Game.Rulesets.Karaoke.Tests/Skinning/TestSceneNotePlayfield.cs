// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.UI;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning
{
    public class TestSceneNotePlayfield : KaraokeSkinnableColumnTestScene
    {
        [BackgroundDependencyLoader]
        private void load(RulesetConfigCache configCache)
        {
            var config = (KaraokeRulesetConfigManager)configCache.GetConfigFor(Ruleset.Value.CreateInstance());
            Dependencies.Cache(new KaraokeSessionStatics(config, null));

            SetContents(() => new KaraokeInputManager(new KaraokeRuleset().RulesetInfo)
            {
                Child = new NotePlayfield(COLUMN_NUMBER)
            });
        }
    }
}
