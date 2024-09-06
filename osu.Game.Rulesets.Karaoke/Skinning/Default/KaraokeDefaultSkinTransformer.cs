// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.UI.HUD;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning.Default;

public class KaraokeDefaultSkinTransformer : SkinTransformer
{
    private readonly KaraokeSkin karaokeSkin;

    public KaraokeDefaultSkinTransformer(ISkin skin, IBeatmap beatmap)
        : base(skin)
    {
        karaokeSkin = new KaraokeSkin(new SkinInfo(), new InternalSkinStorageResourceProvider("Default"));
    }

    public override IBindable<TValue>? GetConfig<TLookup, TValue>(TLookup lookup)
        => karaokeSkin.GetConfig<TLookup, TValue>(lookup);

    public override Drawable? GetDrawableComponent(ISkinComponentLookup lookup)
    {
        switch (lookup)
        {
            case GlobalSkinnableContainerLookup containerLookup:
                // Only handle ruleset level defaults for now.
                if (containerLookup.Ruleset == null)
                    return base.GetDrawableComponent(lookup);

                switch (containerLookup.Lookup)
                {
                    case GlobalSkinnableContainers.MainHUDComponents:
                        // see the fall-back strategy in the SkinManager.AllSources.
                        // will receive the:
                        // 1. Legacy beatmap skin.
                        // 2. default skin(e.g. argon skin) -> container will not be null only if skin is edited.
                        // 3. triangle skin

                        // component will not be null only if skin is edited.
                        var component = base.GetDrawableComponent(lookup) as Container;

                        // todo: technically can return non-null container if current skin is triangle skin.
                        // but have no idea why still not showing the setting button.
                        if (component != null && !component.Children.OfType<SettingButtonsDisplay>().Any())
                        {
                            // should add the setting button if not in the ruleset hud.
                            component.Add(new SettingButtonsDisplay
                            {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                            });
                        }

                        return component;

                    case GlobalSkinnableContainers.SongSelect:
                    default:
                        return base.GetDrawableComponent(lookup);
                }

            default:
                return base.GetDrawableComponent(lookup);
        }
    }
}
