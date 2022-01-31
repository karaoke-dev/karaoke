// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics.OpenGL.Textures;
using osu.Framework.Graphics.Textures;
using osu.Game.Audio;
using osu.Game.IO;
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
        public readonly IDictionary<ElementType, IKaraokeSkinElement> DefaultElement = new Dictionary<ElementType, IKaraokeSkinElement>();

        private readonly Bindable<float> bindableColumnHeight = new(DefaultColumnBackground.COLUMN_HEIGHT);
        private readonly Bindable<float> bindableColumnSpacing = new(ScrollingNotePlayfield.COLUMN_SPACING);

        private readonly IStorageResourceProvider resources;

        public KaraokeSkin(SkinInfo skin, IStorageResourceProvider resources, Stream configurationStream = null)
            : base(skin, resources, configurationStream)
        {
            this.resources = resources;

            SkinInfo.PerformRead(s =>
            {
                // we may want to move this to some kind of async operation in the future.
                foreach (ElementType skinnableTarget in Enum.GetValues(typeof(ElementType)))
                {
                    if (skinnableTarget == ElementType.LyricLayout)
                        return;

                    // todo: load the target from skin info.
                    DefaultElement.Add(skinnableTarget, null);
                }
            });
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

                    return type switch
                    {
                        ElementType.LyricStyle or ElementType.LyricConfig or ElementType.NoteStyle => SkinUtils.As<TValue>(new Bindable<TValue>((TValue)DefaultElement[type])),
                        ElementType.LyricLayout => null,
                        _ => throw new InvalidEnumArgumentException(nameof(type))
                    };
                }

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

        protected virtual IKaraokeSkinElement GetElementByHitObjectAndElementType(KaraokeHitObject hitObject, Type elementType)
        {
            var type = GetElementType(elementType);
            return ToElement(type);
        }

        protected static ElementType GetElementType(Type elementType)
        {
            return elementType switch
            {
                var type when type == typeof(LyricConfig) => ElementType.LyricConfig,
                var type when type == typeof(LyricLayout) => ElementType.LyricLayout,
                var type when type == typeof(LyricStyle) => ElementType.LyricStyle,
                var type when type == typeof(NoteStyle) => ElementType.NoteStyle,
                _ => throw new NotSupportedException()
            };
        }

        protected IKaraokeSkinElement ToElement(ElementType type)
            => type switch
            {
                ElementType.LyricStyle or ElementType.LyricConfig or ElementType.NoteStyle => DefaultElement[type],
                ElementType.LyricLayout => null,
                _ => throw new InvalidEnumArgumentException(nameof(type))
            };
    }
}
