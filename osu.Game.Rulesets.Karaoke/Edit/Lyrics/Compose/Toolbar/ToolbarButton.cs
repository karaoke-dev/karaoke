// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.Toolbar
{
    public abstract class ToolbarButton : OsuClickableContainer
    {
        public void SetIcon(Drawable icon)
        {
            Size = new Vector2(SpecialActionToolbar.HEIGHT);
            IconContainer.Icon = icon;
            IconContainer.Show();
        }

        [Resolved]
        private TextureStore textures { get; set; }

        [Resolved]
        private OsuColour colours { get; set; }

        [Resolved]
        private ReadableKeyCombinationProvider keyCombinationProvider { get; set; }

        public void SetIcon(string texture) =>
            SetIcon(new Sprite
            {
                Texture = textures.Get(texture),
            });

        public void SetIcon(IconUsage iconUsage) =>
            SetIcon(new SpriteIcon
            {
                Icon = iconUsage
            });

        protected void ToggleClickEffect()
        {
            if (Enabled.Value)
            {
                IconContainer.FadeOut(100).Then().FadeIn();
            }
            else
            {
                IconContainer.FadeColour(colours.Red, 100).Then().FadeColour(Colour4.White);
            }
        }

        protected void SetState(bool enabled)
        {
            IconContainer.Icon.Alpha = enabled ? 1f : 0.5f;
            Enabled.Value = enabled;
        }

        protected ConstrainedIconContainer IconContainer;

        protected ToolbarButton()
        {
            Children = new Drawable[]
            {
                IconContainer = new ConstrainedIconContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(SpecialActionToolbar.ICON_SIZE),
                    Alpha = 0,
                },
            };
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (Enabled.Value)
                ToggleClickEffect();

            return base.OnClick(e);
        }
    }
}
