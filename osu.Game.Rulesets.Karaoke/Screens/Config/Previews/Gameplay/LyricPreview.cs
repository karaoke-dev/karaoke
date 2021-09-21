// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.IO.Stores;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.IO.Stores;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Karaoke.Skinning.Fonts;
using osu.Game.Rulesets.Karaoke.Timing;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Previews.Gameplay
{
    public class LyricPreview : SettingsSubsectionPreview
    {
        private readonly Bindable<FontUsage> mainFont = new();
        private readonly Bindable<FontUsage> rubyFont = new();
        private readonly Bindable<FontUsage> romajiFont = new();
        private readonly Bindable<FontUsage> translateFont = new();
        private readonly Bindable<CultureInfo> preferLanguage = new();

        [Resolved]
        private FontStore fontStore { get; set; }

        private KaraokeLocalFontStore localFontStore;

        private readonly DrawableLyric drawableLyric;

        public LyricPreview()
        {
            Size = new Vector2(0.7f, 0.5f);

            // todo : should add skin support.
            Child = drawableLyric = new DrawableLyric(createPreviewLyric())
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Clock = new StopClock(0),
            };

            mainFont.BindValueChanged(e =>
            {
                addFont(e.NewValue);
            });
            rubyFont.BindValueChanged(e =>
            {
                addFont(e.NewValue);
            });
            romajiFont.BindValueChanged(e =>
            {
                addFont(e.NewValue);
            });
            translateFont.BindValueChanged(e =>
            {
                addFont(e.NewValue);
            });
            preferLanguage.BindValueChanged(e =>
            {
                drawableLyric.HitObject.Translates = createPreviewTranslate(e.NewValue);
            });

            void addFont(FontUsage fontUsage)
                => localFontStore.AddFont(fontUsage);
        }

        [BackgroundDependencyLoader]
        private void load(FontManager fontManager, KaraokeRulesetConfigManager config)
        {
            // create local font store and import those files
            localFontStore = new KaraokeLocalFontStore(fontManager);
            fontStore.AddStore(localFontStore);

            // fonts
            config.BindWith(KaraokeRulesetSetting.MainFont, mainFont);
            config.BindWith(KaraokeRulesetSetting.RubyFont, rubyFont);
            config.BindWith(KaraokeRulesetSetting.RomajiFont, romajiFont);
            config.BindWith(KaraokeRulesetSetting.TranslateFont, translateFont);
            config.BindWith(KaraokeRulesetSetting.PreferLanguage, preferLanguage);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            fontStore?.RemoveStore(localFontStore);
        }

        private Lyric createPreviewLyric()
            => new()
            {
                Text = "カラオケ",
                RubyTags = new[]
                {
                    new RubyTag
                    {
                        StartIndex = 0,
                        EndIndex = 1,
                        Text = "か"
                    },
                    new RubyTag
                    {
                        StartIndex = 2,
                        EndIndex = 3,
                        Text = "お"
                    }
                },
                RomajiTags = new[]
                {
                    new RomajiTag
                    {
                        StartIndex = 0,
                        EndIndex = 4,
                        Text = "karaoke"
                    },
                },
                HitWindows = new KaraokeHitWindows(),
            };

        private IDictionary<CultureInfo, string> createPreviewTranslate(CultureInfo cultureInfo)
        {
            var translate = cultureInfo.Name switch
            {
                "ja" or "Ja-jp" => "カラオケ",
                "zh-Hant" or "zh-TW" => "卡拉OK",
                _ => "karaoke"
            };

            return new Dictionary<CultureInfo, string>
            {
                { cultureInfo, translate },
            };
        }
    }
}
