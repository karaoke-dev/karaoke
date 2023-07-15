// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input;
using osu.Game.Graphics.Containers;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.Toolbar;

/// <summary>
/// Base toolbar button.
/// </summary>
public abstract partial class ToolbarButton : OsuClickableContainer
{
    public void SetIcon(Drawable icon)
    {
        Size = new Vector2(SpecialActionToolbar.HEIGHT);
        IconContainer.Icon = icon;
        IconContainer.Show();
    }

    [Resolved]
    private TextureStore textures { get; set; } = null!;

    [Resolved]
    private ReadableKeyCombinationProvider keyCombinationProvider { get; set; } = null!;

    public void SetIcon(string texture) =>
        SetIcon(new Sprite
        {
            Texture = textures.Get(texture),
        });

    public void SetIcon(IconUsage iconUsage) =>
        SetIcon(new SpriteIcon
        {
            Icon = iconUsage,
        });

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
}
