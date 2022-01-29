// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using JetBrains.Annotations;
using osu.Framework.Bindables;
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
    public class KaraokeLyricEditorSkin : KaraokeSkin
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

        public KaraokeLyricEditorSkin(IStorageResourceProvider resources)
            : this(CreateInfo(), resources)
        {
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        public KaraokeLyricEditorSkin(SkinInfo skin, IStorageResourceProvider resources)
            : base(skin, resources)
        {
            BindableDefaultLyricConfig.Value = LyricConfig.DEFAULT;
            BindableDefaultLyricStyle.Value = new LyricStyle { Name = "No effect" };
            BindableDefaultNoteStyle.Value = NoteStyle.DEFAULT;

            // todo: should use better way to handle overall size.
            FontSize = 26;
        }

        protected Bindable<LyricConfig> BindableFont => BindableDefaultLyricConfig;

        public float FontSize
        {
            get => BindableFont.Value.MainTextFont.Size;
            set
            {
                float textSize = Math.Max(Math.Min(value, MAX_FONT_SIZE), MIN_FONT_SIZE);
                float changePercentage = textSize / FontSize;

                BindableFont.Value.MainTextFont
                    = multipleSize(BindableFont.Value.MainTextFont, changePercentage);
                BindableFont.Value.RubyTextFont
                    = multipleSize(BindableFont.Value.RubyTextFont, changePercentage);
                BindableFont.Value.RomajiTextFont
                    = multipleSize(BindableFont.Value.RomajiTextFont, changePercentage);

                BindableFont.TriggerChange();

                static FontUsage multipleSize(FontUsage origin, float percentage)
                    => origin.With(size: origin.Size * percentage);
            }
        }
    }
}
