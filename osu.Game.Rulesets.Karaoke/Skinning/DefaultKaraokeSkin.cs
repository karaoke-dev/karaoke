// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using osu.Framework.Bindables;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class DefaultKaraokeSkin : KaraokeSkin
    {
        internal static readonly Guid DEFAULT_SKIN = new("FEC5A291-5709-11EC-9F10-0800200C9A66");

        public static SkinInfo Default { get; } = new()
        {
            ID = DEFAULT_SKIN,
            Name = "karaoke! (default skin)",
            Creator = "team karaoke!",
            InstantiationInfo = typeof(DefaultKaraokeSkin).GetInvariantInstantiationInfo()
        };

        public DefaultKaraokeSkin(IStorageResourceProvider resources)
            : this(Default, resources)
        {
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        public DefaultKaraokeSkin(SkinInfo skin, IStorageResourceProvider resources)
            : base(skin, resources)
        {
            // TODO : need a better way to load resource
            var assembly = Assembly.GetExecutingAssembly();
            const string resource_name = @"osu.Game.Rulesets.Karaoke.Resources.Skin.default.skin";

            using (var stream = assembly.GetManifestResourceStream(resource_name))
            using (var reader = new LineBufferedReader(stream))
            {
                var karaokeSkin = new KaraokeSkinDecoder().Decode(reader);

                // Create bindable
                for (int i = 0; i < karaokeSkin.Styles.Count; i++)
                    BindableStyles.Add(i, new Bindable<LyricStyle>(karaokeSkin.Styles[i]));
                for (int i = 0; i < karaokeSkin.Layouts.Count; i++)
                    BindableLayouts.Add(i, new Bindable<LyricLayout>(karaokeSkin.Layouts[i]));
                for (int i = 0; i < karaokeSkin.NoteSkins.Count; i++)
                    BindableNotes.Add(i, new Bindable<NoteSkin>(karaokeSkin.NoteSkins[i]));
                BindableDefaultLyricConfig.Value = karaokeSkin.DefaultLyricConfig;

                // Create lookups
                BindableFontsLookup.Value = karaokeSkin.Styles.ToDictionary(k => karaokeSkin.Styles.IndexOf(k), y => y.Name);
                BindableLayoutsLookup.Value = karaokeSkin.Layouts.ToDictionary(k => karaokeSkin.Layouts.IndexOf(k), y => y.Name);
                BindableNotesLookup.Value = karaokeSkin.NoteSkins.ToDictionary(k => karaokeSkin.NoteSkins.IndexOf(k), y => y.Name);
            }
        }
    }
}
