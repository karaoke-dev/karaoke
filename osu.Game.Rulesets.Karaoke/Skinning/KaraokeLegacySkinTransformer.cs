// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class KaraokeLegacySkinTransformer : LegacySkinTransformer
    {
        private readonly ISkin source;

        private readonly IDictionary<int, Bindable<KaraokeFont>> bindableFonts = new Dictionary<int, Bindable<KaraokeFont>>();
        private readonly IDictionary<int, Bindable<KaraokeLayout>> bindableLayouts = new Dictionary<int, Bindable<KaraokeLayout>>();
        private readonly IDictionary<int, Bindable<NoteSkin>> bindableNotes = new Dictionary<int, Bindable<NoteSkin>>();
        private readonly IDictionary<int, Bindable<Singer>> bindableSingers = new Dictionary<int, Bindable<Singer>>();

        private readonly Bindable<IDictionary<int, string>> bindableFontsLookup = new Bindable<IDictionary<int, string>>();
        private readonly Bindable<IDictionary<int, string>> bindableLayoutsLookup = new Bindable<IDictionary<int, string>>();
        private readonly Bindable<IDictionary<int, string>> bindableNotesLookup = new Bindable<IDictionary<int, string>>();
        private readonly Bindable<IDictionary<int, string>> bindableSingersLookup = new Bindable<IDictionary<int, string>>();

        private Lazy<bool> isLegacySkin;

        public KaraokeLegacySkinTransformer(ISkinSource source)
            : base(source)
        {
            this.source = source;

            if (source != null)
                source.SourceChanged += sourceChanged;
            sourceChanged();

            // TODO : need a better way to load resource
            var assembly = Assembly.GetExecutingAssembly();
            const string resource_name = @"osu.Game.Rulesets.Karaoke.Resources.Skin.default.skin";

            using (var stream = assembly.GetManifestResourceStream(resource_name))
            using (var reader = new LineBufferedReader(stream))
            {
                var skin = new KaraokeSkinDecoder().Decode(reader);

                // Create bindables
                for (int i = 0; i < skin.Fonts.Count; i++)
                    bindableFonts.Add(i, new Bindable<KaraokeFont>(skin.Fonts[i]));
                for (int i = 0; i < skin.Layouts.Count; i++)
                    bindableLayouts.Add(i, new Bindable<KaraokeLayout>(skin.Layouts[i]));
                for (int i = 0; i < skin.NoteSkins.Count; i++)
                    bindableNotes.Add(i, new Bindable<NoteSkin>(skin.NoteSkins[i]));
                for (int i = 0; i < skin.Singers.Count; i++)
                    bindableSingers.Add(i, new Bindable<Singer>(skin.Singers[i]));

                // Create lookups
                bindableFontsLookup.Value = skin.Fonts.ToDictionary(k => skin.Fonts.IndexOf(k), y => y.Name);
                bindableLayoutsLookup.Value = skin.Layouts.ToDictionary(k => skin.Layouts.IndexOf(k), y => y.Name);
                bindableNotesLookup.Value = skin.NoteSkins.ToDictionary(k => skin.NoteSkins.IndexOf(k), y => y.Name);
                bindableSingersLookup.Value = skin.Singers.ToDictionary(k => skin.Singers.IndexOf(k), y => y.Name);
            }
        }

        private void sourceChanged()
        {
            isLegacySkin = new Lazy<bool>(() => source?.GetConfig<LegacySkinConfiguration.LegacySetting, decimal>(LegacySkinConfiguration.LegacySetting.Version) != null);
        }

        public override Drawable GetDrawableComponent(ISkinComponent component)
        {
            if (!(component is KaraokeSkinComponent karaokeComponent))
                return null;

            if (!isLegacySkin.Value)
                return null;

            switch (karaokeComponent.Component)
            {
                case KaraokeSkinComponents.ColumnBackground:
                    if (textureExist(LegacyColumnBackground.GetTextureName()))
                        return new LegacyColumnBackground();

                    return null;

                case KaraokeSkinComponents.StageBackground:
                    if (textureExist(LegacyStageBackground.GetTextureName()))
                        return new LegacyStageBackground();

                    return null;

                case KaraokeSkinComponents.JudgementLine:
                    var judgementLine = LegacyJudgementLine.GetTextureNameFromLookup(LegacyKaraokeSkinConfigurationLookups.JudgementLineBodyImage);
                    if (textureExist(judgementLine))
                        return new LegacyJudgementLine();

                    return null;

                case KaraokeSkinComponents.Note:
                    var foregroundBody = LegacyNotePiece.GetTextureNameFromLookup(LegacyKaraokeSkinConfigurationLookups.NoteBodyImage, LegacyKaraokeSkinNoteLayer.Foreground);
                    var backgroundBody = LegacyNotePiece.GetTextureNameFromLookup(LegacyKaraokeSkinConfigurationLookups.NoteBodyImage, LegacyKaraokeSkinNoteLayer.Background);
                    if (textureExist(foregroundBody, backgroundBody))
                        return new LegacyNotePiece();

                    return null;

                case KaraokeSkinComponents.HitExplosion:
                    if (animationExist(LegacyHitExplosion.GetTextureName()))
                        return new LegacyHitExplosion();

                    return null;
            }

            return null;
        }

        private bool textureExist(params string[] textureNames)
            => textureNames.All(x => source.GetTexture(x) != null);

        private bool animationExist(params string[] textureNames)
            => textureNames.All(x => source.GetAnimation(x, true, false) != null);

        public override IBindable<TValue> GetConfig<TLookup, TValue>(TLookup lookup)
        {
            switch (lookup)
            {
                // Lookup skin by type and index
                case KaraokeSkinLookup skinLookup:
                {
                    var config = skinLookup.Config;
                    var lookupNumber = skinLookup.Lookup;

                    switch (config)
                    {
                        case KaraokeSkinConfiguration.LyricStyle:
                            return SkinUtils.As<TValue>(bindableFonts[lookupNumber]);

                        case KaraokeSkinConfiguration.LyricLayout:
                            return SkinUtils.As<TValue>(bindableLayouts[lookupNumber]);

                        case KaraokeSkinConfiguration.NoteStyle:
                            return SkinUtils.As<TValue>(bindableNotes[lookupNumber]);

                        case KaraokeSkinConfiguration.Singer:
                            return SkinUtils.As<TValue>(bindableSingers[lookupNumber]);
                    }

                    break;
                }

                // Lookup list of name by type
                case KaraokeIndexLookup indexLookup:
                    switch (indexLookup)
                    {
                        case KaraokeIndexLookup.Layout:
                            return SkinUtils.As<TValue>(bindableLayoutsLookup);

                        case KaraokeIndexLookup.Style:
                            return SkinUtils.As<TValue>(bindableFontsLookup);

                        case KaraokeIndexLookup.Note:
                            return SkinUtils.As<TValue>(bindableNotesLookup);

                        case KaraokeIndexLookup.Singer:
                            return SkinUtils.As<TValue>(bindableSingersLookup);
                    }

                    break;
            }

            return source.GetConfig<TLookup, TValue>(lookup);
        }
    }
}
