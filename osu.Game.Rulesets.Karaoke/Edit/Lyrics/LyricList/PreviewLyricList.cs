// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList
{
    public class PreviewLyricList : BaseLyricList
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
    }
}
