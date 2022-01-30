// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using osu.Framework.Bindables;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Rulesets.Karaoke.Skinning.Groups;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    /// <summary>
    /// It's the skin for karaoke beatmap.
    /// </summary>
    public class KaraokeBeatmapSkin : KaraokeSkin
    {
        public readonly IDictionary<ElementType, IList<IKaraokeSkinElement>> Elements = new Dictionary<ElementType, IList<IKaraokeSkinElement>>();
        public readonly HashSet<IGroup> BindableStyleMappingRoles = new();

        public readonly Bindable<IDictionary<int, string>> BindableFontsLookup = new();
        public readonly Bindable<IDictionary<int, string>> BindableLayoutsLookup = new();
        public readonly Bindable<IDictionary<int, string>> BindableNotesLookup = new();

        public KaraokeBeatmapSkin(SkinInfo skin, IStorageResourceProvider resources, Stream configurationStream = null)
            : base(skin, resources, configurationStream)
        {
            SkinInfo.PerformRead(s =>
            {
                // we may want to move this to some kind of async operation in the future.
                foreach (ElementType skinnableTarget in Enum.GetValues(typeof(ElementType)))
                {
                    // todo: load the target from skin info.
                    Elements.Add(skinnableTarget, new List<IKaraokeSkinElement>());
                }
            });
        }

        public override IBindable<TValue> GetConfig<TLookup, TValue>(TLookup lookup)
        {
            switch (lookup)
            {
                // Lookup skin by type and index
                case KaraokeSkinLookup skinLookup:
                {
                    var type = skinLookup.Type;
                    int lookupNumber = skinLookup.Lookup;
                    var element = Elements[type].FirstOrDefault(x => x.ID == lookupNumber);
                    return SkinUtils.As<TValue>(new Bindable<TValue>((TValue)element));
                }

                // Lookup list of name by type
                case KaraokeIndexLookup indexLookup:
                    return indexLookup switch
                    {
                        KaraokeIndexLookup.Layout => SkinUtils.As<TValue>(BindableLayoutsLookup),
                        KaraokeIndexLookup.Style => SkinUtils.As<TValue>(BindableFontsLookup),
                        KaraokeIndexLookup.Note => SkinUtils.As<TValue>(BindableNotesLookup),
                        _ => throw new InvalidEnumArgumentException(nameof(indexLookup))
                    };

                case KaraokeSkinConfigurationLookup skinConfigurationLookup:
                    return base.GetConfig<KaraokeSkinConfigurationLookup, TValue>(skinConfigurationLookup);
            }

            return null;
        }
    }
}
