// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;

namespace osu.Game.Rulesets.Karaoke.Graphics.Sprites
{
    public class DrawableSingerAvatar : CompositeDrawable
    {
        private readonly IBindable<string> binsableAvatarFile = new Bindable<string>();

        private readonly Sprite avatar;

        public DrawableSingerAvatar()
        {
            InternalChild = avatar = new Sprite
            {
                RelativeSizeAxes = Axes.Both,
                FillMode = FillMode.Fit,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };
        }

        [BackgroundDependencyLoader]
        private void load(LargeTextureStore textures, IKaraokeBeatmapResourcesProvider karaokeBeatmapResourcesProvider)
        {
            binsableAvatarFile.BindValueChanged(_ =>
            {
                if (singer == null)
                    avatar.Texture = getDefaultAvatar();
                else
                    avatar.Texture = karaokeBeatmapResourcesProvider.GetSingerAvatar(singer) ?? getDefaultAvatar();

                avatar.FadeInFromZero(500);
            }, true);

            Texture getDefaultAvatar()
                => textures.Get(@"Online/avatar-guest");
        }

        private ISinger singer;

        public virtual ISinger Singer
        {
            get => singer;
            set
            {
                singer = value;

                if (singer is not Singer s)
                    return;

                binsableAvatarFile.UnbindBindings();
                binsableAvatarFile.BindTo(s.AvatarFileBindable);
            }
        }
    }
}
