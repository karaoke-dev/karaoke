// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class KaraokeLyricEditor : CompositeDrawable
    {
        private readonly Bindable<EditMode> bindableEditMode = new Bindable<EditMode>();

        private readonly Bindable<int> bindableLyricEditorFontSize = new Bindable<int>();
        private readonly Bindable<Mode> bindableLyricEditorMode = new Bindable<Mode>();
        private readonly Bindable<LyricFastEditMode> bindableLyricEditorFastEditMode = new Bindable<LyricFastEditMode>();
        private readonly Bindable<RecordingMovingCursorMode> bindableRecordingMovingCursorMode = new Bindable<RecordingMovingCursorMode>();

        private readonly LyricEditor lyricEditor;

        [Cached]
        private readonly TimeTagManager timeTagManager;

        public KaraokeLyricEditor(Ruleset ruleset)
        {
            AddInternal(timeTagManager = new TimeTagManager());

            AddInternal(new KaraokeEditInputManager(ruleset.RulesetInfo)
            {
                RelativeSizeAxes = Axes.Both,
                Child = lyricEditor = new LyricEditor
                {
                    RelativeSizeAxes = Axes.Both,
                }
            });

            bindableEditMode.BindValueChanged(e =>
            {
                if (e.NewValue == EditMode.LyricEditor)
                    Show();
                else
                    Hide();
            });
            bindableLyricEditorMode.BindValueChanged(e =>
            {
                lyricEditor.Mode = e.NewValue;
            });
            bindableLyricEditorFastEditMode.BindValueChanged(e =>
            {
                lyricEditor.LyricFastEditMode = e.NewValue;
            });
            bindableLyricEditorFontSize.BindValueChanged(e =>
            {
                lyricEditor.FontSize = e.NewValue;
            });
            bindableRecordingMovingCursorMode.BindValueChanged(e =>
            {
                lyricEditor.RecordingMovingCursorMode = e.NewValue;
            });
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetEditConfigManager editConfigManager)
        {
            editConfigManager.BindWith(KaraokeRulesetEditSetting.EditMode, bindableEditMode);

            editConfigManager.BindWith(KaraokeRulesetEditSetting.LyricEditorFontSize, bindableLyricEditorFontSize);
            editConfigManager.BindWith(KaraokeRulesetEditSetting.LyricEditorMode, bindableLyricEditorMode);
            editConfigManager.BindWith(KaraokeRulesetEditSetting.LyricEditorFastEditMode, bindableLyricEditorFastEditMode);
            editConfigManager.BindWith(KaraokeRulesetEditSetting.RecordingMovingCursorMode, bindableRecordingMovingCursorMode);
        }
    }
}
