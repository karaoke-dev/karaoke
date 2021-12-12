// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions.EnumExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Layout;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Screens.Play;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI.HUD
{
    public class SettingButtonsDisplay : CompositeDrawable, ISkinnableDrawable
    {
        private readonly CornerBackground background;
        private readonly FillFlowContainer<SettingButton> triggerButtons;

        public bool UsesFixedAnchor { get; set; }

        public SettingButtonsDisplay()
        {
            AutoSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                background = new CornerBackground
                {
                    RelativeSizeAxes = Axes.Both,
                },
                triggerButtons = new FillFlowContainer<SettingButton>
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    AutoSizeAxes = Axes.Both,
                    Spacing = new Vector2(10),
                    Margin = new MarginPadding(10),
                    Direction = FillDirection.Vertical,
                },
            };
        }

        protected override bool OnInvalidate(Invalidation invalidation, InvalidationSource source)
        {
            // trying to change relative position in here.
            if ((invalidation & Invalidation.MiscGeometry) != 0)
            {
                var overlayDirection = Anchor.HasFlagFast(Anchor.x0) ? OverlayDirection.Left : OverlayDirection.Right;
                settingOverlayContainer?.ChangeOverlayDirection(overlayDirection);
            }

            return base.OnInvalidate(invalidation, source);
        }

        private SettingOverlayContainer settingOverlayContainer;

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, HUDOverlay hud, Player player)
        {
            background.Colour = colours.ContextMenuGray;

            var rulesetInfo = player.Ruleset.Value;
            Schedule(() =>
            {
                hud.Add(new KaraokeControlInputManager(rulesetInfo)
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = settingOverlayContainer = new SettingOverlayContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        OnNewOverlayAdded = overlay =>
                        {
                            var button = overlay.CreateToggleButton();
                            triggerButtons.Add(button);
                        }
                    }
                });
            });
        }
    }
}
