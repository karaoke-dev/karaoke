// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using System.Linq;
using System.Reflection;
using osu.Framework.Bindables;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Fonts;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Layouts;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Notes;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class DefaultKaraokeSkin : KaraokeSkin
    {
        public DefaultKaraokeSkin(SkinInfo skin, IStorageResourceProvider resources, Stream configurationStream = null)
            : base(skin, resources, configurationStream)
        {
            // TODO : need a better way to load resource
            var assembly = Assembly.GetExecutingAssembly();
            const string resource_name = @"osu.Game.Rulesets.Karaoke.Resources.Skin.default.skin";

            using (var stream = assembly.GetManifestResourceStream(resource_name))
            using (var reader = new LineBufferedReader(stream))
            {
                var karaokeSkin = new KaraokeSkinDecoder().Decode(reader);

                // Create bindable
                for (int i = 0; i < karaokeSkin.Fonts.Count; i++)
                    BindableFonts.Add(i, new Bindable<LyricFont>(karaokeSkin.Fonts[i]));
                for (int i = 0; i < karaokeSkin.Layouts.Count; i++)
                    BindableLayouts.Add(i, new Bindable<LyricLayout>(karaokeSkin.Layouts[i]));
                for (int i = 0; i < karaokeSkin.NoteSkins.Count; i++)
                    BindableNotes.Add(i, new Bindable<NoteSkin>(karaokeSkin.NoteSkins[i]));

                // Create lookups
                BindableFontsLookup.Value = karaokeSkin.Fonts.ToDictionary(k => karaokeSkin.Fonts.IndexOf(k), y => y.Name);
                BindableLayoutsLookup.Value = karaokeSkin.Layouts.ToDictionary(k => karaokeSkin.Layouts.IndexOf(k), y => y.Name);
                BindableNotesLookup.Value = karaokeSkin.NoteSkins.ToDictionary(k => karaokeSkin.NoteSkins.IndexOf(k), y => y.Name);
            }
        }
    }
}
