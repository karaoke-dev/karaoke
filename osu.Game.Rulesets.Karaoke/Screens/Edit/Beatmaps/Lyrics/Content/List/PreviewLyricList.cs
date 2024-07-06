// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.List;

public partial class PreviewLyricList : LyricList
{
    private readonly IBindable<float> bindableFontSize = new Bindable<float>();

    public PreviewLyricList()
    {
        bindableFontSize.BindValueChanged(e =>
        {
            AdjustSkin(skin =>
            {
                skin.FontSize = e.NewValue;
            });
        });
    }

    protected override DrawableLyricList CreateDrawableLyricList()
        => new DrawablePreviewLyricList();

    [BackgroundDependencyLoader]
    private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
    {
        lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.LyricEditorFontSize, bindableFontSize);
    }

    public partial class DrawablePreviewLyricList : DrawableLyricList
    {
        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
        private readonly IBindable<bool> bindableAutoFocusToEditLyric = new Bindable<bool>();
        private readonly IBindable<int> bindableAutoFocusToEditLyricSkipRows = new Bindable<int>();

        protected override bool ScrollToPosition(ICaretPosition caret)
        {
            // should not move the position if caret is only support clicking.
            if (caret is ClickingCaretPosition)
                return false;

            // should not move the position in manage lyric mode.
            if (bindableMode.Value is LyricEditorMode.EditText or LyricEditorMode.EditRuby)
                return false;

            // move to target position if auto focus.
            return bindableAutoFocusToEditLyric.Value;
        }

        protected override int SkipRows()
        {
            return bindableAutoFocusToEditLyricSkipRows.Value;
        }

        protected override Row CreateEditRow(Lyric lyric)
            => new EditLyricPreviewRow(lyric);

        protected override Row GetCreateNewLyricRow()
            => new CreateNewLyricPreviewRow();

        protected override Vector2 Spacing => new(0, 2);

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state, KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
        {
            bindableMode.BindTo(state.BindableMode);

            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.AutoFocusToEditLyric, bindableAutoFocusToEditLyric);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.AutoFocusToEditLyricSkipRows, bindableAutoFocusToEditLyricSkipRows);
        }
    }
}
