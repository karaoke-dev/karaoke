// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class DrawableKaraokeEditorRuleset : DrawableKaraokeRuleset
    {
        private readonly Bindable<bool> bindableDisplayRubyToggle = new();
        private readonly Bindable<bool> bindableDisplayRomajiToggle = new();
        private readonly Bindable<bool> bindableDisplayTranslateToggle = new();

        public new IScrollingInfo ScrollingInfo => base.ScrollingInfo;

        protected override bool DisplayNotePlayfield => true;

        public DrawableKaraokeEditorRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods)
            : base(ruleset, beatmap, mods)
        {
            bindableDisplayRubyToggle.BindValueChanged(x => { Session.SetValue(KaraokeRulesetSession.DisplayRuby, x.NewValue); });
            bindableDisplayRomajiToggle.BindValueChanged(x => { Session.SetValue(KaraokeRulesetSession.DisplayRomaji, x.NewValue); });
            bindableDisplayTranslateToggle.BindValueChanged(x => { Session.SetValue(KaraokeRulesetSession.UseTranslate, x.NewValue); });
        }

        protected override Playfield CreatePlayfield() => new KaraokeEditorPlayfield();

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetEditConfigManager editConfigManager)
        {
            editConfigManager.BindWith(KaraokeRulesetEditSetting.DisplayRuby, bindableDisplayRubyToggle);
            editConfigManager.BindWith(KaraokeRulesetEditSetting.DisplayRomaji, bindableDisplayRomajiToggle);
            editConfigManager.BindWith(KaraokeRulesetEditSetting.DisplayTranslate, bindableDisplayTranslateToggle);
        }

        // todo: use default adjustment container because DrawableEditorRulesetWrapper will create it but contains no KaraokeRulesetConfigManager
        public override PlayfieldAdjustmentContainer CreatePlayfieldAdjustmentContainer() => new();
    }
}
