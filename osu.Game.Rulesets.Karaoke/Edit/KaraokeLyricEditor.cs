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
        private readonly Bindable<float> bindableLyricEditorFontSize = new Bindable<float>();
        private readonly Bindable<Mode> bindableLyricEditorMode = new Bindable<Mode>();
        private readonly Bindable<RecordingMovingCaretMode> bindableRecordingMovingCaretMode = new Bindable<RecordingMovingCaretMode>();
        private readonly BindableBool bindableAutoFocusToEditLyric = new BindableBool();
        private readonly BindableInt bindableAutoFocusToEditLyricSkipRows = new BindableInt();

        private readonly LyricEditor lyricEditor;

        public KaraokeLyricEditor(Ruleset ruleset)
        {
            AddInternal(new KaraokeEditInputManager(ruleset.RulesetInfo)
            {
                RelativeSizeAxes = Axes.Both,
                Child = lyricEditor = new LyricEditor
                {
                    RelativeSizeAxes = Axes.Both,
                }
            });
            bindableLyricEditorMode.BindValueChanged(e =>
            {
                lyricEditor.Mode = e.NewValue;
            });
            bindableLyricEditorFontSize.BindValueChanged(e =>
            {
                lyricEditor.FontSize = e.NewValue;
            });
            bindableRecordingMovingCaretMode.BindValueChanged(e =>
            {
                lyricEditor.RecordingMovingCaretMode = e.NewValue;
            });
            bindableAutoFocusToEditLyric.BindValueChanged(e =>
            {
                lyricEditor.AutoFocusEditLyric = e.NewValue;
            });
            bindableAutoFocusToEditLyricSkipRows.BindValueChanged(e =>
            {
                lyricEditor.AutoFocusEditLyricSkipRows = e.NewValue;
            });
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetEditConfigManager editConfigManager)
        {
            editConfigManager.BindWith(KaraokeRulesetEditSetting.LyricEditorFontSize, bindableLyricEditorFontSize);
            editConfigManager.BindWith(KaraokeRulesetEditSetting.LyricEditorMode, bindableLyricEditorMode);
            editConfigManager.BindWith(KaraokeRulesetEditSetting.RecordingMovingCaretMode, bindableRecordingMovingCaretMode);
            editConfigManager.BindWith(KaraokeRulesetEditSetting.AutoFocusToEditLyric, bindableAutoFocusToEditLyric);
            editConfigManager.BindWith(KaraokeRulesetEditSetting.AutoFocusToEditLyricSkipRows, bindableAutoFocusToEditLyricSkipRows);
        }
    }
}
