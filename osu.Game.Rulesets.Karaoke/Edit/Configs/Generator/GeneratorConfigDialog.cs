// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator
{
    public abstract class GeneratorConfigDialog<T> : TitleFocusedOverlayContainer where T : IHasConfig<T>, new()
    {
        private readonly Bindable<T> bindableConfig = new Bindable<T>();

        public GeneratorConfigDialog()
        {
            // todo : has scroll-bar in here.
            // enable to assign layou by property.

            // has ok and cancel button.
        }

        protected abstract KaraokeRulesetEditGeneratorSetting Config { get; }

        private void load(KaraokeRulesetEditGeneratorConfigManager config)
        {
            bindableConfig.BindTo(config.GetBindable<T>(Config));
        }
    }
}
