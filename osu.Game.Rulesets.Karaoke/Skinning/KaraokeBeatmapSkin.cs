// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Framework.Logging;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Rulesets.Karaoke.Skinning.Groups;
using osu.Game.Rulesets.Karaoke.Skinning.MappingRoles;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    /// <summary>
    /// It's the skin for karaoke beatmap.
    /// </summary>
    public class KaraokeBeatmapSkin : KaraokeSkin
    {
        public readonly IDictionary<ElementType, IList<IKaraokeSkinElement>> Elements = new Dictionary<ElementType, IList<IKaraokeSkinElement>>();
        public readonly List<IGroup> Groups = new();
        public readonly List<IMappingRole> DefaultMappingRoles = new();

        public KaraokeBeatmapSkin(SkinInfo skin, IStorageResourceProvider resources, Stream configurationStream = null)
            : base(skin, resources, configurationStream)
        {
            SkinInfo.PerformRead(s =>
            {
                var globalSetting = CreateJsonSerializerSettings(new KaraokeSkinElementConvertor());

                // we may want to move this to some kind of async operation in the future.
                foreach (ElementType skinnableTarget in Enum.GetValues(typeof(ElementType)))
                {
                    string filename = $"{getFileNameByType(skinnableTarget)}.json";

                    try
                    {
                        Elements.Add(skinnableTarget, new List<IKaraokeSkinElement>());

                        string jsonContent = GetElementStringContentFromSkinInfo(s, filename);
                        if (string.IsNullOrEmpty(jsonContent))
                            return;

                        var deserializedContent = JsonConvert.DeserializeObject<IKaraokeSkinElement[]>(jsonContent, globalSetting);

                        if (deserializedContent == null)
                            continue;

                        Elements[skinnableTarget] = deserializedContent;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, "Failed to load skin element.");
                    }
                }

                static string getFileNameByType(ElementType elementType)
                    => elementType switch
                    {
                        ElementType.LyricConfig => "lyric-configs",
                        ElementType.LyricLayout => "lyric-layouts",
                        ElementType.LyricStyle => "lyric-styles",
                        ElementType.NoteStyle => "note-styles",
                        _ => throw new InvalidEnumArgumentException(nameof(elementType))
                    };
            });

            SkinInfo.PerformRead(s =>
            {
                const string filename = "groups.json";

                try
                {
                    string jsonContent = GetElementStringContentFromSkinInfo(s, filename);
                    if (string.IsNullOrEmpty(jsonContent))
                        return;

                    var globalSetting = CreateJsonSerializerSettings(new KaraokeSkinGroupConvertor());
                    var deserializedContent = JsonConvert.DeserializeObject<IGroup[]>(jsonContent, globalSetting);

                    if (deserializedContent == null)
                        return;

                    Groups.AddRange(deserializedContent);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Failed to load skin element.");
                }
            });

            SkinInfo.PerformRead(s =>
            {
                const string filename = "default-mapping-roles.json";

                try
                {
                    string jsonContent = GetElementStringContentFromSkinInfo(s, filename);
                    if (string.IsNullOrEmpty(jsonContent))
                        return;

                    var globalSetting = CreateJsonSerializerSettings(new KaraokeSkinMappingRoleConvertor());
                    var deserializedContent = JsonConvert.DeserializeObject<IMappingRole[]>(jsonContent, globalSetting);

                    if (deserializedContent == null)
                        return;

                    DefaultMappingRoles.AddRange(deserializedContent);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Failed to load skin element.");
                }
            });
        }

        public override IBindable<TValue> GetConfig<TLookup, TValue>(TLookup lookup)
        {
            switch (lookup)
            {
                // get the target element by hit object.
                case KaraokeHitObject hitObject:
                {
                    var type = typeof(TValue);
                    var element = GetElementByHitObjectAndElementType(hitObject, type);
                    return SkinUtils.As<TValue>(new Bindable<TValue>((TValue)element));
                }

                // in some cases, we still need to get target of element by type and id.
                // e.d: get list of layout in the skin manager.
                case KaraokeSkinLookup skinLookup:
                {
                    var type = skinLookup.Type;
                    int lookupNumber = skinLookup.Lookup;
                    if (lookupNumber < 0)
                        return base.GetConfig<KaraokeSkinLookup, TValue>(skinLookup);

                    var element = Elements[type].FirstOrDefault(x => x.ID == lookupNumber);
                    return SkinUtils.As<TValue>(new Bindable<TValue>((TValue)element));
                }

                // Lookup list of name by type
                case KaraokeIndexLookup indexLookup:
                    return indexLookup switch
                    {
                        KaraokeIndexLookup.Layout => SkinUtils.As<TValue>(getSelectionFromElementType(ElementType.LyricLayout)),
                        KaraokeIndexLookup.Style => SkinUtils.As<TValue>(getSelectionFromElementType(ElementType.LyricStyle)),
                        KaraokeIndexLookup.Note => SkinUtils.As<TValue>(getSelectionFromElementType(ElementType.NoteStyle)),
                        _ => throw new InvalidEnumArgumentException(nameof(indexLookup))
                    };

                case KaraokeSkinConfigurationLookup skinConfigurationLookup:
                    return base.GetConfig<KaraokeSkinConfigurationLookup, TValue>(skinConfigurationLookup);
            }

            return null;

            Bindable<IDictionary<int, string>> getSelectionFromElementType(ElementType elementType)
                => new(Elements[elementType].ToDictionary(k => k.ID, k => k.Name));
        }

        protected override IKaraokeSkinElement GetElementByHitObjectAndElementType(KaraokeHitObject hitObject, Type elementType)
        {
            var type = KaraokeSkinElementConvertor.GetElementType(elementType);
            var firstMatchedRole = DefaultMappingRoles.FirstOrDefault(x => x.CanApply(this, hitObject, type));

            if (firstMatchedRole == null)
                return base.GetElementByHitObjectAndElementType(hitObject, elementType);

            int elementId = firstMatchedRole.ElementId;
            var element = ToElement(type, elementId);
            return element;
        }

        protected IKaraokeSkinElement ToElement(ElementType type, int id)
        {
            var elements = Elements[type];
            if (elements == null)
                throw new ArgumentNullException(nameof(elements));

            var element = elements.FirstOrDefault(x => x.ID == id);
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            // should make sure that must be able to found element from the skin.
            return element;
        }
    }
}
