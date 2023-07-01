// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Translate.Components;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Translate;

[Cached(typeof(ITranslateInfoProvider))]
public partial class TranslateEditSection : Container, ITranslateInfoProvider
{
    private const int row_height = 50;
    private const int column_spacing = 10;
    private const int row_inner_spacing = 10;

    private readonly CornerBackground timeSectionBackground;
    private readonly CornerBackground lyricSectionBackground;
    private readonly LanguageDropdown languageDropdown;

    [Cached(typeof(IBindable<CultureInfo?>))]
    private readonly Bindable<CultureInfo?> currentLanguage = new();

    [Resolved]
    private IBeatmapLanguagesChangeHandler beatmapLanguagesChangeHandler { get; set; } = null!;

    private readonly IBindableList<Lyric> bindableLyrics = new BindableList<Lyric>();

    public TranslateEditSection()
    {
        Padding = new MarginPadding(10);

        var columnDimensions = new[]
        {
            new Dimension(GridSizeMode.Absolute, 200),
            new Dimension(GridSizeMode.Absolute, column_spacing),
            new Dimension(GridSizeMode.Absolute, 400),
            new Dimension(GridSizeMode.Absolute, column_spacing),
            new Dimension()
        };
        GridContainer translateGrid;

        Child = new FillFlowContainer
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Children = new Drawable[]
            {
                new GridContainer
                {
                    Name = "LanguageSelection",
                    RowDimensions = new[]
                    {
                        new Dimension(GridSizeMode.AutoSize)
                    },
                    ColumnDimensions = columnDimensions,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Content = new[]
                    {
                        new[]
                        {
                            Empty(),
                            Empty(),
                            Empty(),
                            Empty(),
                            new GridContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                ColumnDimensions = new[]
                                {
                                    new Dimension(),
                                    new Dimension(GridSizeMode.Absolute, column_spacing),
                                    new Dimension(GridSizeMode.Absolute, 50),
                                    new Dimension(GridSizeMode.Absolute, column_spacing),
                                    new Dimension(GridSizeMode.Absolute, 50),
                                },
                                RowDimensions = new[]
                                {
                                    new Dimension(GridSizeMode.AutoSize),
                                },
                                Content = new[]
                                {
                                    new[]
                                    {
                                        languageDropdown = new LanguageDropdown
                                        {
                                            RelativeSizeAxes = Axes.X,
                                        },
                                        Empty(),
                                        new CreateNewLanguageButton
                                        {
                                            Y = 5,
                                        },
                                        Empty(),
                                        new RemoveLanguageButton
                                        {
                                            Y = 5,
                                        },
                                    }
                                }
                            }
                        },
                    }
                },
                new Container
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Children = new[]
                    {
                        new GridContainer
                        {
                            Name = "Background",
                            RowDimensions = new[]
                            {
                                new Dimension(GridSizeMode.AutoSize)
                            },
                            ColumnDimensions = columnDimensions,
                            RelativeSizeAxes = Axes.Both,
                            Content = new[]
                            {
                                new[]
                                {
                                    new CornerBackground
                                    {
                                        Alpha = 0,
                                    },
                                    null,
                                    null,
                                    null,
                                    null,
                                },
                                new[]
                                {
                                    timeSectionBackground = new CornerBackground
                                    {
                                        RelativeSizeAxes = Axes.Both
                                    },
                                    null,
                                    lyricSectionBackground = new CornerBackground
                                    {
                                        RelativeSizeAxes = Axes.Both
                                    },
                                    null,
                                    null,
                                },
                            }
                        },
                        translateGrid = new GridContainer
                        {
                            Name = "Translates",
                            ColumnDimensions = columnDimensions,
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                        }
                    }
                }
            },
        };

        languageDropdown.Current.BindValueChanged(x =>
        {
            // should use currentLanguage.BindTo(languageDropdown.Current); once bindable is not nullable again.
            currentLanguage.Value = x.NewValue;
        });

        bindableLyrics.BindCollectionChanged((_, _) =>
        {
            // just re-create all the view, lazy to save the performance in here.
            translateGrid.RowDimensions = bindableLyrics.Select(_ => new Dimension(GridSizeMode.Absolute, row_height)).ToArray();
            translateGrid.Content = createContent();
        });
    }

    [BackgroundDependencyLoader]
    private void load(ILyricsProvider lyricsProvider, OverlayColourProvider colourProvider)
    {
        languageDropdown.ItemSource = beatmapLanguagesChangeHandler.Languages;

        bindableLyrics.BindTo(lyricsProvider.BindableLyrics);

        timeSectionBackground.Colour = colourProvider.Background6;
        lyricSectionBackground.Colour = colourProvider.Dark6;
    }

    private Drawable[][] createContent()
    {
        return bindableLyrics.Select(x =>
        {
            return new[]
            {
                createTimeDrawable(x),
                Empty(),
                createPreviewSpriteText(x),
                Empty(),
                createTranslateTextBox(x),
            };
        }).ToArray();
    }

    private Drawable createTimeDrawable(Lyric lyric)
    {
        return new OsuSpriteText
        {
            Text = LyricUtils.LyricTimeFormattedString(lyric),
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            Margin = new MarginPadding { Left = row_inner_spacing },
            Font = OsuFont.GetFont(size: 18, fixedWidth: true),
            AllowMultiline = false
        };
    }

    private Drawable createPreviewSpriteText(Lyric lyric) =>
        new TranslateLyricSpriteText(lyric)
        {
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            RelativeSizeAxes = Axes.X,
            AllowMultiline = false,
            Truncate = true,
            Padding = new MarginPadding { Left = row_inner_spacing },
            Font = new FontUsage(size: 25),
            RubyFont = new FontUsage(size: 10),
            RomajiFont = new FontUsage(size: 10),
        };

    private Drawable createTranslateTextBox(Lyric lyric) =>
        new LyricTranslateTextBox(lyric)
        {
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            RelativeSizeAxes = Axes.X,
            TabbableContentContainer = this,
            CommitOnFocusLost = true,
        };

    public string? GetLyricTranslate(Lyric lyric, CultureInfo cultureInfo)
    {
        ArgumentNullException.ThrowIfNull(cultureInfo);

        return lyric.Translates.TryGetValue(cultureInfo, out string? translate) ? translate : null;
    }
}
