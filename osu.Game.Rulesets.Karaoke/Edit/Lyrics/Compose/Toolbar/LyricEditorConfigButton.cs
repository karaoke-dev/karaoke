// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.Toolbar
{
    public abstract class LyricEditorConfigButton : ToolbarButton
    {
        private readonly Bindable<bool> bindableConfig = new();

        protected LyricEditorConfigButton()
        {
            SetIcon(Icon);

            bindableConfig.BindValueChanged(x =>
            {
                IconContainer.Alpha = x.NewValue ? 1 : 0.5f;
            }, true);

            Action = () =>
            {
                bindableConfig.Value = !bindableConfig.Value;
            };
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
        {
            lyricEditorConfigManager.BindWith(Setting, bindableConfig);
        }

        protected abstract KaraokeRulesetLyricEditorSetting Setting { get; }

        protected abstract IconUsage Icon { get; }
    }
}
