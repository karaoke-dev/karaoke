// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Skinning.Legacy
{
    public class LegacyStageBackground : LegacyKaraokeElement
    {
        private readonly IBindable<ScrollingDirection> direction = new Bindable<ScrollingDirection>();

        public LegacyStageBackground()
        {
            RelativeSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load(ISkinSource skin, IScrollingInfo scrollingInfo)
        {
            InternalChild = new Sprite
            {
                Anchor = Anchor.BottomRight,
                Origin = Anchor.BottomRight,
                Texture = getTexture(skin)
            };

            direction.BindTo(scrollingInfo.Direction);
            direction.BindValueChanged(OnDirectionChanged, true);
        }

        protected virtual void OnDirectionChanged(ValueChangedEvent<ScrollingDirection> direction)
        {
            Scale = direction.NewValue == ScrollingDirection.Left ? Vector2.One : new Vector2(-1, 1);
        }

        private Texture getTexture(ISkinSource skin) => skin.GetTexture(GetTextureName());

        public static string GetTextureName() => "karaoke-stage-background";
    }
}
