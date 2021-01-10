// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class KaraokeLyricEditor : LyricEditor
    {
        private readonly Bindable<EditMode> bindableEditMode = new Bindable<EditMode>();
        private readonly Bindable<Mode> bindableLyricEditorMode = new Bindable<Mode>();
        private readonly Bindable<LyricFastEditMode> bindableLyricEditorFastEditMode = new Bindable<LyricFastEditMode>();

        public KaraokeLyricEditor()
        {
            bindableEditMode.BindValueChanged(e =>
            {
                if (e.NewValue == EditMode.LyricEditor)
                    Show();
                else
                    Hide();
            });
            bindableLyricEditorMode.BindValueChanged(e =>
            {
                Mode = e.NewValue;
            });
            bindableLyricEditorFastEditMode.BindValueChanged(e =>
            {
                LyricFastEditMode = e.NewValue;
            });
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetEditConfigManager editConfigManager)
        {
            editConfigManager.BindWith(KaraokeRulesetEditSetting.EditMode, bindableEditMode);
            editConfigManager.BindWith(KaraokeRulesetEditSetting.LyricEditorMode, bindableLyricEditorMode);
            editConfigManager.BindWith(KaraokeRulesetEditSetting.LyricEditorFastEditMode, bindableLyricEditorFastEditMode);
        }
    }
}
