// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose.Toolbar;

/// <summary>
/// Base toolbar button.
/// </summary>
public abstract partial class ToolbarButton : OsuClickableContainer
{
    [Resolved]
    private OsuColour colours { get; set; } = null!;

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

    public void SetIcon(IconUsage iconUsage) =>
        SetIcon(new SpriteIcon
        {
            Icon = iconUsage,
        });

    public void SetIcon(Drawable icon)
    {
        Size = new Vector2(SpecialActionToolbar.HEIGHT);
        IconContainer.Icon = icon;
        IconContainer.Show();
    }

    protected void SetState(bool enabled)
    {
        IconContainer.Icon.Alpha = enabled ? 1f : 0.5f;
        Enabled.Value = enabled;
    }

    protected override bool OnClick(ClickEvent e)
    {
        ToggleClickEffect();

        return base.OnClick(e);
    }

    public void ToggleClickEffect()
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
}
