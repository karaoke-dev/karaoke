// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList
{
    public class DrawablePreviewLyricList : DrawableLyricList
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
            if (bindableMode.Value == LyricEditorMode.Texting)
                return false;

            // move to target position if auto focus.
            bool autoFocus = bindableAutoFocusToEditLyric.Value;
            if (!autoFocus)
                return false;

            return true;
        }

        protected override int SkipRows()
        {
            return bindableAutoFocusToEditLyricSkipRows.Value;
        }

        protected override Row GetCreateNewLyricRow()
            => new CreateNewLyricPreviewRow();

        protected override DrawableLyricListItem CreateLyricListItem(Lyric item)
            => new DrawablePreviewLyricListItem(item);

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
