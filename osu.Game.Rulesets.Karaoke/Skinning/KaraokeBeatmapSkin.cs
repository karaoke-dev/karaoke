// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
        public readonly IDictionary<int, Bindable<LyricLayout>> BindableLayouts = new Dictionary<int, Bindable<LyricLayout>>();
        public readonly IDictionary<int, Bindable<LayoutGroup>> BindableLayoutGroups = new Dictionary<int, Bindable<LayoutGroup>>();
        public readonly IDictionary<int, Bindable<LyricStyle>> BindableLyricStyles = new Dictionary<int, Bindable<LyricStyle>>();
        public readonly IDictionary<int, Bindable<NoteStyle>> BindableNoteStyles = new Dictionary<int, Bindable<NoteStyle>>();
        public readonly HashSet<IGroup> BindableStyleMappingRoles = new();

        public readonly Bindable<IDictionary<int, string>> BindableFontsLookup = new();
        public readonly Bindable<IDictionary<int, string>> BindableLayoutsLookup = new();
        public readonly Bindable<IDictionary<int, string>> BindableNotesLookup = new();

        public KaraokeBeatmapSkin(SkinInfo skin, IStorageResourceProvider resources, Stream configurationStream = null)
            : base(skin, resources, configurationStream)
        {
        }

        public override IBindable<TValue> GetConfig<TLookup, TValue>(TLookup lookup)
        {
            switch (lookup)
            {
                // Lookup skin by type and index
                case KaraokeSkinLookup skinLookup:
                {
                    var config = skinLookup.Config;
                    int lookupNumber = skinLookup.Lookup;

                    return config switch
                    {
                        KaraokeSkinConfiguration.LyricStyle => SkinUtils.As<TValue>(BindableLyricStyles[lookupNumber]),
                        KaraokeSkinConfiguration.LyricLayout => SkinUtils.As<TValue>(BindableLayouts[lookupNumber]),
                        KaraokeSkinConfiguration.LyricConfig => SkinUtils.As<TValue>(BindableDefaultLyricConfig),
                        KaraokeSkinConfiguration.NoteStyle => SkinUtils.As<TValue>(BindableNoteStyles[lookupNumber]),
                        _ => base.GetConfig<KaraokeSkinLookup, TValue>(skinLookup),
                    };
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
