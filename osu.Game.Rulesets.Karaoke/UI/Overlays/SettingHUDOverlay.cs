// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Timing;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.UI.Overlays.Settings;
using osu.Game.Rulesets.Mods;
using osu.Game.Screens.Play.PlayerSettings;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI.Overlays
{
    public class SettingHUDOverlay : Container
    {
        private readonly ControlLayer controlLayer;

        public SettingHUDOverlay(DrawableKaraokeRuleset drawableRuleset, IReadOnlyList<Mod> mods)
        {
            RelativeSizeAxes = Axes.Both;

            Children = new Drawable[]
            {
                new KaraokeControlInputManager(drawableRuleset.Ruleset.RulesetInfo)
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = controlLayer = new ControlLayer(drawableRuleset.Beatmap)
                    {
                        RelativeSizeAxes = Axes.Both,
                        Clock = new FramedClock(new StopwatchClock(true))
                    }
                }
            };

            foreach (var mod in mods.OfType<IApplicableToSettingHUDOverlay>())
                mod.ApplyToOverlay(this);
        }

        public void AddSettingsGroup(PlayerSettingsGroup group) => controlLayer.AddSettingsGroup(group);

        public void AddExtraOverlay(RightSideOverlay overlay) => controlLayer.AddExtraOverlay(overlay);

        public class ControlLayer : CompositeDrawable, IKeyBindingHandler<KaraokeAction>
        {
            private readonly BindableInt bindablePitch = new BindableInt();
            private readonly BindableInt bindableVocalPitch = new BindableInt();
            private readonly BindableInt bindableSaitenPitch = new BindableInt();

            private readonly FillFlowContainer<SettingButton> triggerButtons;

            private readonly ControlOverlay gameplaySettingsOverlay;

            public ControlLayer(IBeatmap beatmap)
            {
                InternalChildren = new Drawable[]
                {
                    triggerButtons = new FillFlowContainer<SettingButton>
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        AutoSizeAxes = Axes.Both,
                        Spacing = new Vector2(10),
                        Margin = new MarginPadding(40),
                        Direction = FillDirection.Vertical,
                    }
                };

                AddExtraOverlay(gameplaySettingsOverlay = new ControlOverlay(beatmap));
            }

            public void ToggleGameplaySettingsOverlay() => gameplaySettingsOverlay.ToggleVisibility();

            public virtual bool OnPressed(KaraokeAction action)
            {
                switch (action)
                {
                    // Open adjustment overlay
                    case KaraokeAction.OpenPanel:
                        ToggleGameplaySettingsOverlay();
                        break;

                    // Pitch
                    case KaraokeAction.IncreasePitch:
                        bindablePitch.TriggerIncrease();
                        break;

                    case KaraokeAction.DecreasePitch:
                        bindablePitch.TriggerDecrease();
                        break;

                    case KaraokeAction.ResetPitch:
                        bindablePitch.SetDefault();
                        break;

                    // Vocal pitch
                    case KaraokeAction.IncreaseVocalPitch:
                        bindableVocalPitch.TriggerIncrease();
                        break;

                    case KaraokeAction.DecreaseVocalPitch:
                        bindableVocalPitch.TriggerDecrease();
                        break;

                    case KaraokeAction.ResetVocalPitch:
                        bindableVocalPitch.SetDefault();
                        break;

                    // Saiten pitch
                    case KaraokeAction.IncreaseSaitenPitch:
                        bindableSaitenPitch.TriggerIncrease();
                        break;

                    case KaraokeAction.DecreaseSaitenPitch:
                        bindableSaitenPitch.TriggerDecrease();
                        break;

                    case KaraokeAction.ResetSaitenPitch:
                        bindableSaitenPitch.SetDefault();
                        break;

                    default:
                        return false;
                }

                return true;
            }

            public virtual void OnReleased(KaraokeAction action)
            {
            }

            public void AddSettingsGroup(PlayerSettingsGroup group)
            {
                gameplaySettingsOverlay.Add(group);
            }

            public void AddExtraOverlay(RightSideOverlay container)
            {
                AddInternal(container);
                triggerButtons.Add(container.CreateToggleButton());
            }

            [BackgroundDependencyLoader]
            private void load(KaraokeSessionStatics session)
            {
                session.BindWith(KaraokeRulesetSession.Pitch, bindablePitch);
                session.BindWith(KaraokeRulesetSession.VocalPitch, bindableVocalPitch);
                session.BindWith(KaraokeRulesetSession.SaitenPitch, bindableSaitenPitch);
            }
        }
    }

    /// <summary>
    /// Will move into framework layer
    /// </summary>
    public static class BindableNumberExtension
    {
        public static void TriggerIncrease(this BindableInt bindableInt)
        {
            bindableInt.Value += bindableInt.Precision;
        }

        public static void TriggerDecrease(this BindableInt bindableInt)
        {
            bindableInt.Value -= bindableInt.Precision;
        }
    }
}
