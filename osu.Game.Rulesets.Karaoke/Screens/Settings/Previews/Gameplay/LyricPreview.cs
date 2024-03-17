// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Sprites;
using osu.Framework.IO.Stores;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.IO.Stores;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Karaoke.Skinning.Fonts;
using osu.Game.Rulesets.Karaoke.UI;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings.Previews.Gameplay;

public partial class LyricPreview : SettingsSubsectionPreview
{
    private readonly Bindable<FontUsage> mainFont = new();
    private readonly Bindable<FontUsage> rubyFont = new();
    private readonly Bindable<FontUsage> romanisationFont = new();
    private readonly Bindable<FontUsage> translateFont = new();
    private readonly Bindable<CultureInfo> preferLanguage = new();

    [Resolved]
    private FontStore fontStore { get; set; } = null!;

    private KaraokeLocalFontStore localFontStore = null!;

    private readonly LyricPlayfield lyricPlayfield;
    private readonly Lyric lyric;

    public LyricPreview()
    {
        Size = new Vector2(0.7f, 0.5f);

        Child = new Container
        {
            RelativeSizeAxes = Axes.Both,
            Padding = new MarginPadding(30),
            Child = lyricPlayfield = new LyricPlayfield
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            },
        };
        lyricPlayfield.Add(lyric = createPreviewLyric());

        mainFont.BindValueChanged(e =>
        {
            addFont(e.NewValue);
        });
        rubyFont.BindValueChanged(e =>
        {
            addFont(e.NewValue);
        });
        romanisationFont.BindValueChanged(e =>
        {
            addFont(e.NewValue);
        });
        translateFont.BindValueChanged(e =>
        {
            addFont(e.NewValue);
        });
        preferLanguage.BindValueChanged(e =>
        {
            lyric.Translates = createPreviewTranslate(e.NewValue);
        });

        void addFont(FontUsage fontUsage)
            => localFontStore.AddFont(fontUsage);
    }

    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
    {
        var baseDependencies = new DependencyContainer(base.CreateChildDependencies(parent));
        baseDependencies.Cache(new KaraokeSessionStatics(baseDependencies.Get<KaraokeRulesetConfigManager>(), null));
        return baseDependencies;
    }

    [BackgroundDependencyLoader]
    private void load(FontManager fontManager, IRenderer renderer, KaraokeRulesetConfigManager config)
    {
        // create local font store and import those files
        localFontStore = new KaraokeLocalFontStore(fontManager, renderer);
        fontStore.AddStore(localFontStore);

        // fonts
        config.BindWith(KaraokeRulesetSetting.MainFont, mainFont);
        config.BindWith(KaraokeRulesetSetting.RubyFont, rubyFont);
        config.BindWith(KaraokeRulesetSetting.RomanisationFont, romanisationFont);
        config.BindWith(KaraokeRulesetSetting.TranslateFont, translateFont);
        config.BindWith(KaraokeRulesetSetting.PreferLanguage, preferLanguage);
    }

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);

        fontStore.RemoveStore(localFontStore);
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
                    EndIndex = 0,
                    Text = "か",
                },
                new RubyTag
                {
                    StartIndex = 2,
                    EndIndex = 2,
                    Text = "お",
                },
            },
            StartTime = 0,
            Duration = 1000000,
            TimeTags = new List<TimeTag>
            {
                new(new TextIndex(0), 500)
                {
                    FirstSyllable = true,
                    RomanisedSyllable = "karaoke",
                },
                new(new TextIndex(1), 600),
                new(new TextIndex(2), 1000),
                new(new TextIndex(3), 1500),
                new(new TextIndex(4), 2000),
            },
            HitWindows = new KaraokeLyricHitWindows(),
            EffectApplier = new PreviewLyricEffectApplier(),
        };

    private IDictionary<CultureInfo, string> createPreviewTranslate(CultureInfo cultureInfo)
    {
        string translate = cultureInfo.Name switch
        {
            "ja" or "Ja-jp" => "カラオケ",
            "zh-Hant" or "zh-TW" => "卡拉OK",
            _ => "karaoke",
        };

        return new Dictionary<CultureInfo, string>
        {
            { cultureInfo, translate },
        };
    }
}
