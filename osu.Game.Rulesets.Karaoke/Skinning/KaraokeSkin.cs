// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics.OpenGL.Textures;
using osu.Framework.Graphics.Textures;
using osu.Game.Audio;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Fonts;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Layouts;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Notes;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    /// <summary>
    /// It's the skin for karaoke ruleset.
    /// todo: should inherit ruleset skin if have.
    /// </summary>
    public class KaraokeSkin : Skin
    {
        protected readonly IDictionary<int, Bindable<LyricFont>> bindableFonts = new Dictionary<int, Bindable<LyricFont>>();
        protected readonly IDictionary<int, Bindable<LyricLayout>> bindableLayouts = new Dictionary<int, Bindable<LyricLayout>>();
        protected readonly IDictionary<int, Bindable<NoteSkin>> bindableNotes = new Dictionary<int, Bindable<NoteSkin>>();

        protected readonly Bindable<IDictionary<int, string>> bindableFontsLookup = new();
        protected readonly Bindable<IDictionary<int, string>> bindableLayoutsLookup = new();
        protected readonly Bindable<IDictionary<int, string>> bindableNotesLookup = new();

        private readonly Bindable<float> bindableColumnHeight = new(DefaultColumnBackground.COLUMN_HEIGHT);
        private readonly Bindable<float> bindableColumnSpacing = new(ScrollingNotePlayfield.COLUMN_SPACING);

        private readonly IStorageResourceProvider resources;

        public KaraokeSkin(SkinInfo skin, IStorageResourceProvider resources, Stream configurationStream = null)
            : base(skin, resources, configurationStream)
        {
            this.resources = resources;
        }

        public override ISample GetSample(ISampleInfo sampleInfo)
        {
            foreach (string lookup in sampleInfo.LookupNames)
            {
                var sample = resources.AudioManager.Samples.Get(lookup);
                if (sample != null)
                    return sample;
            }

            return null;
        }

        public override Texture GetTexture(string componentName, WrapMode wrapModeS, WrapMode wrapModeT)
            => null;

        public override IBindable<TValue> GetConfig<TLookup, TValue>(TLookup lookup)
        {
            switch (lookup)
            {
                // Lookup skin by type and index
                case KaraokeSkinLookup skinLookup:
                {
                    var config = skinLookup.Config;
                    var lookupNumber = skinLookup.Lookup;

                    return config switch
                    {
                        KaraokeSkinConfiguration.LyricStyle => SkinUtils.As<TValue>(bindableFonts[lookupNumber]),
                        KaraokeSkinConfiguration.LyricLayout => SkinUtils.As<TValue>(bindableLayouts[lookupNumber]),
                        KaraokeSkinConfiguration.NoteStyle => SkinUtils.As<TValue>(bindableNotes[lookupNumber]),
                        _ => throw new InvalidEnumArgumentException(nameof(config))
                    };
                }

                // Lookup list of name by type
                case KaraokeIndexLookup indexLookup:
                    return indexLookup switch
                    {
                        KaraokeIndexLookup.Layout => SkinUtils.As<TValue>(bindableLayoutsLookup),
                        KaraokeIndexLookup.Style => SkinUtils.As<TValue>(bindableFontsLookup),
                        KaraokeIndexLookup.Note => SkinUtils.As<TValue>(bindableNotesLookup),
                        _ => throw new InvalidEnumArgumentException(nameof(indexLookup))
                    };

                case KaraokeSkinConfigurationLookup skinConfigurationLookup:
                    switch (skinConfigurationLookup.Lookup)
                    {
                        // should use customize height for note playfield in lyric editor.
                        case LegacyKaraokeSkinConfigurationLookups.ColumnHeight:
                            return SkinUtils.As<TValue>(bindableColumnHeight);

                        // not have note playfield judgement spacing in lyric editor.
                        case LegacyKaraokeSkinConfigurationLookups.ColumnSpacing:
                            return SkinUtils.As<TValue>(bindableColumnSpacing);
                    }

                    break;
            }

            return null;
        }
    }
}
