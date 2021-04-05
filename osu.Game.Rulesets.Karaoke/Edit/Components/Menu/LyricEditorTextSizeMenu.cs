// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menu
{
    public class LyricEditorTextSizeMenu : MenuItem
    {
        private readonly Bindable<float> bindableFontSize = new Bindable<float>();

        public LyricEditorTextSizeMenu(KaraokeRulesetEditConfigManager config, string text)
            : base(text)
        {
            Items = createMenuItems();

            config.BindWith(KaraokeRulesetEditSetting.LyricEditorFontSize, bindableFontSize);
            bindableFontSize.BindValueChanged(e =>
            {
                var newSelection = e.NewValue;
                Items.OfType<ToggleMenuItem>().ForEach(x =>
                {
                    var match = x.Text.Value == FontUtils.GetText(newSelection);
                    x.State.Value = match;
                });
            }, true);
        }

        private ToggleMenuItem[] createMenuItems()
        {
            var sizes = FontUtils.DefaultPreviewFontSize();
            return sizes.Select(e =>
            {
                var item = new ToggleMenuItem(FontUtils.GetText(e), MenuItemType.Standard, _ => updateMode(e));
                return item;
            }).ToArray();
        }

        private void updateMode(float size)
        {
            bindableFontSize.Value = size;
        }
    }
}
