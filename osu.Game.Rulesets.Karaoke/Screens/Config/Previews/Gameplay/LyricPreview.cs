// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Karaoke.Timing;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Previews.Gameplay
{
    public class LyricPreview : SettingsSubsectionPreview
    {
        private readonly Bindable<bool> displayRuby = new Bindable<bool>();
        private readonly Bindable<bool> displayRomaji = new Bindable<bool>();
        private readonly Bindable<bool> useTranslate = new Bindable<bool>();
        private readonly Bindable<CultureInfo> translateLanguage = new Bindable<CultureInfo>();

        private readonly DrawableLyric drawableLyric;

        public LyricPreview()
        {
            Size = new Vector2(0.7f, 0.5f);

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
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetConfigManager config)
        {
            // ruby and romaji
            config.BindWith(KaraokeRulesetSetting.DisplayRuby, displayRuby);
            config.BindWith(KaraokeRulesetSetting.DisplayRomaji, displayRomaji);

            // translate
            config.BindWith(KaraokeRulesetSetting.UseTranslate, useTranslate);
            config.BindWith(KaraokeRulesetSetting.PreferLanguage, translateLanguage);
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
