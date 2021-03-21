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
    public class DrawableKaraokeEditRuleset : DrawableKaraokeRuleset
    {
        private readonly Bindable<EditMode> bindableEditMode = new Bindable<EditMode>();
        private readonly Bindable<bool> bindableDisplayRubyToggle = new Bindable<bool>();
        private readonly Bindable<bool> bindableDisplayRomajiToggle = new Bindable<bool>();
        private readonly Bindable<bool> bindableDisplayTranslateToggle = new Bindable<bool>();

        public new IScrollingInfo ScrollingInfo => base.ScrollingInfo;

        public override bool DisplayNotePlayfield => true;

        public DrawableKaraokeEditRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods)
            : base(ruleset, beatmap, mods)
        {
            bindableEditMode.BindValueChanged(e =>
            {
                if (e.NewValue == EditMode.LyricEditor)
                    Playfield.Hide();
                else
                    Playfield.Show();
            }, true);
            bindableDisplayRubyToggle.BindValueChanged(x => { Session.SetValue(KaraokeRulesetSession.DisplayRuby, x.NewValue); });
            bindableDisplayRomajiToggle.BindValueChanged(x => { Session.SetValue(KaraokeRulesetSession.DisplayRomaji, x.NewValue); });
            bindableDisplayTranslateToggle.BindValueChanged(x => { Session.SetValue(KaraokeRulesetSession.UseTranslate, x.NewValue); });
        }

        protected override void InitialOverlay()
        {
        }

        public override DrawableHitObject<KaraokeHitObject> CreateDrawableRepresentation(KaraokeHitObject h) => null;

        protected override Playfield CreatePlayfield() => new KaraokeEditPlayfield();

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetEditConfigManager editConfigManager)
        {
            editConfigManager.BindWith(KaraokeRulesetEditSetting.EditMode, bindableEditMode);
            editConfigManager.BindWith(KaraokeRulesetEditSetting.DisplayRuby, bindableDisplayRubyToggle);
            editConfigManager.BindWith(KaraokeRulesetEditSetting.DisplayRomaji, bindableDisplayRomajiToggle);
            editConfigManager.BindWith(KaraokeRulesetEditSetting.DisplayTranslate, bindableDisplayTranslateToggle);
        }
    }
}
