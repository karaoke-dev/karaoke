// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Input.Bindings;
using osu.Game.Beatmaps;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI.HUD;
using osu.Game.Rulesets.Karaoke.UI.PlayerSettings;
using osu.Game.Screens.Play.PlayerSettings;
using osuTK;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class ControlLayer : CompositeDrawable, IKeyBindingHandler<KaraokeAction>
    {
        private readonly BindableInt bindablePitch = new BindableInt();
        private readonly BindableInt bindableVocalPitch = new BindableInt();
        private readonly BindableInt bindableSaitenPitch = new BindableInt();

        private readonly FillFlowContainer<TriggerButton> triggerButtons;

        private readonly GameplaySettingsOverlay gameplaySettingsOverlay;

        public ControlLayer(IBeatmap beatmap)
        {
            InternalChildren = new Drawable[]
            {
                triggerButtons = new FillFlowContainer<TriggerButton>
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    AutoSizeAxes = Axes.Both,
                    Spacing = new Vector2(10),
                    Margin = new MarginPadding(40),
                    Direction = FillDirection.Vertical,
                }
            };

            AddExtraOverlay(new TriggerButton
                {
                    Name = "Toggle setting button",
                    Text = "Settings",
                    TooltipText = "Open/Close setting",
                    Action = ToggleGameplaySettingsOverlay
                },
                gameplaySettingsOverlay = new GameplaySettingsOverlay
                {
                    RelativeSizeAxes = Axes.Y,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                });

            // Add common group
            AddSettingsGroup(new VisualSettings { Expanded = false });
            AddSettingsGroup(new PitchSettings { Expanded = false });
            AddSettingsGroup(new RubyRomajiSettings { Expanded = false });

            // Add translate group if this beatmap has translate
            var translateDictionary = beatmap?.HitObjects?.OfType<TranslateDictionary>().FirstOrDefault();
            if (translateDictionary != null && translateDictionary.Translates.Any())
                AddSettingsGroup(new TranslateSettings(translateDictionary) { Expanded = false });
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

        public void AddExtraOverlay(TriggerButton button, FocusedOverlayContainer container)
        {
            triggerButtons.Add(button);
            AddInternal(container);
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeSessionStatics session)
        {
            session.BindWith(KaraokeRulesetSession.Pitch, bindablePitch);
            session.BindWith(KaraokeRulesetSession.VocalPitch, bindableVocalPitch);
            session.BindWith(KaraokeRulesetSession.SaitenPitch, bindableSaitenPitch);
        }

        public class TriggerButton : TriangleButton, IHasTooltip
        {
            public string TooltipText { get; set; }

            public TriggerButton()
            {
                Width = 90;
                Height = 45;
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
