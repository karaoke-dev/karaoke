// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics.OpenGL.Textures;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Game.Audio;
using osu.Game.Extensions;
using osu.Game.IO;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
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
        public readonly IDictionary<ElementType, IKaraokeSkinElement> DefaultElement = new Dictionary<ElementType, IKaraokeSkinElement>
        {
            { ElementType.LyricConfig, LyricConfig.CreateDefault() },
            { ElementType.LyricStyle, LyricStyle.CreateDefault() },
            { ElementType.NoteStyle, NoteStyle.CreateDefault() },
        };

        private readonly Bindable<float> bindableColumnHeight = new(DefaultColumnBackground.COLUMN_HEIGHT);
        private readonly Bindable<float> bindableColumnSpacing = new(ScrollingNotePlayfield.COLUMN_SPACING);

        private readonly IStorageResourceProvider resources;

        public KaraokeSkin(SkinInfo skin, IStorageResourceProvider resources, IResourceStore<byte[]>? storage = null)
            : base(skin, resources, storage)
        {
            this.resources = resources;

            SkinInfo.PerformRead(s =>
            {
                const string filename = "default.json";

                try
                {
                    string? jsonContent = GetElementStringContentFromSkinInfo(s, filename);
                    if (string.IsNullOrEmpty(jsonContent))
                        return;

                    var globalSetting = CreateJsonSerializerSettings(new KaraokeSkinElementConvertor());
                    var deserializedContent = JsonConvert.DeserializeObject<DefaultSkinFormat>(jsonContent, globalSetting);

                    if (deserializedContent == null)
                        return;

                    DefaultElement[ElementType.LyricConfig] = deserializedContent.LyricConfig;
                    DefaultElement[ElementType.LyricStyle] = deserializedContent.LyricStyle;
                    DefaultElement[ElementType.NoteStyle] = deserializedContent.NoteStyle;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Failed to load skin element.");
                }
            });
        }

        protected JsonSerializerSettings CreateJsonSerializerSettings(params JsonConverter[] converters)
        {
            var globalSetting = JsonSerializableExtensions.CreateGlobalSettings();
            globalSetting.ContractResolver = new SnakeCaseKeyContractResolver();
            globalSetting.Converters.AddRange(converters);
            return globalSetting;
        }

        protected string? GetElementStringContentFromSkinInfo(SkinInfo skinInfo, string filename)
        {
            // should get by file name if files is namespace resource store.
            var files = resources.Files;
            if (files == null)
                return null;

            byte[]? bytes = files is NamespacedResourceStore<byte[]> ? getFileFromNamespaceStore(files, filename) : getFileFromSkinInfo(files, skinInfo, filename);

            if (bytes == null)
                return null;

            return Encoding.UTF8.GetString(bytes);

            static byte[]? getFileFromNamespaceStore(IResourceStore<byte[]> files, string filename)
                => files.Get(filename);

            static byte[]? getFileFromSkinInfo(IResourceStore<byte[]> files, SkinInfo skinInfo, string filename)
            {
                // skin element files may be null for default skin.
                var fileInfo = skinInfo.Files.FirstOrDefault(f => f.Filename == filename);

                if (fileInfo == null)
                    return null;

                return files.Get(fileInfo.File.GetStoragePath());
            }
        }

        public override ISample? GetSample(ISampleInfo sampleInfo)
            => sampleInfo.LookupNames.Select(lookup => resources.AudioManager.Samples.Get(lookup)).FirstOrDefault(sample => sample != null);

        public override Texture? GetTexture(string componentName, WrapMode wrapModeS, WrapMode wrapModeT)
            => null;

        public override IBindable<TValue>? GetConfig<TLookup, TValue>(TLookup lookup)
        {
            switch (lookup)
            {
                // get the target element by hit object.
                case KaraokeHitObject hitObject:
                {
                    var type = typeof(TValue);
                    var element = GetElementByHitObjectAndElementType(hitObject, type);
                    return SkinUtils.As<TValue>(new Bindable<TValue>((TValue)element!));
                }

                // in some cases, we still need to get target of element by type and id.
                // e.d: get list of layout in the skin manager.
                case KaraokeSkinLookup skinLookup:
                {
                    var type = skinLookup.Type;

                    return type switch
                    {
                        ElementType.LyricStyle or ElementType.LyricConfig or ElementType.NoteStyle => SkinUtils.As<TValue>(new Bindable<TValue>((TValue)DefaultElement[type])),
                        ElementType.LyricLayout => null,
                        _ => throw new InvalidEnumArgumentException(nameof(type))
                    };
                }

                case KaraokeSkinConfigurationLookup skinConfigurationLookup:
                {
                    return skinConfigurationLookup.Lookup switch
                    {
                        // should use customize height for note playfield in lyric editor
                        LegacyKaraokeSkinConfigurationLookups.ColumnHeight => SkinUtils.As<TValue>(bindableColumnHeight),

                        // not have note playfield judgement spacing in lyric editor.
                        LegacyKaraokeSkinConfigurationLookups.ColumnSpacing => SkinUtils.As<TValue>(bindableColumnSpacing),

                        _ => null,
                    };
                }

                default:
                    return null;
            }
        }

        protected virtual IKaraokeSkinElement? GetElementByHitObjectAndElementType(KaraokeHitObject hitObject, Type elementType)
        {
            var type = KaraokeSkinElementConvertor.GetElementType(elementType);
            return toElement(type);
        }

        private IKaraokeSkinElement? toElement(ElementType type)
            => type switch
            {
                ElementType.LyricStyle or ElementType.LyricConfig or ElementType.NoteStyle => DefaultElement[type],
                ElementType.LyricLayout => null,
                _ => throw new InvalidEnumArgumentException(nameof(type))
            };

        private class DefaultSkinFormat
        {
            public LyricConfig LyricConfig { get; set; } = null!;

            public LyricStyle LyricStyle { get; set; } = null!;

            public NoteStyle NoteStyle { get; set; } = null!;
        }
    }
}
