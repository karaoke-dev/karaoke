// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Sprites;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    /// <summary>
    /// This karaoke skin is using in lyric editor only.
    /// </summary>
    public class LyricEditorSkin : KaraokeSkin
    {
        public const int MIN_FONT_SIZE = 10;
        public const int MAX_FONT_SIZE = 45;

        internal static readonly Guid DEFAULT_SKIN = new("FEC5A290-5709-11EC-9F10-0800200C9A66");

        public static SkinInfo CreateInfo() => new()
        {
            ID = DEFAULT_SKIN,
            Name = "karaoke! (default editor skin)",
            Creator = "team karaoke!",
            Protected = true,
            InstantiationInfo = typeof(DefaultKaraokeSkin).GetInvariantInstantiationInfo(),
        };

        public LyricEditorSkin(IStorageResourceProvider resources)
            : this(CreateInfo(), resources)
        {
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        public LyricEditorSkin(SkinInfo skin, IStorageResourceProvider resources)
            : base(skin, resources)
        {
            DefaultElement[ElementType.LyricConfig] = LyricConfig.CreateDefault();
            DefaultElement[ElementType.LyricStyle] = new LyricStyle
            {
                Name = "Default",
                LeftLyricTextShaders = new List<ICustomizedShader>
                {
                    new OutlineShader
                    {
                        Radius = 2,
                        Colour = Color4Extensions.FromHex("#3D2D6B"),
                        OutlineColour = Color4Extensions.FromHex("#CCA532")
                    },
                },
                RightLyricTextShaders = new List<ICustomizedShader>
                {
                    new OutlineShader
                    {
                        Radius = 2,
                        OutlineColour = Color4Extensions.FromHex("#5932CC")
                    },
                }
            };
            DefaultElement[ElementType.NoteStyle] = NoteStyle.CreateDefault();

            // todo: should use better way to handle overall size.
            FontSize = 26;
        }

        protected LyricConfig LyricConfig => DefaultElement[ElementType.LyricConfig] as LyricConfig;

        public float FontSize
        {
            get => LyricConfig.MainTextFont.Size;
            set
            {
                float textSize = Math.Max(Math.Min(value, MAX_FONT_SIZE), MIN_FONT_SIZE);
                float changePercentage = textSize / FontSize;

                LyricConfig.MainTextFont
                    = multipleSize(LyricConfig.MainTextFont, changePercentage);
                LyricConfig.RubyTextFont
                    = multipleSize(LyricConfig.RubyTextFont, changePercentage);
                LyricConfig.RomajiTextFont
                    = multipleSize(LyricConfig.RomajiTextFont, changePercentage);

                // todo: change size might not working now.
                // DefaultElement[ElementType.LyricConfig].TriggerChange();

                static FontUsage multipleSize(FontUsage origin, float percentage)
                    => origin.With(size: origin.Size * percentage);
            }
        }
    }
}
