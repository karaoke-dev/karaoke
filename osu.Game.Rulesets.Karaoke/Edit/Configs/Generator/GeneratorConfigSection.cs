// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator
{
    public abstract class GeneratorConfigSection<TConfig> : GeneratorConfigSection where TConfig : IHasConfig<TConfig>, new()
    {
        private readonly TConfig defaultConfig;
        private readonly Bindable<TConfig> current;

        protected GeneratorConfigSection(Bindable<TConfig> current)
        {
            this.current = current;
            defaultConfig = new TConfig().CreateDefaultConfig();
        }

        protected void RegisterConfig<TValue>(Bindable<TValue> bindable, string propertyName)
        {
            // set default value
            var defaultValue = getConfigValue<TValue>(defaultConfig, propertyName);
            bindable.Default = defaultValue;

            // set current value
            current.BindValueChanged(e =>
            {
                var currentValue = getConfigValue<TValue>(e.NewValue, propertyName);
                if (bindable.Value != null && EqualityComparer<TValue>.Default.Equals(currentValue, bindable.Value))
                    return;

                bindable.Value = currentValue;
            });

            // save value if control changed
            bindable.BindValueChanged(e =>
            {
                setConfigValue(propertyName, e.NewValue);
                current.TriggerChange();
            });
        }

        protected void RegisterDisableTrigger(Bindable<bool> bindable, Drawable[] triggeredControl)
        {
            Schedule(() =>
            {
                bindable.BindValueChanged(e =>
                {
                    var enabled = e.NewValue;

                    foreach (var control in triggeredControl)
                    {
                        // todo : Use this because input interface v2 disable property not working.
                        control.Alpha = enabled ? 1 : 0.5f;

                        // todo : should user better way to handle IHasCurrentValue
                        if (control is IHasCurrentValue<bool> iHasCurrentValue)
                            iHasCurrentValue.Current.Disabled = !enabled;
                    }
                }, true);
            });
        }

        private TValue getConfigValue<TValue>(TConfig config, string propertyName)
            => (TValue)config.GetType().GetProperty(propertyName)?.GetValue(config);

        private void setConfigValue(string propertyName, object value)
            => current.Value.GetType().GetProperty(propertyName)?.SetValue(current.Value, value);
    }

    public abstract class GeneratorConfigSection : Container
    {
        private readonly FillFlowContainer flow;

        [Resolved]
        protected OsuColour Colours { get; private set; }

        [Resolved]
        protected IBindable<WorkingBeatmap> Beatmap { get; private set; }

        protected override Container<Drawable> Content => flow;

        protected abstract string Title { get; }

        protected GeneratorConfigSection()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            Padding = new MarginPadding(10);

            InternalChildren = new Drawable[]
            {
                new OsuSpriteText
                {
                    Font = OsuFont.GetFont(weight: FontWeight.Bold, size: 18),
                    Text = Title,
                },
                flow = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Spacing = new Vector2(10),
                    Direction = FillDirection.Vertical,
                    Margin = new MarginPadding { Top = 30 }
                }
            };
        }
    }
}
