// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.IO.Stores;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Karaoke.Timing;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Previews.Gameplay
{
    public class LyricPreview : SettingsSubsectionPreview
    {
        private readonly Bindable<bool> displayRuby = new Bindable<bool>();
        private readonly Bindable<bool> displayRomaji = new Bindable<bool>();
        private readonly Bindable<bool> useTranslate = new Bindable<bool>();
        private readonly Bindable<CultureInfo> translateLanguage = new Bindable<CultureInfo>();

        private readonly Bindable<FontUsage> mainFont = new Bindable<FontUsage>();
        private readonly Bindable<FontUsage> rubyFont = new Bindable<FontUsage>();
        private readonly Bindable<FontUsage> romajiFont = new Bindable<FontUsage>();
        private readonly Bindable<FontUsage> translateFont = new Bindable<FontUsage>();

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

            displayRuby.BindValueChanged(e =>
            {
                drawableLyric.DisplayRuby = e.NewValue;
            }, true);
            displayRomaji.BindValueChanged(e =>
            {
                drawableLyric.DisplayRomaji = e.NewValue;
            }, true);
            useTranslate.BindValueChanged(e =>
            {
                updateTranslate();
            });
            translateLanguage.BindValueChanged(e =>
            {
                updateTranslate();
            });

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

            void updateTranslate()
            {
                var translate = useTranslate.Value;
                var language = translateLanguage.Value;

                if (language != null)
                {
                    drawableLyric.HitObject.Translates = new Dictionary<CultureInfo, string>
                    {
                        { language, language.DisplayName }
                    };
                }

                drawableLyric.DisplayTranslateLanguage = translate ? language : null;
            }

            void addFont(FontUsage fontUsage)
            {
                var fontInfo = FontUsageUtils.ToFontInfo(fontUsage);
                localFontStore.AddFont(fontInfo);
            }
        }

        [BackgroundDependencyLoader]
        private void load(GameHost host, KaraokeRulesetConfigManager config)
        {
            // create local font store and import those files
            localFontStore = new KaraokeLocalFontStore(host);
            fontStore.AddStore(localFontStore);

            // ruby and romaji
            config.BindWith(KaraokeRulesetSetting.DisplayRuby, displayRuby);
            config.BindWith(KaraokeRulesetSetting.DisplayRomaji, displayRomaji);

            // translate
            config.BindWith(KaraokeRulesetSetting.UseTranslate, useTranslate);
            config.BindWith(KaraokeRulesetSetting.PreferLanguage, translateLanguage);

            // fonts
            config.BindWith(KaraokeRulesetSetting.MainFont, mainFont);
            config.BindWith(KaraokeRulesetSetting.RubyFont, rubyFont);
            config.BindWith(KaraokeRulesetSetting.RomajiFont, romajiFont);
            config.BindWith(KaraokeRulesetSetting.TranslateFont, translateFont);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            fontStore?.RemoveStore(localFontStore);
        }

        private Lyric createPreviewLyric()
            => new Lyric
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
                Translates =
                {
                    { new CultureInfo("Ja-JP"), "からおけ" },
                },
                HitWindows = new KaraokeHitWindows(),
            };
    }
}
